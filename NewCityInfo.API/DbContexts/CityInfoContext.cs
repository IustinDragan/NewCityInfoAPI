using System.Drawing;
using Microsoft.EntityFrameworkCore;
using NewCityInfo.API.Entities;

namespace NewCityInfo.API.DbContexts;

public class CityInfoContext : DbContext
{
    public DbSet<City> Cities { get; set; } = null!;
    public DbSet<PointOfInterest> PointOfInterests { get; set; } = null!;

    public CityInfoContext(DbContextOptions<CityInfoContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>()
            .HasData(
                new City("New York City")
                {
                    Id = 1,
                    Description ="The one with that big park."
                },
                new City("Antwerp")
                {
                    Id=2,
                    Description = "The one with the cathedral that is never really finished."
                },
                new City("Paris")
                {
                    Id=3,
                    Description = "The one with that big tower."
                });
        modelBuilder.Entity<PointOfInterest>()
            .HasData(
                new PointOfInterest("Central Park")
                {
                    Id = 1,
                    CityId = 1,
                    Description = "The most visited urban park in the United States."
                },
                new PointOfInterest("Empire State Building")
                {
                    Id = 2,
                    CityId = 1,
                    Description = "A 102 - story skyscraper located in Midtown Manhattan."
                },
                new PointOfInterest("Cathedral")
                {
                    Id = 3,
                    CityId = 2,
                    Description = "A Gothic style cathedral, conceived by architects Jan and Piere Appelmans."
                },
                new PointOfInterest("Antwerp Central Station")
                {
                    Id = 4,
                    CityId = 2,
                    Description = "The finest example of railway architecture in Belgium."
                },
                new PointOfInterest("Eiffel Tower")
                {
                    Id = 5,
                    CityId = 3,
                    Description = "A wrought iron lattiece tower on the Champ de Mars, named..."
                },
                new PointOfInterest("The Louvre")
                {
                    Id = 6,
                    CityId = 3,
                    Description = "the world's largest museum"
                });
        base.OnModelCreating(modelBuilder);
    }
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseSqlite("connectionstring");
    //     base.OnConfiguring(optionsBuilder);
    // }
}