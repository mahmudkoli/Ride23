using Ride23.Order.Infrastructure;
try
{

	var builder = WebApplication.CreateBuilder(args);
	builder.AddOrderInfrastructure();
	var app = builder.Build();
	app.UseOrderInfrastructure();

	app.MapGet("/", () => "Hello From Order Service").AllowAnonymous();
	app.Run();
}
catch (Exception ex)
{
	var eee = ex;
	throw;
}