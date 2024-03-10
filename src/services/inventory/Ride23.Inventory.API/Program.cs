using Ride23.Inventory.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.AddInventoryInfrastructure();
var app = builder.Build();
app.UseInventoryInfrastructure();

app.MapGet("/", () => "Hello From Inventory Service").AllowAnonymous();
app.Run();
