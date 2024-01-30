using MassTransit;
using Microsoft.AspNetCore.Http.HttpResults;
using order.api;
using SharedMessages;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddMassTransit(ms =>
{
    ms.UsingRabbitMq();
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/order", () =>
{
    return "this is order api";
});


app.MapPost("/order", async (
    OrderDTO orderDto,
    IPublishEndpoint _publishEnpoint,
    ILogger<OrderDTO> _logger
) =>
{
    var orderId = Guid.NewGuid().ToString();
    _logger.Log(LogLevel.Information, $"Order record created in Order table in database at {DateTime.Now}");
    await _publishEnpoint.Publish<IOrderCreated>(new
    {
        OrderId = orderId,
        UserId = orderDto.UserId,
        ProductIds = orderDto.ProductIds,
        Remark = orderDto.Remark,
        OrderDate = DateTime.Now,
    });

    _logger.Log(LogLevel.Information, $"Message has been sent to consumer!");
    return Results.Ok();
});

app.Run();