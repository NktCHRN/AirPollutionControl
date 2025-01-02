using Application.Abstractions;
using MediatR;

namespace Application.Behaviors;
public sealed class TransactionalBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ITransactionHandler transactionHandler;

    public TransactionalBehavior(ITransactionHandler transactionHandler)
    {
        this.transactionHandler = transactionHandler;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        return await transactionHandler.ExecuteWithTransactionAsync(async () => await next());
    }
}
