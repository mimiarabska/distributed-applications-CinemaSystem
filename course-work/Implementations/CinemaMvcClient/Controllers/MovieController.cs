using CinemaMvcClient.DTO_s.Movie;
using CinemaMvcClient.DTO_s;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CinemaMvcClient.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MovieController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public MovieController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiWithToken");
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<IActionResult> Index(int page = 1, int itemsPerPage = 10)
        {
            var response = await _httpClient.GetAsync($"movie?page={page}&itemsPerPage={itemsPerPage}");
            if (!response.IsSuccessStatusCode) return View("Error");

            var json = await response.Content.ReadAsStringAsync();
            var movies = JsonSerializer.Deserialize<PagedMoviesDTO>(json, _jsonOptions);

            return View(movies);
        }

        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"movie/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var movie = JsonSerializer.Deserialize<MovieDTO>(json, _jsonOptions);

            return View(movie);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMovieDTO dto)
        {
            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("movie", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            return View(dto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"movie/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var movie = JsonSerializer.Deserialize<MovieDTO>(json, _jsonOptions);

            var updateDto = new UpdateMovieDTO
            {
                Description = movie.Description,
                Is3D = movie.Is3D
            };

            ViewData["MovieId"] = movie.Id;
            return View(updateDto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdateMovieDTO dto)
        {
            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"movie/{id}", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            ViewData["MovieId"] = id;
            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.GetAsync($"movie/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var movie = JsonSerializer.Deserialize<MovieDTO>(json, _jsonOptions);

            return View(movie);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(MovieDTO model)
        {
            var response = await _httpClient.DeleteAsync($"movie/{model.Id}");
            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            return NotFound();
        }
        // Допълнителни методи

        public async Task<IActionResult> FilterByYear(int year)
        {
            var response = await _httpClient.GetAsync($"movie/year?year={year}");
            if (!response.IsSuccessStatusCode) return View("Error");

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<PagedMoviesDTO>(json, _jsonOptions);

            return View("Index", result);
        }

        public async Task<IActionResult> FilterByGenre(string genre)
        {
            var response = await _httpClient.GetAsync($"movie/genre?genre={genre}");
            if (!response.IsSuccessStatusCode) return View("Error");

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<PagedMoviesDTO>(json, _jsonOptions);

            return View("Index", result);
        }

        public async Task<IActionResult> Filter3D()
        {
            var response = await _httpClient.GetAsync($"movie/3D");
            if (!response.IsSuccessStatusCode) return View("Error");

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<PagedMoviesDTO>(json, _jsonOptions);

            return View("Index", result);
        }

        public async Task<IActionResult> Search(DateTime? releaseDateFrom, DateTime? releaseDateTo)
        {
            var url = $"movie/search?releaseDateFrom={releaseDateFrom}&releaseDateTo={releaseDateTo}";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return View("Error");

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<PagedMoviesDTO>(json, _jsonOptions);

            return View("Index", result);
        }
    }
}
