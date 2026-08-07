using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Gilead.Infrastructure.Data;

public sealed class SqlConnectionFactory(IConfiguration configuration)
{
    public SqlConnection CreateConnection()
    {
        string _connectionString = "";

        // 1. Check every possible key variation across providers
        string? renderUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
        string? configUrl = configuration["DATABASE_URL"];
        string? gileadDbEnv = Environment.GetEnvironmentVariable("ConnectionStrings__GileadDb");
        string? gileadDbConfig = configuration.GetConnectionString("GileadDb");

        // 2. Print absolute diagnostics to your Render Log screen
        Console.WriteLine($"[DIAGNOSTICS] Env(DATABASE_URL): {(string.IsNullOrEmpty(renderUrl) ? "MISSING" : "FOUND")}");
        Console.WriteLine($"[DIAGNOSTICS] Config(DATABASE_URL): {(string.IsNullOrEmpty(configUrl) ? "MISSING" : "FOUND")}");
        Console.WriteLine($"[DIAGNOSTICS] Env(ConnectionStrings__GileadDb): {(string.IsNullOrEmpty(gileadDbEnv) ? "MISSING" : "FOUND")}");
        Console.WriteLine($"[DIAGNOSTICS] Config(GileadDb): {(string.IsNullOrEmpty(gileadDbConfig) ? "MISSING" : "FOUND")}");

        // 3. Fallback hierarchy assignment
        string? rawConnectionString = renderUrl ?? configUrl ?? gileadDbEnv ?? gileadDbConfig;

        if (string.IsNullOrEmpty(rawConnectionString))
        {
            throw new InvalidOperationException("CRITICAL ERROR: No database connection string detected anywhere!");
        }

        // 4. Transform if it's a postgres:// URL
        if (rawConnectionString.StartsWith("postgres://"))
        {
            var uri = new Uri(rawConnectionString);
            var userInfo = uri.UserInfo.Split(':');
            var builder = new Npgsql.NpgsqlConnectionStringBuilder
            {
                Host = uri.Host,
                Port = uri.Port,
                Username = userInfo[0],
                Password = userInfo.Length > 1 ? userInfo[1] : "",
                Database = uri.LocalPath.TrimStart('/'),
                SslMode = Npgsql.SslMode.Require,
                TrustServerCertificate = true
            };
            rawConnectionString = builder.ToString();
        }

        _connectionString = rawConnectionString;


        return new SqlConnection(_connectionString);
    }

    public async Task<SqlConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken)
    {
        var connection = CreateConnection();
        await connection.OpenAsync(cancellationToken);
        return connection;
    }
}
