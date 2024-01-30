using MassTransit;
using SharedMessages;

namespace product.api;

public class OrderCreatedConsumer : IConsumer<IOrderCreated>
{
    private readonly ILogger<IOrderCreated> _logger;
    public OrderCreatedConsumer(ILogger<IOrderCreated> logger)
    {
        _logger = logger;
    }
    public async Task Consume(ConsumeContext<IOrderCreated> context)
    {
        // This message could be store in a database
        // Or perform any task that is required in the project
        var message = "Order " + context.Message.OrderId + "at " + context.Message.OrderDate;
        _logger.LogInformation(message);
        await Task.CompletedTask;
    }
}
