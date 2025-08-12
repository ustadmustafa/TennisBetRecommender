using System.Text.Json;
using TennisBets.Models;

namespace TennisBets.Services
{
    public class TennisApiService : ITennisApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _apiKey;
        private readonly string _baseUrl = "https://api.api-tennis.com/tennis/";

        public TennisApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _apiKey = _configuration["TennisApi:ApiKey"] ?? "your_api_key_here";
        }

        public async Task<H2HResponse> GetH2HAsync(long player1Key, long player2Key)
        {
            try
            {
                var url = $"{_baseUrl}?method=get_H2H&APIkey={_apiKey}&first_player_key={player1Key}&second_player_key={player2Key}";
                Console.WriteLine($"Calling H2H API: {url}");
                
                var response = await _httpClient.GetStringAsync(url);
                Console.WriteLine($"H2H API Response received: {response.Length} characters");
                
                var result = JsonSerializer.Deserialize<H2HResponse>(response) ?? new H2HResponse();
                Console.WriteLine($"H2H API deserialized: {result.Result?.H2H?.Count ?? 0} matches found");
                
                return result;
            }
            catch (Exception ex)
            {
                // Log error
                Console.WriteLine($"Error fetching H2H data: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                return new H2HResponse();
            }
        }

        public async Task<StandingsResponse> GetStandingsAsync(string eventType = "WTA")
        {
            try
            {
                var url = $"{_baseUrl}?method=get_standings&event_type={eventType}&APIkey={_apiKey}";
                var response = await _httpClient.GetStringAsync(url);
                return JsonSerializer.Deserialize<StandingsResponse>(response) ?? new StandingsResponse();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching standings data: {ex.Message}");
                return new StandingsResponse();
            }
        }

        public async Task<PlayerResponse> GetPlayerStatsAsync(long playerKey)
        {
            try
            {
                var url = $"{_baseUrl}?method=get_players&player_key={playerKey}&APIkey={_apiKey}";
                var response = await _httpClient.GetStringAsync(url);
                return JsonSerializer.Deserialize<PlayerResponse>(response) ?? new PlayerResponse();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching player stats: {ex.Message}");
                return new PlayerResponse();
            }
        }

        public async Task<PlayerStats> GetPlayerDetailedStatsAsync(long playerKey)
        {
            try
            {
                var url = $"{_baseUrl}?method=get_players&player_key={playerKey}&APIkey={_apiKey}";
                Console.WriteLine($"Calling Player Stats API: {url}");
                
                var response = await _httpClient.GetStringAsync(url);
                Console.WriteLine($"Player Stats API Response received: {response.Length} characters");
                Console.WriteLine($"Raw API Response: {response}");
                
                // Önce PlayerStatsResponse olarak deserialize et
                var responseWrapper = JsonSerializer.Deserialize<PlayerStatsResponse>(response);
                Console.WriteLine($"Response wrapper deserialized: Success={responseWrapper?.Success}, Result count={responseWrapper?.Result?.Count ?? 0}");
                
                // İlk oyuncuyu al (genellikle tek oyuncu döner)
                var result = responseWrapper?.Result?.FirstOrDefault() ?? new PlayerStats();
                Console.WriteLine($"Player Stats extracted: PlayerKey={result.PlayerKey}, PlayerName={result.PlayerName}, Stats count={result.Stats?.Count ?? 0}");
                
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching player detailed stats: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                return new PlayerStats();
            }
        }
    }
}
