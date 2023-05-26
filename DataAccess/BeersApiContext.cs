using DataAccess.Configurations;
using Domain.Authorization;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DataAccess
{
   public class BeersApiContext : DbContext, IBeersApiContext
   {
      public BeersApiContext(DbContextOptions options) : base(options) { }

      public DbSet<Category> Categories { get; set; }

      public DbSet<Country> Countries { get; set; }


      public DbSet<Color> Colors { get; set; }

      public DbSet<Flavour> Flavours { get; set; }

      public DbSet<Beer> Beers { get; set; }

      public DbSet<Image> Images { get; set; }

      public DbSet<User> Users { get; set; }


      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
         modelBuilder.ApplyConfiguration(new CountryConfiguration());
         modelBuilder.ApplyConfiguration(new CategoryConfiguration());
         modelBuilder.ApplyConfiguration(new ColorConfiguration());
         modelBuilder.ApplyConfiguration(new FlavourConfiguration());
         modelBuilder.ApplyConfiguration(new BeerConfiguration());
         modelBuilder.ApplyConfiguration(new BeerFlavourConfiguration());
         modelBuilder.ApplyConfiguration(new ImageConfiguration());
         modelBuilder.ApplyConfiguration(new UserConfiguration());
         modelBuilder.ApplyConfiguration(new CommentConfiguration());
         //modelBuilder.SeedCategories();
         //modelBuilder.SeedCountries();
         //modelBuilder.SeedColors();
         //modelBuilder.SeedFlavours();
      }

      public override int SaveChanges()
      {
         return base.SaveChanges();
      }

      public Task<int> SaveChangesAsync()
      {
         return SaveChangesAsync(CancellationToken.None);
      }

      public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
      {
         return await base.SaveChangesAsync(cancellationToken);
      }
   }
}
