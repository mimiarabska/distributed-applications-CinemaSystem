using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CinemaAPI.DTO_s;
using CinemaAPI.DTO_s.ProjectionDTOs;
using CinemaMvcClient.DTO_s.ProjectionDTOs;
using CinemaMvcClient.DTO_s;
using CinemaMvcClient.DTO_s.ProjectionDTOss;
using CinemaMvcClient.Services.ProjectionServices;

public class ProjectionService : IProjectionService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "https://localhost:7051/api/projection/";

    public ProjectionService(HttpClient httpClient)
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

    public async Task<PagedProjectionsDTO> GetAllProjections(string token, PaginationParams pagination)
    {
        SetAuthorizationHeader(token);

        var url = $"{_baseUrl}?page={pagination.Page}&itemsPerPage={pagination.ItemsPerPage}";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<PagedProjectionsDTO>();
    }

    public async Task<ProjectionDTO?> GetProjectionById(string token, int id)
    {
        SetAuthorizationHeader(token);

        var response = await _httpClient.GetAsync($"{_baseUrl}{id}");
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<ProjectionDTO>();
    }

    public async Task<ProjectionDTO> CreateProjection(string token, CreateProjectionDTO dto)
    {
        SetAuthorizationHeader(token);

        var response = await _httpClient.PostAsJsonAsync(_baseUrl, dto);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<ProjectionDTO>();
    }

    public async Task<ProjectionDTO> UpdateProjection(string token, int id, UpdateProjectionDTO dto)
    {
        SetAuthorizationHeader(token);

        var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}{id}", dto);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<ProjectionDTO>();
    }

    public async Task<bool> DeleteProjection(string token, int id)
    {
        SetAuthorizationHeader(token);

        var response = await _httpClient.DeleteAsync($"{_baseUrl}{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<PagedProjectionsDTO> SearchProjections(string token, string? movieTitle, DateTime? date, PaginationParams pagination)
    {
        SetAuthorizationHeader(token);

        var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
        if (!string.IsNullOrWhiteSpace(movieTitle))
            query["movieTitle"] = movieTitle;
        if (date.HasValue)
            query["date"] = date.Value.ToString("yyyy-MM-dd");
        query["page"] = pagination.Page.ToString();
        query["itemsPerPage"] = pagination.ItemsPerPage.ToString();

        var url = $"{_baseUrl}search?{query.ToString()}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<PagedProjectionsDTO>();
    }
}
