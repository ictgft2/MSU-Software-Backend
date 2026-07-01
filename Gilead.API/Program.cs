using System.Text.Json.Serialization;
using Gilead.Application.Services;
using Gilead.Infrastructure;
using Gilead.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGileadServices();
builder.Services.AddGileadRepositories();
builder.Services.AddGileadCache(builder.Configuration);

DatabaseMigrationRunner.Migrate(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapGet("/health", () => Results.Ok(new { status = "Healthy" }));
app.MapControllers();
app.Run();
