using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ParkingApi.Model;

namespace ParkingApi.Repository
{
    public class ParkingApiDbContext : DbContext
    {
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Settings> Settings { get; set; }

        public ParkingApiDbContext(DbContextOptions<ParkingApiDbContext> options ): base(options)
        {
            
        }

        public void Seed(int maxHour, int occupation)
        {
            Settings.Add(new Settings
            {
                HourSetting = maxHour,
                Occupation = occupation
            });
            SaveChanges();
        }
    }
}