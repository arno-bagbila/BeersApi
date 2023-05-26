using BeersApi.Models.Input.Flavours.Create;
using BeersApi.NUnit.FeatureTests.Extensions;
using FluentAssertions;
using IdentityModel;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BeersApi.NUnit.FeatureTests.Flavours.Tests
{
   [TestFixture]
   public class CreateFlavours : NUnitFeatureTestBase
   {

      private CreateFlavour _createFlavour;
      private const string FlavourName = "Flavour Name";
      private const string FlavourDescription = "Flavour Description";
      private string _token;

      [SetUp]
      public void BeforeEachTest()
      {
         var claims = new List<Claim>
         {
            new Claim(ClaimTypes.Email, "test@email.com")
         };
         _token = TestEnvAuthentication.GenerateToken(claims);
         _createFlavour = new CreateFlavour { Name = FlavourName, Description = FlavourDescription };
      }

      #region Tests

      [Test]
      public async Task CreateFlavour_NoTokenProvided_ReturnsUnauthorized401()
      {
         //act
         _token = string.Empty;
         var response = await Exec(_token);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
      }

      [Test]
      public async Task CreateFlavour_NoEmailClaim_ReturnsBadRequest400()
      {
         //arrange
         var claims = new List<Claim>
         {
            new Claim(JwtClaimTypes.GivenName, "testUser"),
            new Claim(JwtClaimTypes.Role, "admin")
         };

         _token = TestEnvAuthentication.GenerateToken(claims);

         //act
         var response = await Exec(_token).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Test]
      public async Task CreateFlavour_NoBeersApiAdmin_ReturnsForbidden403()
      {
         //arrange
         var claims = new List<Claim>
         {
            new Claim(ClaimTypes.Email, "wrongemail@test.com")
         };

         _token = TestEnvAuthentication.GenerateToken(claims);

         //act
         var response = await Exec(_token).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
      }

      [Test]
      public async Task CreateFlavour_FlavourWithSameNameAlreadyExists_ReturnsBadRequest400()
      {
         //arrange
         _createFlavour.Name = "Acidity";

         //act
         var response = await Exec(_token).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Test]
      public async Task CreateFlavour_ValidInputWithBeersApiAdmin_ReturnsCreatedFlavour201()
      {
         //act
         var response = await Exec(_token).ConfigureAwait(false);
         var flavour = await response.BodyAs<Models.Output.Flavours.Flavour>();

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.Created);
         flavour.UId.Should().NotBeEmpty();
         flavour.Name.Should().Be(_createFlavour.Name, "Is the same name sended on Exec();");
         flavour.Description.Should().Be(_createFlavour.Description, "Is the same description sended on Exec();");
      }

      #endregion

      #region Private Methods

      private async Task<HttpResponseMessage> Exec(string token)
      {
         Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
         return await Client.PostAsJsonAsync("flavours", _createFlavour).ConfigureAwait(false);
      }

      #endregion
   }
}
