using MassTransit;
using SharedMessages;
using user.api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(busconfig =>
{
    busconfig.SetKebabCaseEndpointNameFormatter();
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

List<User> users = [];

app.MapPost("/register", async (UserDto userDto, IPublishEndpoint publishEndpoint) => {

    var u = new User
    {
        Id = Guid.NewGuid().ToString(),
        Email = userDto.Email,
        Password = userDto.Password,
        Phone = userDto.Phone,
        CreatedAt = DateTime.Now
    };

    users.Add(u);

    // email to this new user
    await publishEndpoint.Publish(new UserRegisterEvent {
        UserId = u.Id,
        Email = u.Email,
        CreatedAt = u.CreatedAt
    });

    return Results.Ok();
});


app.MapPost("/login", (string email, string password)=> {
    var u = users.Where(u => u.Email == email & u.Password == password).Any();
    if (!u)
        return Results.BadRequest("login failed");
    return Results.Ok("success");
});
 
app.Run();
