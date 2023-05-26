using System.IO;
using Microsoft.Extensions.Configuration;

namespace BeersApi.IntegrationTests.Helpers
{
   public class ConfigurationSingleton
   {
      private static IConfigurationRoot _configuration;

      private static IConfigurationBuilder _configurationBuilder;
      private ConfigurationSingleton() { }

      public static IConfigurationRoot GetConfiguration()
      {
         return _configuration ??= new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Path.GetFullPath("../../../")))
            .AddJsonFile("appsettings.Test.json")
            .AddEnvironmentVariables()
            .Build();
      }
   }
}
