using BeersApi.NUnit.FeatureTests.Infrastructure.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.SqlServer.Dac;
using NUnit.Framework;
using System;
using System.Configuration;
using System.IO;
using System.Net.Http;

namespace BeersApi.NUnit.FeatureTests
{
   [SetUpFixture]
   public class NUnitFeatureTestSetup
   {
      private string _applicationConnectionString;
      private IConfiguration _config;
      private WebApplicationFactory<Program> _factory;

      protected internal static HttpClient Client;
      protected internal static TestEnvAuthentication TestEnvAuthentication;

      [OneTimeSetUp]
      public void RunBeforeAnyTests()
      {
         var suffix = Guid.NewGuid().ToString().Replace("-", "").ToUpper();

         _config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

         _applicationConnectionString = EnsureUniqueDbName(_config.GetConnectionString("beersApi_db"), suffix);
         UpdateConfig();

         DeployDb(_applicationConnectionString, "BeersApi.DB.dacpac");

         _factory = new NUnitFeatureTestsApplicationFactory();
         Client = _factory.CreateClient();
         TestEnvAuthentication = new TestEnvAuthentication(_config);
      }

      [OneTimeTearDown]
      public void RunAfterAnyTests()
      {
         _factory?.Dispose();
         Client?.Dispose();
         DropDb(_applicationConnectionString);
      }

      public TestEnvAuthentication Jwt => new TestEnvAuthentication(_config);

      #region Private Methods

      private string EnsureUniqueDbName(string connectionString, string suffix)
      {
         var builder = new SqlConnectionStringBuilder(connectionString);
         builder.InitialCatalog += $"_{suffix}";

         return builder.ConnectionString;
      }

      private void DeployDb(string connectionString, string dacpacFileName)
      {
         var dbName = GetDbName(connectionString);

         var dacOptions = new DacDeployOptions
         {
            BlockOnPossibleDataLoss = false,
            CreateNewDatabase = true
         };

         dacOptions.SqlCommandVariableValues.Add("Intent", "Test");

         Console.WriteLine($"Deploying {dbName} ...");

         var dacServiceInstance = new DacServices(connectionString);

         var basePath = TestContext.CurrentContext.TestDirectory;
         var dacpacFile = Path.Combine(basePath, dacpacFileName);
         using var dacPac = DacPackage.Load(dacpacFile);
         dacServiceInstance.Deploy(dacPac, dbName, true, dacOptions);
      }

      private void DropDb(string connectionString)
      {
         var dbName = GetDbName(connectionString);
         Console.WriteLine($"Dropping {dbName}");

         // use "original" connection string otherwise DB will be in use
         using (var cnn = new SqlConnection(UseMasterDb(connectionString)))
         {
            cnn.Open();
            using (var cm = cnn.CreateCommand())
            {
               cm.CommandText = $"ALTER DATABASE [{dbName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; " +
                                $"DROP DATABASE [{dbName}]";

               cm.ExecuteNonQuery();
            }
         }
      }

      private string GetDbName(string connectionString)
      {
         var builder = new SqlConnectionStringBuilder(connectionString);
         return builder.InitialCatalog;
      }

      private string UseMasterDb(string connectionString)
      {
         var builder = new SqlConnectionStringBuilder(connectionString)
         {
            InitialCatalog = "master"
         };

         return builder.ConnectionString;
      }

      private void UpdateConfig()
      {
         try
         {
            var filePath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
            var json = File.ReadAllText(filePath);
            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

            void SetSectionPath(string key, string value)
            {
               var sectionPath = key.Split(":")[0];
               if (!string.IsNullOrEmpty(sectionPath))
               {
                  var keyPath = key.Split(":")[1];
                  jsonObj[sectionPath][keyPath] = value;
               }
               else
               {
                  jsonObj[sectionPath] = value; //if no sectionpath , just set the value
               }
            }

            SetSectionPath("ConnectionStrings:beersApi_db", _applicationConnectionString);
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(filePath, output);
         }
         catch (ConfigurationErrorsException)
         {
            Console.WriteLine("Error writing app settings");
         }
      }

      #endregion
   }
}
