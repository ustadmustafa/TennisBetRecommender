using System.Text.Json;
using TennisBets.Models;

namespace TennisBets.Services
{
    public class TennisService : ITennisService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _apiKey;
        private readonly string _baseUrl = "https://api.api-tennis.com/tennis/";

        public TennisService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _apiKey = "a8a78ad99e8bc0623fb22b8ae906133c12361eae083008375af2a362aaa3cb31";
        }

        public async Task<List<TennisMatch>> GetLiveMatchesAsync()
        {
            try
            {
                var url = $"{_baseUrl}?method=get_livescore&APIkey={_apiKey}";
                Console.WriteLine($"Calling API: {url}");
                
                var response = await _httpClient.GetStringAsync(url);
                Console.WriteLine($"Raw API Response Length: {response?.Length ?? 0}");
                Console.WriteLine($"Raw API Response (first 1000 chars): {response?.Substring(0, Math.Min(1000, response?.Length ?? 0))}");
                
                if (string.IsNullOrEmpty(response))
                {
                    Console.WriteLine("API response is empty");
                    return new List<TennisMatch>();
                }

                // Try to parse as dynamic first to see the structure
                try
                {
                    var jsonDoc = JsonDocument.Parse(response);
                    var root = jsonDoc.RootElement;
                    Console.WriteLine($"Root element type: {root.ValueKind}");
                    
                    if (root.TryGetProperty("success", out var successProp))
                    {
                        Console.WriteLine($"Success property found: {successProp.GetInt32()}");
                    }
                    
                    if (root.TryGetProperty("result", out var resultProp))
                    {
                        Console.WriteLine($"Result property found, type: {resultProp.ValueKind}, count: {resultProp.GetArrayLength()}");
                        
                        // Try to parse first item to see structure
                        if (resultProp.GetArrayLength() > 0)
                        {
                            var firstItem = resultProp[0];
                            Console.WriteLine($"First item keys: {string.Join(", ", firstItem.EnumerateObject().Select(p => p.Name))}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error parsing JSON structure: {ex.Message}");
                }

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var apiResponse = JsonSerializer.Deserialize<ApiResponse>(response, options);
                Console.WriteLine($"Deserialized Success: {apiResponse?.Success}");
                Console.WriteLine($"Deserialized Result Count: {apiResponse?.Result?.Count ?? 0}");
                
                if (apiResponse?.Result != null)
                {
                    var liveMatches = apiResponse.Result.Where(m => m.IsLive).ToList();
                    Console.WriteLine($"Live Matches Count: {liveMatches.Count}");
                    return liveMatches;
                }
                
                return new List<TennisMatch>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching live matches: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return new List<TennisMatch>();
            }
        }

        public async Task<List<TennisMatch>> GetUpcomingMatchesAsync()
        {
            try
            {
                var url = $"{_baseUrl}?method=get_livescore&APIkey={_apiKey}";
                Console.WriteLine($"Calling API for upcoming matches: {url}");
                
                var response = await _httpClient.GetStringAsync(url);
                Console.WriteLine($"Upcoming matches API Response Length: {response?.Length ?? 0}");
                
                if (string.IsNullOrEmpty(response))
                {
                    return new List<TennisMatch>();
                }

                // Debug JSON structure for upcoming matches
                try
                {
                    var jsonDoc = JsonDocument.Parse(response);
                    var root = jsonDoc.RootElement;
                    Console.WriteLine($"Upcoming - Root element type: {root.ValueKind}");
                    
                    if (root.TryGetProperty("success", out var successProp))
                    {
                        Console.WriteLine($"Upcoming - Success property found: {successProp.GetInt32()}");
                    }
                    
                    if (root.TryGetProperty("result", out var resultProp))
                    {
                        Console.WriteLine($"Upcoming - Result property found, type: {resultProp.ValueKind}, count: {resultProp.GetArrayLength()}");
                        
                        // Check first few items to see their live status
                        if (resultProp.GetArrayLength() > 0)
                        {
                            for (int i = 0; i < Math.Min(3, resultProp.GetArrayLength()); i++)
                            {
                                var item = resultProp[i];
                                if (item.TryGetProperty("event_live", out var liveProp))
                                {
                                    Console.WriteLine($"Item {i} - event_live: {liveProp.GetString()}");
                                }
                                if (item.TryGetProperty("event_first_player", out var playerProp))
                                {
                                    Console.WriteLine($"Item {i} - first player: {playerProp.GetString()}");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error parsing JSON structure for upcoming: {ex.Message}");
                }

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var apiResponse = JsonSerializer.Deserialize<ApiResponse>(response, options);
                Console.WriteLine($"Upcoming matches - Deserialized Success: {apiResponse?.Success}");
                Console.WriteLine($"Upcoming matches - Deserialized Result Count: {apiResponse?.Result?.Count ?? 0}");
                
                if (apiResponse?.Result != null)
                {
                    // Debug each match's live status
                    foreach (var match in apiResponse.Result.Take(3))
                    {
                        Console.WriteLine($"Match: {match.EventFirstPlayer} vs {match.EventSecondPlayer} - EventLive: '{match.EventLive}', IsLive: {match.IsLive}");
                    }
                    
                    var upcomingMatches = apiResponse.Result.Where(m => !m.IsLive).ToList();
                    Console.WriteLine($"Upcoming Matches Count: {upcomingMatches.Count}");
                    return upcomingMatches;
                }
                
                return new List<TennisMatch>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching upcoming matches: {ex.Message}");
                return new List<TennisMatch>();
            }
        }

        public async Task<TennisMatch?> GetMatchDetailsAsync(long eventKey)
        {
            try
            {
                var url = $"{_baseUrl}?method=get_livescore&APIkey={_apiKey}";
                var response = await _httpClient.GetStringAsync(url);
                
                if (string.IsNullOrEmpty(response))
                {
                    return null;
                }

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var apiResponse = JsonSerializer.Deserialize<ApiResponse>(response, options);
                return apiResponse?.Result?.FirstOrDefault(m => m.EventKey == eventKey);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching match details: {ex.Message}");
                return null;
            }
        }
    }
}
