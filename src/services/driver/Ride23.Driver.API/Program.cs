using Ride23.Driver.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.AddDriverInfrastructure();
var app = builder.Build();
app.UseDriverInfrastructure();

app.MapGet("/", () => "Hello From Driver Service").AllowAnonymous();
app.Run();