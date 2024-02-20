using Ride23.Customer.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.AddCustomerInfrastructure();
var app = builder.Build();
app.UseCustomerInfrastructure();

app.MapGet("/", () => "Hello From Customer Service").AllowAnonymous();
app.Run();