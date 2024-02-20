using Ride23.Identity.Infrastructure;
var builder = WebApplication.CreateBuilder(args);
builder.AddIdentityInfrastructure();
var app = builder.Build();
app.UseIdentityInfrastructure();
app.Run();
