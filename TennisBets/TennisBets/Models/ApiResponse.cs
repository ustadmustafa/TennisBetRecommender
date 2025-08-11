using System.Text.Json.Serialization;

namespace TennisBets.Models
{
    public class ApiResponse
    {
        [JsonPropertyName("success")]
        public int Success { get; set; }

        [JsonPropertyName("result")]
        public List<TennisMatch> Result { get; set; } = new();
    }
}
