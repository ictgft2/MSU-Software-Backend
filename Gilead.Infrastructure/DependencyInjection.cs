using Gilead.Application.Interfaces;
using Gilead.Infrastructure.Cache;
using Gilead.Infrastructure.Data;
using Gilead.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Gilead.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddGileadRepositories(this IServiceCollection services)
    {
        services.AddSingleton<SqlConnectionFactory>();
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IEncounterRepository, EncounterRepository>();
        services.AddScoped<IVitalsRepository, VitalsRepository>();
        services.AddScoped<IConsultationRepository, ConsultationRepository>();
        services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
        services.AddScoped<IDispensingRepository, DispensingRepository>();
        services.AddScoped<IDrugHandoverRepository, DrugHandoverRepository>();
        services.AddScoped<ILabRepository, LabRepository>();
        services.AddScoped<IDressingRepository, DressingRepository>();
        services.AddScoped<IContactTraceRepository, ContactTraceRepository>();
        services.AddScoped<IRegisterRepository, RegisterRepository>();
        services.AddScoped<IServiceWindowRepository, ServiceWindowRepository>();
        return services;
    }

    public static IServiceCollection AddGileadCache(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["Redis:ConnectionString"]
            ?? throw new InvalidOperationException("Redis:ConnectionString is not configured.");
        services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(connectionString));
        services.AddScoped<IQueueCacheService, QueueCacheService>();
        return services;
    }
}
