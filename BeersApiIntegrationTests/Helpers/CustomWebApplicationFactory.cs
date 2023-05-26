using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using BeersApi.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace BeersApi.IntegrationTests.Helpers
{
   public class CustomWebApplicationFactory<TStartup>
      : WebApplicationFactory<TStartup> where TStartup : class
   {
      private readonly HttpClient _client;

      public CustomWebApplicationFactory()
      {
         CreateHostBuilder();
         _client = CreateClient();
      }

      protected override IHostBuilder CreateHostBuilder()
      {
         var hostBuilder = new HostBuilder()
            .ConfigureWebHost(webhost =>
            {
               //add TestServer
               webhost.UseTestServer();
            })
            .ConfigureAppConfiguration(config =>
            {
               config.SetBasePath(Path.Combine(Path.GetFullPath("../../../")))
                  .AddJsonFile("appsettings.Test.json")
                  .AddEnvironmentVariables();
            })
            .UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureWebHostDefaults(webBuilder =>
            {
               webBuilder.UseStartup<TStartup>();
               webBuilder.ConfigureTestServices(services =>
               {
                  services
                     .AddAuthentication(GetAuthenticationOptions)
                     .AddJwtBearer("Test", GetJwtBearerOptions);
               });
            });

         return hostBuilder;
      }

      public Task<HttpResponseMessage> Get(string url) => _client.GetAsync(url);

      public Task<HttpResponseMessage> Post<T>(string url, T body) => _client.PostAsJsonAsync(url, body);

      public Task<HttpResponseMessage> Put<T>(string url, T body) => _client.PutAsJsonAsync(url, body);

      public Task<HttpResponseMessage> Delete(string url) => _client.DeleteAsync(url);

      public CustomWebApplicationFactory<TStartup> AddAuth(string token)
      {
         _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
         return this;
      }

      public JwtAuthenticationTests Jwt => new JwtAuthenticationTests(ConfigurationSingleton.GetConfiguration());

      private static void GetAuthenticationOptions(AuthenticationOptions options)
      {
         options.DefaultAuthenticateScheme = "Test";
      }

      private void GetJwtBearerOptions(JwtBearerOptions options)
      {
         options.TokenValidationParameters = new TokenValidationParameters
         {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = ConfigurationSingleton.GetConfiguration()["Jwt:ValidIssuer"],
            ValidAudience = ConfigurationSingleton.GetConfiguration()["Jwt:ValidAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationSingleton.GetConfiguration()["Jwt:SecurityKey"]))
         };
      }

   }
}