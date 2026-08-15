using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Gilead.Infrastructure.Data;

public sealed class SqlConnectionFactory(IConfiguration configuration)
{
    public SqlConnection CreateConnection()
    {
        string _connectionString = "";

        var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL")
                        ?? configuration.GetConnectionString("GileadDb");
        // Check if Render provided a postgres:// URL, and convert it
        if (connectionString != null && connectionString.StartsWith("postgres://"))
        {
            var databaseUri = new Uri(connectionString);
            var userInfo = databaseUri.UserInfo.Split(':');

            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo.Length > 1 ? userInfo[1] : "",
                Database = databaseUri.LocalPath.TrimStart('/'),
                SslMode = SslMode.Require,
                TrustServerCertificate = true // Required for Render's managed certificates
            };

            _connectionString = builder.ToString();
        }
        else
        {
            _connectionString = connectionString;
        }
        return new SqlConnection(_connectionString);
    }

    public async Task<SqlConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken)
    {
        var connection = CreateConnection();
        await connection.OpenAsync(cancellationToken);
        return connection;
    }
}
