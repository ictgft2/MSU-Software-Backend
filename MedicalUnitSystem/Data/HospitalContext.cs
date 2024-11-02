using System;
using MedicalUnitSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MedicalUnitSystem.Data
{
    public class HospitalContext : IdentityDbContext<ApplicationUser>
    {
        public HospitalContext(DbContextOptions<HospitalContext> options) : base(options) { }

        //public DbSet<Patient> Patients { get; set; }
        //public DbSet<UserRegistrationModel> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}


