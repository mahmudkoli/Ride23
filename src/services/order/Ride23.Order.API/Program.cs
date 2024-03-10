using Ride23.Order.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.AddOrderInfrastructure();
var app = builder.Build();
app.UseOrderInfrastructure();

app.MapGet("/", () => "Hello From Order Service").AllowAnonymous();
app.Run();
