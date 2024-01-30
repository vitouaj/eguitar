namespace order.api;

public class OrderDTO
{
    public string UserId { get; set; }
    public List<string> ProductIds { get; set; } = [];
    public string? Remark { get; set; }
}
