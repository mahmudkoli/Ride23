namespace Ride23.Framework.Core.gRPC;

public interface IGrpcClient<TClient>
{
    Task<TResponse> CallAsync<TRequest, TResponse>(Func<TClient, Task<TResponse>> call, TRequest request);
    TResponse Call<TRequest, TResponse>(Func<TClient, TResponse> call, TRequest request);
}
