using BeersApi.NUnit.FeatureTests.Extensions;
using FluentAssertions;
using IdentityModel;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BeersApi.NUnit.FeatureTests.Users.UserTests
{
   [TestFixture]
   public class GetUsers : NUnitFeatureTestBase
   {
      private string _token;
      private string _email;

      [SetUp]
      public void BeforeEachTest()
      {
         var claims = new List<Claim>
         {
            new Claim(JwtClaimTypes.GivenName, "testUser"),
         };
         _token = TestEnvAuthentication.GenerateToken(claims);
         _email = "test@email.com";
      }

      #region Tests

      [Test]
      public async Task GetUser_ByEmail_ReturnsUser()
      {
         //act

         var response = await Exec(_token).ConfigureAwait(false);
         var user = await response.BodyAs<Models.Output.Users.User>();

         //assert
         user.Should().NotBeNull();
      }

      [Test]
      public async Task GetUser_NoToken_ReturnsUnauthorized401()
      {
         //arrange
         _token = string.Empty;

         //act
         var response = await Exec(_token).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
      }

      #endregion


      private async Task<HttpResponseMessage> Exec(string token)
      {
         Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
         return await Client.GetAsync($"users/{_email}").ConfigureAwait(false);
      }
   }
}
