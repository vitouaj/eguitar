using MassTransit;
using SharedMessages;

namespace email.api;

public class UserCreatedEventConsumer : IConsumer<UserRegisterEvent>
{
    private readonly ILogger<UserRegisterEvent> _logger;
    public UserCreatedEventConsumer(ILogger<UserRegisterEvent> logger){
        _logger = logger;
    }
    public async Task Consume(ConsumeContext<UserRegisterEvent> context)
    {
        _logger.Log(LogLevel.Information, $"Confirm Email for user {context.Message.Email}");
        Task.Delay(200);
    }
}
