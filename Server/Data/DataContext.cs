using Microsoft.EntityFrameworkCore;
using BlazorFullStackCrud.Shared;
using System.Data.Entity;

namespace BlazorFullStackCrud.Server.Data
{
    public class DataContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comic>().HasData(
                  new Comic { Id = 1, Name = "Marvel" },
                  new Comic { Id = 2, Name = "DC" }
            );

            modelBuilder.Entity<SuperHero>().HasData(
                 new SuperHero
                 {
                     Id = 1,
                     FirstName = "Peter",
                     LastName = "Parker",
                     HeroName = "Spiderman",                   
                     ComicId = 1
                 },
                  new SuperHero
                  {
                      Id = 2,
                      FirstName = "Bruce",
                      LastName = "Wayne",
                      HeroName = "Batman",                      
                      ComicId = 2
                  }
             );
        }

        public Microsoft.EntityFrameworkCore.DbSet<SuperHero> SuperHeroes { get; set; }
        public Microsoft.EntityFrameworkCore.DbSet<Comic> Comics { get; set; }
    }
}
