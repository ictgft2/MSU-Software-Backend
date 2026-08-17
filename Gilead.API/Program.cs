using System.Text.Json.Serialization;
using Gilead.API.Middleware;
using Gilead.Application.Services;
using Gilead.Infrastructure;
using Gilead.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);
const string AllowAnyCorsPolicy = "AllowAnyCorsPolicy";

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddCors(options =>
{
    options.AddPolicy(AllowAnyCorsPolicy, policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGileadServices();
builder.Services.AddGileadRepositories();
builder.Services.AddGileadCache(builder.Configuration);

DatabaseMigrationRunner.Migrate(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseCors(AllowAnyCorsPolicy);
app.MapGet("/health", async (PostgresConnectionFactory connectionFactory, CancellationToken cancellationToken) =>
{
    try
    {
        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT 1";
        await command.ExecuteScalarAsync(cancellationToken);
        return Results.Ok(new { status = "Healthy", database = "PostgreSQL" });
    }
    catch
    {
        return Results.Problem(statusCode: StatusCodes.Status503ServiceUnavailable, title: "Database unavailable");
    }
});
app.MapControllers();
app.Run();
