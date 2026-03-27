using Shared.DTOs;
using Shared.DTOs.ReserveDto;
using System.Net.Http.Json;

public class ReserveService
{
    private readonly HttpClient _http;
    public ReserveService(HttpClient http) => _http = http;

    public async Task<bool> CreateReserveAsync(CreateReserveDto dto)
    {
        var response = await _http.PostAsJsonAsync("api/Reserve", dto);
        return response.IsSuccessStatusCode;
    }
    public async Task<List<MyReserveDto>> GetMyReservesAsync(Guid userId)
{
    var response = await _http.GetAsync($"api/reserve/my-reserves/{userId}");
    if (response.IsSuccessStatusCode)
    {
        return await response.Content.ReadFromJsonAsync<List<MyReserveDto>>() ?? new();
    }
    return new List<MyReserveDto>();
}

    public async Task<bool> CancelReserveAsync(Guid reserveId)
    {
        var response = await _http.PutAsync($"api/reserve/cancel/{reserveId}", null);
        return response.IsSuccessStatusCode;
    }
}