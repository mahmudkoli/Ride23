using Ride23.Location.API.Dtos;
using Ride23.Location.API.Services;

namespace Ride23.Location.API;

public static class Endpoints
{
    public static void UseEndpoints(this WebApplication app)
    {
        // get driver location history
        app.MapGet("/driver-location-history/{id}", async (string id, ILocationService driverLocationService) =>
        {
            return await driverLocationService.GetDriverLocationHistoryAsync(id);
        }).RequireAuthorization("location:read");

        // update driver location
        app.MapPost("/driver-location-update", async (AddLocationDto request, ILocationService driverLocationService) =>
        {
            return await driverLocationService.UpdateDriverLocationAsync(request);
        }).RequireAuthorization("location:write");

        // find nearby drivers
        app.MapPost("/find-nearby-drivers", async (AddLocationDto request, ILocationService driverLocationService) =>
        {
            return await driverLocationService.FindNearbyDriversAsync(request);
        }).RequireAuthorization("location:read");
    }
}
