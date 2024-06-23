using System.Diagnostics;

namespace Cinema.Application.Common.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly Stopwatch _timer;
    private readonly ILogger<TRequest> _logger;

    public PerformanceBehaviour(
        ILogger<TRequest> logger)
    {
        _timer = new Stopwatch();

        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _timer.Start();

        var response = await next();

        _timer.Stop();

        var requestName = typeof(TRequest).Name;
        
        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        _logger.LogInformation($"Request [{requestName}] executed in {elapsedMilliseconds} ms.");

        return response;
    }
}