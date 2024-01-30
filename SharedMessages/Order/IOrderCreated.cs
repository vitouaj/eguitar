namespace SharedMessages;

public interface IOrderCreated
{
    string OrderId { get; set; }
    string UserId { get; set; }
    List<string> ProductIds { get; set; }
    string? Remark { get; set; }
    DateTime OrderDate { get; set; }
}
