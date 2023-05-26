using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DataAccess
{
   public class ContextFactory : IDesignTimeDbContextFactory<BeersApiContext>
   {
      private static string _connectionString;

      public BeersApiContext CreateDbContext()
      {
         return CreateDbContext(null);
      }

      public BeersApiContext CreateDbContext(string[] args)
      {
         if (string.IsNullOrEmpty(_connectionString))
         {
            LoadConnectionString();
         }

         var builder = new DbContextOptionsBuilder<BeersApiContext>();
         builder.EnableSensitiveDataLogging();
         builder.UseSqlServer(_connectionString);

         return new BeersApiContext(builder.Options);
      }

      private static void LoadConnectionString()
      {
         var builder = new ConfigurationBuilder();
         builder.AddJsonFile("appsettings.json", optional: false);

         var configuration = builder.Build();
         _connectionString =
            configuration.GetConnectionString("beersApi_db"); //configuration["sqlserverconnection:connectionString"];
      }
   }
}
