using MedicalUnitSystem.Data;
using Microsoft.EntityFrameworkCore;
namespace MedicalUnitSystem.Helpers
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using HospitalContext dbContext =
                scope.ServiceProvider.GetRequiredService<HospitalContext>();

            dbContext.Database.Migrate();
        }
    }
}
