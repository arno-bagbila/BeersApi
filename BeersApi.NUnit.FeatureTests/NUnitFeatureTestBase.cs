using BeersApi.NUnit.FeatureTests.Infrastructure.Authentication;
using System.Net.Http;
using System.Threading.Tasks;

namespace BeersApi.NUnit.FeatureTests
{
   public class NUnitFeatureTestBase
   {
      protected HttpClient Client = NUnitFeatureTestSetup.Client;
      protected TestEnvAuthentication TestEnvAuthentication = NUnitFeatureTestSetup.TestEnvAuthentication;

      protected async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
      {
         return await Client.SendAsync(request);
      }

   }
}
