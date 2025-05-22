using CinemaAPI.DTO_s.UserDTO;
using CinemaAPI.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService userService;

    public UserController(IUserService userService)
    {
        this.userService = userService;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Username) ||
            string.IsNullOrWhiteSpace(dto.Password) ||
            string.IsNullOrWhiteSpace(dto.FullName) ||
            string.IsNullOrWhiteSpace(dto.Email))
        {
            return BadRequest("All fields are required.");
        }

        try
        {
            var result = await userService.Register(dto);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
            return BadRequest("Username and password are required.");

        var token = await userService.Login(dto);
        if (token == null)
            return Unauthorized("Invalid username or password.");

        return Ok(new
        {
            message = "Login successful.",
            token = token
        });
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<List<UserDTO>>> GetAll()
    {
        var users = await userService.GetAll();
        return Ok(users);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDTO>> GetById(int id)
    {
        var user = await userService.GetById(id);
        if (user == null)
            return NotFound();

        return Ok(user);
    }

    [Authorize]
    [HttpGet("by-username")]
    public async Task<ActionResult<UserDTO>> GetByUsername([FromQuery] string username)
    {
        var user = await userService.GetByUsername(username);
        if (user == null)
            return NotFound();

        return Ok(user);
    }

    [Authorize]
    [HttpGet("by-email")]
    public async Task<ActionResult<UserDTO>> GetByEmail([FromQuery] string email)
    {
        var user = await userService.GetByEmail(email);
        if (user == null)
            return NotFound();

        return Ok(user);
    }

    [Authorize]
    [HttpGet("search")]
    public IActionResult SearchUsers([FromQuery] string? username, [FromQuery] string? email)
    {
        var result = userService.Search(username, email);
        return Ok(result);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDTO dto)
    {
        var isAdmin = User.IsInRole("Admin");
        var userIdFromToken = int.Parse(User.FindFirst("id")?.Value ?? "0");

        try
        {
            var updatedUser = await userService.Update(id, dto, userIdFromToken, isAdmin);
            if (updatedUser == null)
                return NotFound();

            return Ok(updatedUser);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await userService.Delete(id);
        if (!success)
            return NotFound();

        return Ok("User deleted successfully.");
    }
}
