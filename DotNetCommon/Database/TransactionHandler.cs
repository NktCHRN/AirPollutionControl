using Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace Database;
public sealed class TransactionHandler : ITransactionHandler
{
    private DbContext dbContext;
    private IDbContextTransaction? currentTransaction;
    private readonly ILogger<TransactionHandler> logger;

    public TransactionHandler(DbContext dbContext, ILogger<TransactionHandler> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }

    [MemberNotNullWhen(true, nameof(currentTransaction))]
    private bool HasActiveTransaction => currentTransaction != null;

    private async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        if (currentTransaction != null)
        {
            throw new InvalidOperationException("Db context already has active transaction;");
        }

        currentTransaction = await dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        return currentTransaction;
    }

    private async Task CommitTransactionAsync()
    {
        if (currentTransaction == null) throw new ArgumentNullException(nameof(currentTransaction));

        try
        {
            await dbContext.SaveChangesAsync();
            await currentTransaction.CommitAsync();
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (HasActiveTransaction)
            {
                currentTransaction.Dispose();
                currentTransaction = null;
            }
        }
    }

    public async Task<TResult> ExecuteWithTransactionAsync<TResult>(Func<Task<TResult>> operation)
    {
        try
        {
            TResult result = default!;

            if (HasActiveTransaction)
            {
                result = await operation();
                return result;
            }

            var strategy = dbContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                Guid transactionId;

                await using var transaction = await BeginTransactionAsync();
                using (logger.BeginScope(new List<KeyValuePair<string, object>> { new("TransactionContext", transaction.TransactionId) }))
                {
                    logger.LogInformation("Begin transaction {TransactionId}", transaction.TransactionId);
                    result = await operation();

                    logger.LogInformation("Commit transaction {TransactionId}", transaction.TransactionId);

                    await CommitTransactionAsync();

                    transactionId = transaction.TransactionId;
                }
            });

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error Handling transaction");
            throw;
        }
    }

    private void RollbackTransaction()
    {
        try
        {
            currentTransaction?.Rollback();
        }
        finally
        {
            if (HasActiveTransaction)
            {
                currentTransaction.Dispose();
                currentTransaction = null;
            }
        }
    }
}
