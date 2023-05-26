using DataAccess;
using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace BeersApi.IntegrationTests.Helpers
{
   public class DbContextFactory : IDisposable
   {
      public BeersApiContext Context { get; private set; }

      public DbContextFactory()
      {
         var dbBuilder = GetContextBuilderOptions<BeersApiContext>("beersApi_db");
         Context = new BeersApiContext(dbBuilder.Options);
         Context.Database.EnsureCreatedAsync();
         SeedTest();
      }

      public void Dispose()
      {
         Context.Dispose();
      }

      public BeersApiContext GetRefreshContext()
      {
         var dbBuilder = GetContextBuilderOptions<BeersApiContext>("beersApi_db");
         Context = new BeersApiContext(dbBuilder.Options);
         return Context;
      }

      private DbContextOptionsBuilder<BeersApiContext> GetContextBuilderOptions<T>(string connectionStringName)
      {
         var connectionString = ConfigurationSingleton.GetConfiguration().GetConnectionString(connectionStringName);
         var contextBuilder = new DbContextOptionsBuilder<BeersApiContext>();
         contextBuilder.EnableSensitiveDataLogging();
         var servicesCollection = new ServiceCollection().AddEntityFrameworkSqlServer().BuildServiceProvider();

         contextBuilder.UseSqlServer(connectionString).UseInternalServiceProvider(servicesCollection);

         return contextBuilder;
      }

      private void SeedTest()
      {
         //categories
         var category = Category.Create("FirstCategory", "description");
         var secondCategory = Category.Create("SecondCategory", "description");

         //colors
         var color = Color.Create("FirstColor");
         var secondColor = Color.Create("SecondColor");

         //countries
         var country = Country.Create("Burkina Faso", "BUR");
         var secondCountry = Country.Create("United Kingdom", "UK");

         //flavours
         var flavour = Flavour.Create("FirstFlavour", "description");
         var secondFlavour = Flavour.Create("SecondFlavour", "description");

         //beers
         var beer = Beer.Create("FirstBeer", "description",
            "http://127.0.0.1:10000/devstoreaccount1/beers/default_beer_edited.jpg", 6.6, 4.3, category, color,
            country);
         var secondBeer = Beer.Create("SecondBeer", "description",
            "http://127.0.0.1:10000/devstoreaccount1/beers/default_beer_edited.jpg", 8.9, 4.5, secondCategory,
            secondColor, secondCountry);

         Context.AddRange(color, secondColor, category, secondCategory, country, 
            secondCountry, flavour, secondFlavour, beer, secondBeer);
         Context.SaveChanges();
      }
   }
}
