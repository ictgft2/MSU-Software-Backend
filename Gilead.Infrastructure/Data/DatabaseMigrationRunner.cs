using System.Reflection;
using DbUp;
using DbUp.Engine;
using DbUp.Support;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Gilead.Infrastructure.Data;

public static class DatabaseMigrationRunner
{
    private const string ScriptRoot = ".Gilead.DB.";

    public static void Migrate(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("GileadDb")
            ?? throw new InvalidOperationException("ConnectionStrings:GileadDb is not configured.");

        EnsureDatabase.For.SqlDatabase(connectionString);

        using var lockConnection = new SqlConnection(connectionString);
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

            var repeatableResult = CreateRepeatableProcedureUpgrader(connectionString, assembly).PerformUpgrade();
            if (!repeatableResult.Successful)
            {
                throw new InvalidOperationException("Database procedure migration failed.", repeatableResult.Error);
            }
        }
        finally
        {
            ReleaseMigrationLock(lockConnection);
        }
    }

    private static UpgradeEngine CreateRunOnceUpgrader(string connectionString, Assembly assembly) =>
        DeployChanges.To
            .SqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(
                assembly,
                scriptName => scriptName.Contains(ScriptRoot, StringComparison.Ordinal)
                    && !IsStoredProcedureScript(scriptName))
            .WithScriptSorter(SortScripts)
            .LogToConsole()
            .Build();

    private static UpgradeEngine CreateRepeatableProcedureUpgrader(string connectionString, Assembly assembly) =>
        DeployChanges.To
            .SqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(
                assembly,
                IsStoredProcedureScript,
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

        if (scriptName.Contains("._002_TVPs.", StringComparison.Ordinal))
        {
            return 1;
        }

        if (scriptName.Contains("._003_StoredProcedures.", StringComparison.Ordinal))
        {
            return 2;
        }

        return 3;
    }

    private static bool IsStoredProcedureScript(string scriptName) =>
        scriptName.Contains(".Gilead.DB._003_StoredProcedures.", StringComparison.Ordinal);

    private static void AcquireMigrationLock(SqlConnection connection)
    {
        using var command = connection.CreateCommand();
        command.CommandText = """
            DECLARE @Result int;
            EXEC @Result = sp_getapplock
                @Resource = 'Gilead.DatabaseMigration',
                @LockMode = 'Exclusive',
                @LockOwner = 'Session',
                @LockTimeout = 60000;
            SELECT @Result;
            """;

        var result = (int)command.ExecuteScalar()!;
        if (result < 0)
        {
            throw new InvalidOperationException($"Could not acquire database migration lock. sp_getapplock returned {result}.");
        }
    }

    private static void ReleaseMigrationLock(SqlConnection connection)
    {
        using var command = connection.CreateCommand();
        command.CommandText = """
            EXEC sp_releaseapplock
                @Resource = 'Gilead.DatabaseMigration',
                @LockOwner = 'Session';
            """;
        command.ExecuteNonQuery();
    }

    private static bool ShouldBaselineExistingSchema(SqlConnection connection)
    {
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT CASE
                WHEN OBJECT_ID(N'dbo.Patients', N'U') IS NOT NULL
                    AND OBJECT_ID(N'dbo.SchemaVersions', N'U') IS NULL
                THEN CAST(1 AS bit)
                ELSE CAST(0 AS bit)
            END;
            """;

        return (bool)command.ExecuteScalar()!;
    }
}
