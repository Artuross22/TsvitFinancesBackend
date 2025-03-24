using System.Text.Json.Serialization;

public class ShareData
{
    [JsonPropertyName("symbol")]
    public required string Symbol { get; set; }

    [JsonPropertyName("date")]
    public required string Date { get; set; }

    [JsonPropertyName("calendarYear")]
    public required string CalendarYear { get; set; }

    [JsonPropertyName("priceEarningsRatio")]
    public required decimal PERatio { get; set; }

    [JsonPropertyName("operatingCashFlowPerShare")]
    public required decimal OperatingCashFlowPerShare { get; set; }

    [JsonPropertyName("returnOnEquity")]
    public required decimal ROE { get; set; }

    [JsonPropertyName("priceToBookRatio")]
    public required decimal PBRatio { get; set; }

    [JsonPropertyName("dividendYield")]
    public required decimal DividendYield { get; set; }

    [JsonPropertyName("debtEquityRatio")]
    public required decimal DebtToEquityRatio { get; set; }

    [JsonPropertyName("ebitPerRevenue")]
    public required decimal EBIT { get; set; }

    [JsonPropertyName("priceSalesRatio")]
    public required decimal PSRatio { get; set; }

    [JsonPropertyName("freeCashFlowPerShare")]
    public required decimal FreeCashFlowPerShare { get; set; }

    [JsonPropertyName("returnOnAssets")]
    public required decimal ROA { get; set; }

    [JsonPropertyName("netProfitMargin")]
    public required decimal NetProfitMargin { get; set; }

    //[JsonPropertyName("revenueGrowth")]
    //public decimal RevenueGrowth { get; set; }

    [JsonPropertyName("debtRatio")]
    public required decimal DebtRatio { get; set; }

    [JsonPropertyName("freeCashFlow")]
    public decimal FreeCashFlow { get; set; }

    // EPS netIncome / totalSharesOutstanding.
    [JsonPropertyName("netIncome")]
    public decimal NetIncome { get; set; }
}