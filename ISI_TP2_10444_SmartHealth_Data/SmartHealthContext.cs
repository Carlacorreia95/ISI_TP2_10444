using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ISI_TP2_10444_SmartHealth_Data
{
    public class SmartHealthContext : AppDbContext
    {
   
        public SmartHealthContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Patient> Pacientes { get; set; }
        public DbSet<Wearable> Wearables { get; set; }
        public DbSet<RegisterSignal> RegisterSignals { get; set; }
        public DbSet<Alert> Alerts { get; set; }
    }
}
