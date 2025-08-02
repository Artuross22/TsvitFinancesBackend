namespace Brokers.IBKR.Client.Models;

public class OrderResponse
{
    public string Id { get; set; }
    public List<string> Message { get; set; }
    public bool IsSuppressed { get; set; }
    public string MessageId { get; set; }
}
