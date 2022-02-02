using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_FluentAPI
{
    public class AirlinesDbContext : DbContext
    {
        public AirlinesDbContext()
        {
            //Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(@"data source=(LocalDb)\MSSQLLocalDB;initial catalog=TestAirlinesDb;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // FluentAPI Configurations
            modelBuilder.Entity<Flight>().HasKey(a => a.Number); // set primary key
            modelBuilder.Entity<Airplane>().Property(a => a.Model)
                                           .IsRequired().HasMaxLength(200);

            // Configure Relationships

            // Type: One to Many (1...*)
            modelBuilder.Entity<Airplane>().HasMany(a => a.Flights).WithOne(f => f.Airplane);

            modelBuilder.Entity<Flight>().HasOne(f => f.DepartureCity).WithMany(c => c.FlightsFrom);
            modelBuilder.Entity<Flight>().HasOne(f => f.ArrivalCity).WithMany(c => c.FlightsTo);

            modelBuilder.Entity<Country>().HasMany(c => c.Cities).WithOne(c => c.Country);
        }

        public DbSet<Flight> Flights { get; set; }
        public DbSet<Airplane> Airplanes { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
    }

    public class Airplane
    {
        public Airplane()
        {
            Flights = new HashSet<Flight>();
        }
        public int Id { get; set; }
        public string Model { get; set; }
        public int MaxPassangers { get; set; }
        public ICollection<Flight> Flights { get; set; }
    }

    public class Flight
    {
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public Airplane Airplane { get; set; }

        public City DepartureCity { get; set; }
        public City ArrivalCity { get; set; }
    }

    public class City
    {
        public int Id { get; set; }
        [Required, MaxLength(200)]
        public string Name { get; set; }
        public Country Country { get; set; }

        public ICollection<Flight> FlightsFrom { get; set; }
        public ICollection<Flight> FlightsTo { get; set; }
    }
    public class Country
    {
        public int Id { get; set; }
        [Required, MaxLength(200)]
        public string Name { get; set; }
        public ICollection<City> Cities { get; set; }
    }
}
