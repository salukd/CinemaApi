namespace Cinema.Application.Common.Interfaces.Caching;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}