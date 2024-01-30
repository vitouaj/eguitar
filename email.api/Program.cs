using email.api;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(busconfig =>
{
    busconfig.SetKebabCaseEndpointNameFormatter();

    busconfig.AddConsumer<UserCreatedEventConsumer>();

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



app.Run();
