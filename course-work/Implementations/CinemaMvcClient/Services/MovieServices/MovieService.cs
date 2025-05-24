
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CinemaMvcClient.Services.MovieServices;
using CinemaMvcClient.DTO_s.Movie;
using CinemaMvcClient.DTO_s;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace CinemaMvcClient.Services
{
    public class MovieService : IMovieService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://localhost:7051/api/movie/";

        public MovieService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        private void SetAuthorizationHeader(string token)
        {
            if (_httpClient.DefaultRequestHeaders.Authorization?.Parameter != token)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
        public async Task<PagedMoviesDTO> GetAllMoviesAsync(PaginationParams pagination)
        {
            var url = $"{_baseUrl}?page={pagination.Page}&itemsPerPage={pagination.ItemsPerPage}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PagedMoviesDTO>(content);
        }

        public async Task<MovieDTO> GetMovieByIdAsync(string token,int id)
        {
            SetAuthorizationHeader(token);
            var url = $"{_baseUrl}{id}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<MovieDTO>(content);
        }

        public async Task<PagedMoviesDTO> GetMoviesByGenreAsync(string genre, PaginationParams pagination)
        {
            var url = $"{_baseUrl}genre?genre={genre}&page={pagination.Page}&itemsPerPage={pagination.ItemsPerPage}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PagedMoviesDTO>(content);
        }

        public async Task<PagedMoviesDTO> GetMoviesByYearAsync(int year, PaginationParams pagination)
        {
            var url = $"{_baseUrl}year?year={year}&page={pagination.Page}&itemsPerPage={pagination.ItemsPerPage}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PagedMoviesDTO>(content);
        }

        public async Task<PagedMoviesDTO> GetMoviesWith3DAsync(PaginationParams pagination)
        {
            var url = $"{_baseUrl}with3d?page={pagination.Page}&itemsPerPage={pagination.ItemsPerPage}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PagedMoviesDTO>(content);
        }

        public async Task<PagedMoviesDTO> SearchMoviesAsync(DateTime? releaseDateFrom, DateTime? releaseDateTo, PaginationParams pagination)
        {
            var queryParams = new List<string>();

            if (releaseDateFrom.HasValue)
                queryParams.Add($"releaseDateFrom={releaseDateFrom.Value:yyyy-MM-dd}");

            if (releaseDateTo.HasValue)
                queryParams.Add($"releaseDateTo={releaseDateTo.Value:yyyy-MM-dd}");

            queryParams.Add($"page={pagination.Page}");
            queryParams.Add($"itemsPerPage={pagination.ItemsPerPage}");

            var url = $"{_baseUrl}search?" + string.Join("&", queryParams);

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PagedMoviesDTO>(content);
        }


        public async Task<MovieDTO> CreateMovieAsync(string token,CreateMovieDTO movie)
        {
            SetAuthorizationHeader(token);
            var json = JsonConvert.SerializeObject(movie);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_baseUrl, content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<MovieDTO>(responseContent);
        }

        public async Task<MovieDTO> UpdateMovieAsync(string token,int id, UpdateMovieDTO movie)
        {
            SetAuthorizationHeader(token);
            var json = JsonConvert.SerializeObject(movie);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_baseUrl}{id}", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<MovieDTO>(responseContent);
        }

        public async Task<bool> DeleteMovieAsync(string token,int id)
        {
            SetAuthorizationHeader(token);
            var response = await _httpClient.DeleteAsync($"{_baseUrl}{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
