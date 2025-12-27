using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ISI_TP2_10444_SmartHealth_Data
{
    public class SmartHealthContext : DbContext
    {

        public SmartHealthContext(DbContextOptions<SmartHealthContext> options) : base(options) { }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Alert> Alerts { get; set; }
        //public DbSet<Wearable> Wearables { get; set; }
        //public DbSet<VitalSign> VitalSigns { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Alert>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Alerts)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
