using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Cinema.gRPC.Api;

public class ExceptionHandlingInterceptor : Interceptor
{
    private readonly ILogger<ExceptionHandlingInterceptor> _logger;

    public ExceptionHandlingInterceptor(ILogger<ExceptionHandlingInterceptor> logger)
    {
        _logger = logger;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (RpcException)
        {
            throw; 
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred during the gRPC call.");
            throw new RpcException(new Status(StatusCode.Internal, ex.Message));
        }
    }
}