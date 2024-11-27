using MedicalUnitSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MedicalUnitSystem.Data
{
    public class HospitalContext : IdentityDbContext<ApplicationUser>
    {
        public HospitalContext(DbContextOptions<HospitalContext> options) : base(options) { }

        public DbSet<Patient> Patients { get; set; }        
        public DbSet<Consultation> Consultations { get; set; }
        public DbSet<LaboratoryTest> LaboratoryTests { get; set; }
        public DbSet<LaboratoryTestType> LaboratoryTestTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}


