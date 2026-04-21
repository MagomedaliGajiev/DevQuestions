using System.Data;
using Shared.Database;

namespace Questions.Infrastructure.Postgres.Database;

public class TransactionManager : ITransactionManager
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public TransactionManager(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        var connection = _sqlConnectionFactory.Create();
        connection.Open();
        return connection.BeginTransaction();
    }
}