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
        DapperTypeHandlers.Register();

        services.AddSingleton<PostgresConnectionFactory>();
        services.AddScoped<IStaffRepository, StaffRepository>();
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
        var connectionString = Environment.GetEnvironmentVariable("REDIS_URL")
                        ?? configuration["Redis:ConnectionString"]
                        ?? throw new InvalidOperationException(
                            "A Redis connection string must be configured through " +
                            "REDIS_URL or Redis:ConnectionString.");

        //var redisOptions = ConfigurationOptions.Parse(connectionString);
        //var redisUser = configuration["Redis:User"];
        //var redisPassword = configuration["Redis:Password"];

        ConfigurationOptions configOptions;

        if (connectionString.StartsWith("redis://") || connectionString.StartsWith("rediss://"))
        {
            var uri = new Uri(connectionString);
            var userInfo = uri.UserInfo.Split(':');
            string password = userInfo.Length > 1 ? userInfo[1] : userInfo[0];

            configOptions = new ConfigurationOptions
            {
                EndPoints = { { uri.Host, uri.Port > 0 ? uri.Port : 6379 } },
                Password = password,
                Ssl = uri.Scheme == "rediss",
                AbortOnConnectFail = false
            };
        }
        else
        {
            configOptions = ConfigurationOptions.Parse(connectionString);
            configOptions.AbortOnConnectFail = false;
        }

        //if (!string.IsNullOrWhiteSpace(redisUser))
        //{
        //    redisOptions.User = redisUser;
        //}

        //if (!string.IsNullOrWhiteSpace(redisPassword))
        //{
        //    redisOptions.Password = redisPassword;
        //}

        services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(configOptions));
        services.AddScoped<IQueueCacheService, QueueCacheService>();
        return services;
    }
}
