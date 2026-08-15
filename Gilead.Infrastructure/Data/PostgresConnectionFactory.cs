using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Gilead.Infrastructure.Data;

public sealed class PostgresConnectionFactory(IConfiguration configuration)
{
    public NpgsqlConnection CreateConnection() =>
        new(PostgresConnectionString.Resolve(configuration));

    public async Task<NpgsqlConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken)
    {
        var connection = CreateConnection();
        await connection.OpenAsync(cancellationToken);
        return connection;
    }
}

internal static class PostgresConnectionString
{
    public static string Resolve(IConfiguration configuration)
    {
        var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL")
            ?? configuration.GetConnectionString("GileadDb")
            ?? throw new InvalidOperationException(
                "A PostgreSQL connection string must be configured through DATABASE_URL or ConnectionStrings:GileadDb.");

        if (!connectionString.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase)
            && !connectionString.StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase))
        {
            return connectionString;
        }

        var databaseUri = new Uri(connectionString);
        var userInfo = databaseUri.UserInfo.Split(':', 2);
        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = databaseUri.Host,
            Port = databaseUri.IsDefaultPort ? 5432 : databaseUri.Port,
            Username = Uri.UnescapeDataString(userInfo[0]),
            Password = userInfo.Length == 2 ? Uri.UnescapeDataString(userInfo[1]) : string.Empty,
            Database = Uri.UnescapeDataString(databaseUri.AbsolutePath.TrimStart('/')),
            SslMode = SslMode.Require,
            Timezone = "UTC"
        };

        return builder.ConnectionString;
    }
}
