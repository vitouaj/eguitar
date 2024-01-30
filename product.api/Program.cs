using MassTransit;
using product.api;
using SharedMessages;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(busconfig =>
{
    busconfig.SetKebabCaseEndpointNameFormatter();

    busconfig.AddConsumer<OrderCreatedConsumer>();

    busconfig.UsingRabbitMq((context, cfg) => {
        cfg.Host(new Uri(builder.Configuration["MessageBroker:Host"]!), h =>{
            h.Username(builder.Configuration["MessageBroker:Username"]);
            h.Password(builder.Configuration["MessageBroker:Password"]);
        });
        cfg.ConfigureEndpoints(context);
    });
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