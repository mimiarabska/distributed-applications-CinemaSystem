
using CinemaMvcClient.DTO_s;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using CinemaMvcClient.DTO_s.UserDTO;

namespace CinemaMvcClient.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://localhost:7051/api/user/";

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private void SetAuthorizationHeader(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<string> RegisterAsync(RegisterUserDTO dto)
        {
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_baseUrl + "register", content);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string?> LoginAsync(LoginUserDTO dto)
        {
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_baseUrl + "login", content);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<PagedUsersDTO> GetAllUsersAsync(string token, PaginationParams pagination)
        {
            SetAuthorizationHeader(token);
            var url = $"{_baseUrl}?page={pagination.Page}&itemsPerPage={pagination.ItemsPerPage}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PagedUsersDTO>(json);
        }

        public async Task<UserDTO?> GetByIdAsync(string token, int id)
        {
            SetAuthorizationHeader(token);
            var response = await _httpClient.GetAsync($"{_baseUrl}{id}");
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UserDTO>(json);
        }

        public async Task<UserDTO?> GetByUsernameAsync(string token, string username)
        {
            SetAuthorizationHeader(token);
            var response = await _httpClient.GetAsync($"{_baseUrl}username/{username}");
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UserDTO>(json);
        }

        public async Task<UserDTO?> GetByEmailAsync(string token, string email)
        {
            SetAuthorizationHeader(token);
            var response = await _httpClient.GetAsync($"{_baseUrl}email/{email}");
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UserDTO>(json);
        }

        public async Task<List<UserDTO>> SearchAsync(string token, string? username, string? email)
        {
            SetAuthorizationHeader(token);

            var queryParams = new List<string>();
            if (!string.IsNullOrEmpty(username))
                queryParams.Add($"username={username}");
            if (!string.IsNullOrEmpty(email))
                queryParams.Add($"email={email}");

            var queryString = string.Join("&", queryParams);
            var url = $"{_baseUrl}search";
            if (!string.IsNullOrEmpty(queryString))
                url += $"?{queryString}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<UserDTO>>(json);
        }

        public async Task<UserDTO?> UpdateAsync(string token, int id, UpdateUserDTO dto)
        {
            SetAuthorizationHeader(token);

            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_baseUrl}{id}", content);
            if (!response.IsSuccessStatusCode)
                return null;

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UserDTO>(jsonResponse);
        }

        public async Task<bool> DeleteAsync(string token, int id)
        {
            SetAuthorizationHeader(token);
            var response = await _httpClient.DeleteAsync($"{_baseUrl}{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
