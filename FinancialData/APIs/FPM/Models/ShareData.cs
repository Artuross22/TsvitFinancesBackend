using Newtonsoft.Json;

public class ShareData
{
    [JsonProperty("symbol")]
    public required string Symbol { get; set; }

    [JsonProperty("date")]
    public required string Date { get; set; }

    [JsonProperty("calendarYear")]
    public required string CalendarYear { get; set; }

    [JsonProperty("priceEarningsRatio")]
    public required decimal PERatio { get; set; }

    [JsonProperty("operatingCashFlowPerShare")]
    public required decimal OperatingCashFlowPerShare { get; set; }

    [JsonProperty("returnOnEquity")]
    public required decimal ROE { get; set; }

    [JsonProperty("priceToBookRatio")]
    public required decimal PBRatio { get; set; }

    [JsonProperty("dividendYield")]
    public required decimal DividendYield { get; set; }

    [JsonProperty("debtEquityRatio")]
    public required decimal DebtToEquityRatio { get; set; }

    [JsonProperty("ebitPerRevenue")]
    public required decimal EBIT { get; set; }

    [JsonProperty("priceSalesRatio")]
    public required decimal PSRatio { get; set; }

    [JsonProperty("freeCashFlowPerShare")]
    public required decimal FreeCashFlowPerShare { get; set; }

    [JsonProperty("returnOnAssets")]
    public required decimal ROA { get; set; }

    [JsonProperty("netProfitMargin")]
    public required decimal NetProfitMargin { get; set; }

    //[JsonProperty("revenueGrowth")]
    //public decimal RevenueGrowth { get; set; }

    [JsonProperty("debtRatio")]
    public required decimal DebtRatio { get; set; }

    [JsonProperty("freeCashFlow")]
    public decimal FreeCashFlow { get; set; }

    // EPS netIncome / totalSharesOutstanding.
    [JsonProperty("netIncome")]
    public decimal NetIncome { get; set; }
}