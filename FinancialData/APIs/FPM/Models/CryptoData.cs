using Newtonsoft.Json;

namespace FinancialData.APIs.FPM.Models
{
    public class CryptoData
    {
        [JsonProperty("symbol")]
        public required string Symbol { get; set; }

        [JsonProperty("price")]
        public required decimal Price { get; set; }

        [JsonProperty("marketCap")]
        public required decimal MarketCap { get; set; }

        [JsonProperty("volume")]
        public required decimal Volume { get; set; }

        [JsonProperty("dayHigh")]
        public required decimal DayHigh { get; set; }

        [JsonProperty("dayLow")]
        public required decimal DayLow { get; set; }

        [JsonProperty("yearHigh")]
        public required decimal YearHigh { get; set; }

        [JsonProperty("yearLow")]
        public required decimal YearLow { get; set; }
    }
}