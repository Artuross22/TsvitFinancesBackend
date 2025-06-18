using System.Text.Json.Serialization;

namespace Brokers.IBKR.Client.Modelsl;

public class Contract
{
    [JsonPropertyName("conid")]
    public int Conid { get; set; }

    [JsonPropertyName("companyHeader")]
    public string? CompanyHeader { get; set; }

    [JsonPropertyName("companyName")]
    public string? CompanyName { get; set; }

    [JsonPropertyName("symbol")]
    public string? Symbol { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("restricted")]
    public string? Restricted { get; set; }

    [JsonPropertyName("fop")]
    public string? Fop { get; set; }

    [JsonPropertyName("opt")]
    public string? Opt { get; set; }
}