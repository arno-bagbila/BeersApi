using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace BeersApi.NUnit.FeatureTests.Extensions
{
   public static class HttpResponseMessageExtensions
   {
      public static async Task<T> BodyAs<T>(this HttpResponseMessage httpResponseMessage)
      {
         var bodyString = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
         return JsonConvert.DeserializeObject<T>(bodyString);
      }
   }
}
