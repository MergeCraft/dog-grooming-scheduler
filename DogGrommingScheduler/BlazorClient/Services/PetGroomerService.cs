using Shared.DTOs;
using Shared.DTOs.PetGroomerDtos;
using System.Net.Http.Json;

namespace BlazorClient.Services
{
    public class PetGroomerService
    {
        private readonly HttpClient _http;

        public PetGroomerService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<PetGroomerDto>> GetAllGroomersAsync()
        {
            try
            {
                var groomers = await _http.GetFromJsonAsync<List<PetGroomerDto>>("api/PetGroomer");
                return groomers ?? new List<PetGroomerDto>();
            }
            catch (Exception)
            {
                return new List<PetGroomerDto>();
            }
        }
        public async Task<ScheduleDto?> GetScheduleAsync(Guid groomerId, DateTime date)
        {
            try
            {
                
                var dateString = date.ToString("yyyy-MM-dd");
                return await _http.GetFromJsonAsync<ScheduleDto>($"api/Reserve/schedule/{groomerId}/{dateString}");
            }
            catch (Exception)
            {
                return null; 
            }
        }
    }
}
