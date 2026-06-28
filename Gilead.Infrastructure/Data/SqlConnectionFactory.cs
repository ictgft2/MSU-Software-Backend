using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Gilead.Infrastructure.Data;

public sealed class SqlConnectionFactory(IConfiguration configuration)
{
    public SqlConnection CreateConnection()
    {
        var connectionString = configuration.GetConnectionString("GileadDb")
            ?? throw new InvalidOperationException("ConnectionStrings:GileadDb is not configured.");
        return new SqlConnection(connectionString);
    }

    public async Task<SqlConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken)
    {
        var connection = CreateConnection();
        await connection.OpenAsync(cancellationToken);
        return connection;
    }
}
