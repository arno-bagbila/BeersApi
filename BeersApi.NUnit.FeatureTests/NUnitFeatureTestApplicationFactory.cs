using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.IO;
using System.Text;

namespace BeersApi.NUnit.FeatureTests
{
   /// <summary>
   /// Factory for bootstrapping an application for in memory functional tests end to end
   /// </summary>
   public class NUnitFeatureTestsApplicationFactory : WebApplicationFactory<Program> 
   {
      private readonly string _environment;

      public NUnitFeatureTestsApplicationFactory(string environment = "Development")
      {
         _environment = environment;
      }

      protected override IHost CreateHost(IHostBuilder builder)
      {
         builder.UseEnvironment(_environment);


         builder.ConfigureServices(services =>
         {
            services.AddAuthentication(options =>
            {
               options.DefaultAuthenticateScheme = "Test";
            })
            .AddJwtBearer("Test", GetJwtBearerOptions);
         });

         builder.ConfigureAppConfiguration((hostingContext, config) =>
         {
            config.Sources.Clear();

            config.SetBasePath(Directory.GetCurrentDirectory());
            config
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true,
                     reloadOnChange: true);
         });


         return base.CreateHost(builder);
      }

      private void GetJwtBearerOptions(JwtBearerOptions options)
      {
         options.TokenValidationParameters = new TokenValidationParameters
         {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = "yes",
            ValidAudience = "valid",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("THIS IS USED TO SIGN AND VERIFY JWT TOKENS, REPLACE IT WITH YOUR OWN SECRET, IT CAN BE ANY STRING"))
         };
      }
   }
}
