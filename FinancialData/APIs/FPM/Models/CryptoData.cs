using System.Text.Json.Serialization;

namespace FinancialData.APIs.FPM.Models
{
    public class CryptoData
    {
        [JsonPropertyName("symbol")]
        public required string Symbol { get; set; }

        [JsonPropertyName("price")]
        public required decimal Price { get; set; }

        [JsonPropertyName("marketCap")]
        public required decimal MarketCap { get; set; }

        [JsonPropertyName("volume")]
        public required decimal Volume { get; set; }

        [JsonPropertyName("dayHigh")]
        public required decimal DayHigh { get; set; }

        [JsonPropertyName("dayLow")]
        public required decimal DayLow { get; set; }

        [JsonPropertyName("yearHigh")]
        public required decimal YearHigh { get; set; }

        [JsonPropertyName("yearLow")]
        public required decimal YearLow { get; set; }
    }
}