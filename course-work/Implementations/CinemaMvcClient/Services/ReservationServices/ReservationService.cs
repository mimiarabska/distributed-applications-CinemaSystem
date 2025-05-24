using CinemaMvcClient.DTO_s.Reservation;
using CinemaMvcClient.Services.ReservationServices;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace CinemaMVC.Services
{
    public class ReservationService : IReservationService
    {
        private readonly HttpClient _httpClient;
        private readonly string baseUrl = "https://localhost:7051/api/reservation/";

        public ReservationService(HttpClient _httpClient)
        {
            this._httpClient = _httpClient;
        }

        private void SetAuthorizationHeader(string token)
        {
            if (_httpClient.DefaultRequestHeaders.Authorization?.Parameter != token)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<PagedReservationsDTO?> GetAll(string token, int page = 1, int itemsPerPage = 10)
        {
            SetAuthorizationHeader(token);
            var response = await _httpClient.GetAsync($"{baseUrl}?page={page}&itemsPerPage={itemsPerPage}");
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PagedReservationsDTO>(json);
        }

        public async Task<ReservationDTO?> GetById(string token, int id)
        {
            SetAuthorizationHeader(token);
            var response = await _httpClient.GetAsync($"{baseUrl}{id}");
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ReservationDTO>(json);
        }

        public async Task<List<ReservationDTO>?> GetByUserId(string token, int userId)
        {
            SetAuthorizationHeader(token);
            var response = await _httpClient.GetAsync($"{baseUrl}user/{userId}");
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ReservationDTO>>(json);
        }

        public async Task<ReservationDTO?> Create(string token, CreateReservationDTO model)
        {
            SetAuthorizationHeader(token);
            var jsonContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(baseUrl, jsonContent);

            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ReservationDTO>(json);
        }

        public async Task<ReservationDTO?> Update(string token, UpdateReservationDTO model, int id)
        {
            SetAuthorizationHeader(token);
            var jsonContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{baseUrl}{id}", jsonContent);

            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ReservationDTO>(json);
        }

        public async Task<bool> Delete(string token, int id)
        {
            SetAuthorizationHeader(token);
            var response = await _httpClient.DeleteAsync($"{baseUrl}{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<PagedReservationsDTO?> Search(string token, int? minSeats, bool? isConfirmed, int page = 1, int itemsPerPage = 10)
        {
            SetAuthorizationHeader(token);
            var query = $"{baseUrl}search?page={page}&itemsPerPage={itemsPerPage}";

            if (minSeats.HasValue)
                query += $"&minSeats={minSeats.Value}";

            if (isConfirmed.HasValue)
                query += $"&isConfirmed={isConfirmed.Value.ToString().ToLower()}";

            var response = await _httpClient.GetAsync(query);
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PagedReservationsDTO>(json);
        }
    }
}
