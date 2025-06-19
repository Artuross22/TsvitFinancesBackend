namespace Brokers.IBKR.Client.Models;

public class OrderResponse
{
    public string Id { get; set; }
    public List<object> Message { get; set; }
    public bool IsSuppressed { get; set; }
    public string MessageId { get; set; }
}
