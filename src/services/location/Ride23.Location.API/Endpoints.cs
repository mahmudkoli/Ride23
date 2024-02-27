using Microsoft.AspNetCore.Mvc;
using Ride23.Location.API.Dtos;
using Ride23.Location.API.Services;

namespace Ride23.Location.API;

public static class Endpoints
{
    public static void UseEndpoints(this WebApplication app)
    {
        // get locations
        app.MapGet("/locations/{id:guid}", async (Guid id, ILocationService locationService) =>
        {
            return await locationService.GetLocationsAsync(id);
        }).RequireAuthorization("location:read");

        // update locations
        app.MapPost("/locations", async (AddLocationDto request, ILocationService locationService) =>
        {
            return await locationService.UpdateLocationAsync(Guid.Empty, request);
        }).RequireAuthorization("location:write");
    }
}
