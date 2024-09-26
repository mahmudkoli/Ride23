using Grpc.Core;
using Microsoft.Extensions.Logging;
using Ride23.Framework.Core.Exceptions;
using Ride23.Framework.Core.gRPC;
using System.Net;

namespace Ride23.Framework.Infrastructure.gRPC;

public class GrpcClient<TClient> : IGrpcClient<TClient>
{
    private readonly TClient _client;
    private readonly ILogger<GrpcClient<TClient>> _logger;

    public GrpcClient(TClient client, ILogger<GrpcClient<TClient>> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<TResponse> CallAsync<TRequest, TResponse>(Func<TClient, Task<TResponse>> call, TRequest request)
    {
        try
        {
            return await call(_client);
        }
        catch (RpcException ex)
        {
            _logger.LogError(ex, "gRPC call failed.");
            throw new GrpcException(ex.Status.Detail, HttpStatusCode.InternalServerError);
        }
    }

    public TResponse Call<TRequest, TResponse>(Func<TClient, TResponse> call, TRequest request)
    {
        try
        {
            return call(_client);
        }
        catch (RpcException ex)
        {
            _logger.LogError(ex, "gRPC call failed.");
            throw new GrpcException(ex.Status.Detail, HttpStatusCode.InternalServerError);
        }
    }
}