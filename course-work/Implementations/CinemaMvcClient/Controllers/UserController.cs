using CinemaMvcClient.DTO_s.UserDTO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

public class UserController : Controller
{
    private readonly HttpClient _httpClient;

    public UserController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("ApiWithToken");
    }

    [AllowAnonymous]
    public IActionResult Register() => View();

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Register(RegisterUserDTO model)
    {
        if (!ModelState.IsValid) return View(model);

        var response = await _httpClient.PostAsJsonAsync("user/register", model);

        if (response.IsSuccessStatusCode)
        {
            TempData["SuccessMessage"] = "Registration successful! Please login.";
            return RedirectToAction("Login");
        }

        var errorMessage = await response.Content.ReadAsStringAsync();
        ModelState.AddModelError(string.Empty, errorMessage);
        return View(model);
    }

    [AllowAnonymous]
    public IActionResult Login() => View();

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login(LoginUserDTO model)
    {
        if (!ModelState.IsValid) return View(model);

        var response = await _httpClient.PostAsJsonAsync("user/login", model);

        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(jsonString);
            var token = doc.RootElement.GetProperty("token").GetString();

            if (!string.IsNullOrEmpty(token))
            {
                // Save token in session
                HttpContext.Session.SetString("JWTToken", token);

                // Parse JWT to get claims
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var claims = new List<Claim>();

                // Add claims from JWT payload
                foreach (var claim in jwtToken.Claims)
                {
                    claims.Add(claim);
                }

                // Create identity and principal
                var identity = new ClaimsIdentity(claims, "CookieAuth");
                var principal = new ClaimsPrincipal(identity);

                // Sign in the user (create auth cookie)
                await HttpContext.SignInAsync("CookieAuth", principal);

                return RedirectToAction("Index", "Home");
            }
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View(model);
    }


    [Authorize]
    public async Task<IActionResult> Logout()
    {
        HttpContext.Session.Remove("JWTToken");
        await HttpContext.SignOutAsync("CookieAuth");
        return RedirectToAction("Index", "Home");
    }


    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> Index(int page = 1, int itemsPerPage = 10)
    {
        var response = await _httpClient.GetAsync($"user?page={page}&itemsPerPage={itemsPerPage}");

        if (response.IsSuccessStatusCode)
        {
            var users = await response.Content.ReadFromJsonAsync<PagedUsersDTO>();
            return View(users);
        }

        return View("Error");
    }

    [Authorize]
    public async Task<IActionResult> Details(int id)
    {
        var response = await _httpClient.GetAsync($"user/{id}");

        if (response.IsSuccessStatusCode)
        {
            var user = await response.Content.ReadFromJsonAsync<UserDTO>();
            return View(user);
        }

        return NotFound();
    }

    [Authorize]
   
    public async Task<IActionResult> Edit(int id)
    {
        var response = await _httpClient.GetAsync($"user/{id}");

        if (response.IsSuccessStatusCode)
        {
            var user = await response.Content.ReadFromJsonAsync<UserDTO>();
            var updateDto = new UpdateUserDTO
            {
                Username = user.Username,
                FullName = user.FullName
            };
            return View(updateDto);
        }

        return NotFound();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Edit(int id, UpdateUserDTO model)
    {
        if (!ModelState.IsValid) return View(model);

        var response = await _httpClient.PutAsJsonAsync($"user/{id}", model);

        if (response.IsSuccessStatusCode)
        {
            TempData["SuccessMessage"] = "Profile updated successfully!";
            return RedirectToAction("Details", new { id });
        }

        if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            return Forbid();

        ModelState.AddModelError(string.Empty, "Error updating profile.");
        return View(model);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _httpClient.DeleteAsync($"user/{id}");

        if (response.IsSuccessStatusCode)
        {
            TempData["SuccessMessage"] = "User deleted successfully!";
            return RedirectToAction("Index");
        }

        return NotFound();
    }

    [Authorize]
    public async Task<IActionResult> Search(string username, string email)
    {
        var response = await _httpClient.GetAsync($"user/search?username={username}&email={email}");

        if (response.IsSuccessStatusCode)
        {
            var users = await response.Content.ReadFromJsonAsync<List<UserDTO>>();
            return View("SearchResults", users);
        }

        return View("SearchResults", new List<UserDTO>());
    }
}
