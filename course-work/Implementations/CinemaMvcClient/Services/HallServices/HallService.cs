using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using CinemaMvcClient.DTO_s;
using CinemaMvcClient.DTO_s.HallDTOs;
using CinemaMvcClient.Services.HallServices;

public class HallService : IHallService
{
    private readonly HttpClient _httpClient;

    public HallService(HttpClient httpClient)
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

    public async Task<PagedHallsDTO> GetAllHalls(string token, PaginationParams pagination)
    {
        SetAuthorizationHeader(token);

        string url = $"api/halls?page={pagination.Page}&itemsPerPage={pagination.ItemsPerPage}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<PagedHallsDTO>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
    }

    public async Task<HallDTO> GetHallById(string token, int id)
    {
        SetAuthorizationHeader(token);

        var response = await _httpClient.GetAsync($"api/halls/{id}");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<HallDTO>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
    }

    public async Task<PagedHallsDTO> GetHallsByLocation(string token, string location, PaginationParams pagination)
    {
        SetAuthorizationHeader(token);

        string url = $"api/halls/location/{Uri.EscapeDataString(location)}?page={pagination.Page}&itemsPerPage={pagination.ItemsPerPage}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<PagedHallsDTO>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
    }

    public async Task<PagedHallsDTO> SearchHalls(string token, string? name, int? minCapacity, PaginationParams pagination)
    {
        SetAuthorizationHeader(token);

        var queryParams = new List<string>();
        if (!string.IsNullOrEmpty(name))
            queryParams.Add($"name={Uri.EscapeDataString(name)}");
        if (minCapacity.HasValue)
            queryParams.Add($"minCapacity={minCapacity.Value}");
        queryParams.Add($"page={pagination.Page}");
        queryParams.Add($"itemsPerPage={pagination.ItemsPerPage}");

        string url = "api/halls/search";
        if (queryParams.Count > 0)
            url += "?" + string.Join("&", queryParams);

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<PagedHallsDTO>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
    }

    public async Task<HallDTO> CreateHall(string token, CreateHallDTO dto)
    {
        SetAuthorizationHeader(token);

        var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("api/halls", content);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<HallDTO>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
    }

    public async Task<HallDTO> UpdateHall(string token, int id, UpdateHallDTO dto)
    {
        SetAuthorizationHeader(token);

        var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"api/halls/{id}", content);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<HallDTO>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
    }

    public async Task<bool> DeleteHall(string token, int id)
    {
        SetAuthorizationHeader(token);

        var response = await _httpClient.DeleteAsync($"api/halls/{id}");
        return response.IsSuccessStatusCode;
    }
}
