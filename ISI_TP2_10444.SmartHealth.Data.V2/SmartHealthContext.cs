using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ISI_TP2_10444_SmartHealth_Data
{
    public class SmartHealthContext : DbContext
    {

        public SmartHealthContext(DbContextOptions<SmartHealthContext> options) : base(options) { }

        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<Alert> Alerts => Set<Alert>();
        public DbSet<Wearable> Wearables => Set<Wearable>();
        public DbSet<VitalSignRecord> VitalSignRecords => Set<VitalSignRecord>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Patient
            modelBuilder.Entity<Patient>().HasKey(p => p.PatientId);

            //Alert
            modelBuilder.Entity<Alert>().HasKey(a => a.AlertId);
            modelBuilder.Entity<Alert>()
                .HasOne<Patient>() // Alert has one Patient
                .WithMany() // Patient has many Alerts
                .HasForeignKey(a => a.PatientId) // Foreign key in Alert
                .OnDelete(DeleteBehavior.Cascade); // If Patient is deleted, delete related Alerts

            //Wearable
            modelBuilder.Entity<Wearable>().HasKey(w => w.WearableId);
            modelBuilder.Entity<Wearable>()
                .HasOne<Patient>() // Wearable has one Patient
                .WithMany() // Patient has many Wearables
                .HasForeignKey(w => w.PatientId) // Foreign key in Wearable
                .OnDelete(DeleteBehavior.Cascade); // If Patient is deleted, delete related Wearables

            // VitalSigns
            modelBuilder.Entity<VitalSignRecord>().HasKey(v => v.RecordId);
            modelBuilder.Entity<VitalSignRecord>()
                .HasOne<Wearable>()
                .WithMany()
                .HasForeignKey(v => v.WearableId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
