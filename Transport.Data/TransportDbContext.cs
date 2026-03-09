using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Transport.Data.Models;

namespace Transport.Data
{
    public class TransportDbContext : DbContext
    {
        public DbSet<BusStop> BusStops { get; set; }
        public DbSet<BusLine> BusLines { get; set; }
        public DbSet<RouteStop> RouteStops { get; set; }
        public DbSet<Schedule> Schedules { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=TransportSystem.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BusStop>().Property(s => s.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<BusLine>().Property(l => l.Number).IsRequired();
        }
    }
}
