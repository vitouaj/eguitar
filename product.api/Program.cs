using MassTransit;
using Microsoft.VisualBasic;
using product.api;
using SharedMessages;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(m =>
{
    m.AddConsumer<OrderCreatedConsumer>();

    m.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
    {
        cfg.Host(new Uri("rabbitmq://localhost"), h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.ReceiveEndpoint("order-create", ep =>
        {
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(r => r.Interval(2, 100));
            ep.ConfigureConsumer<OrderCreatedConsumer>(provider);
        });
    }));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


List<Guitar> guitarList = [
    new Guitar
    {
        Id = Guid.NewGuid().ToString(),
        Brand = "yamaha",
        Model = "FG300 Serie",
        Price = 530
    },
    new Guitar
    {
        Id = Guid.NewGuid().ToString(),
        Brand = "fender",
        Model = "cd 60",
        Price = 660
    },
];

app.MapGet("/guitar-all", () =>
{
    return Results.Ok(guitarList.ToArray());
});

app.MapGet("/guitar", (string guitarId) =>
{
    var guitar = guitarList.Where(g => g.Id == guitarId).FirstOrDefault();
    if (guitar == null)
    {
        return Results.BadRequest("can't find");
    }
    return Results.Ok(guitar);
});

app.MapPost("/guitar", (string brand, string model, float price) =>
{
    var newGuitar = new Guitar
    {
        Id = Guid.NewGuid().ToString(),
        Brand = brand,
        Model = model,
        Price = price
    };
    guitarList.Add(newGuitar);
    return Results.Created();
});

app.MapDelete("/remove/{guitarId}", (string guitarId) =>
{
    var guitar = guitarList.Where(g => g.Id == guitarId).FirstOrDefault();
    if (guitar == null)
    {
        return Results.BadRequest();
    }
    guitarList.Remove(guitar);
    return Results.NoContent();
});

app.Run();