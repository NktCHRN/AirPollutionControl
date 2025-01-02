namespace Application.Abstractions;

public interface ITransactionHandler
{
    public Task<TResult> ExecuteWithTransactionAsync<TResult>(Func<Task<TResult>> operation);
}
