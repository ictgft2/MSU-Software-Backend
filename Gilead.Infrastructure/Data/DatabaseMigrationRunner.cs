using System.Reflection;
using DbUp;
using DbUp.Engine;
using DbUp.Support;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Gilead.Infrastructure.Data;

public static class DatabaseMigrationRunner
{
    private const string ScriptRoot = ".Gilead.DB.";

    public static void Migrate(IConfiguration configuration)
    {
        var connectionString = PostgresConnectionString.Resolve(configuration);
        EnsureDatabase.For.PostgresqlDatabase(connectionString);

        using var lockConnection = new NpgsqlConnection(connectionString);
        lockConnection.Open();
        AcquireMigrationLock(lockConnection);

        try
        {
            var assembly = typeof(DatabaseMigrationRunner).Assembly;
            var runOnceUpgrader = CreateRunOnceUpgrader(connectionString, assembly);
            var result = ShouldBaselineExistingSchema(lockConnection)
                ? runOnceUpgrader.MarkAsExecuted()
                : runOnceUpgrader.PerformUpgrade();

            if (!result.Successful)
            {
                throw new InvalidOperationException("Database migration failed.", result.Error);
            }

            var repeatableResult = CreateRepeatableFunctionUpgrader(connectionString, assembly).PerformUpgrade();
            if (!repeatableResult.Successful)
            {
                throw new InvalidOperationException("Database function migration failed.", repeatableResult.Error);
            }
        }
        finally
        {
            ReleaseMigrationLock(lockConnection);
        }
    }

    private static UpgradeEngine CreateRunOnceUpgrader(string connectionString, Assembly assembly) =>
        DeployChanges.To
            .PostgresqlDatabase(connectionString)
            .WithVariablesDisabled()
            .WithScriptsEmbeddedInAssembly(
                assembly,
                scriptName => scriptName.Contains(ScriptRoot, StringComparison.Ordinal)
                    && !IsFunctionScript(scriptName))
            .WithScriptSorter(SortScripts)
            .LogToConsole()
            .Build();

    private static UpgradeEngine CreateRepeatableFunctionUpgrader(string connectionString, Assembly assembly) =>
        DeployChanges.To
            .PostgresqlDatabase(connectionString)
            .WithVariablesDisabled()
            .WithScriptsEmbeddedInAssembly(
                assembly,
                IsFunctionScript,
                new SqlScriptOptions
                {
                    ScriptType = ScriptType.RunAlways,
                    RunGroupOrder = 100
                })
            .WithScriptSorter(SortScripts)
            .LogToConsole()
            .Build();

    private static IEnumerable<SqlScript> SortScripts(IEnumerable<SqlScript> scripts) =>
        scripts
            .OrderBy(script => GetScriptRank(script.Name))
            .ThenBy(script => script.Name, StringComparer.OrdinalIgnoreCase);

    private static int GetScriptRank(string scriptName)
    {
        if (scriptName.Contains("._001_Tables.", StringComparison.Ordinal))
        {
            return 0;
        }

        if (scriptName.Contains("._003_StoredProcedures.", StringComparison.Ordinal))
        {
            return 1;
        }

        return 2;
    }

    private static bool IsFunctionScript(string scriptName) =>
        scriptName.Contains(".Gilead.DB._003_StoredProcedures.", StringComparison.Ordinal);

    private static void AcquireMigrationLock(NpgsqlConnection connection)
    {
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT pg_advisory_lock(hashtextextended('Gilead.DatabaseMigration', 0));";
        command.ExecuteNonQuery();
    }

    private static void ReleaseMigrationLock(NpgsqlConnection connection)
    {
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT pg_advisory_unlock(hashtextextended('Gilead.DatabaseMigration', 0));";
        command.ExecuteNonQuery();
    }

    private static bool ShouldBaselineExistingSchema(NpgsqlConnection connection)
    {
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT to_regclass('public.patients') IS NOT NULL
                AND to_regclass('public.schemaversions') IS NULL;
            """;

        return (bool)command.ExecuteScalar()!;
    }
}
