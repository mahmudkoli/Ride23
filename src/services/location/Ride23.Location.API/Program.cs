using Ride23.Location.API;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<RouteHandlerOptions>(o => o.ThrowOnBadRequest = true);
builder.AddLocationInfrastructure();
var app = builder.Build();
app.UseLocationInfrastructure();
app.UseEndpoints();

app.MapGet("/", () => "Hello From Location Service").AllowAnonymous();
app.Run();