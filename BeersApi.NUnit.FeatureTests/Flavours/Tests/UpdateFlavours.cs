using BeersApi.Models.Input.Flavours.Update;
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
   public class UpdateFlavours : NUnitFeatureTestBase
   {
      private UpdateFlavour _updateFlavour;
      private const string UpdateValidFlavourName = "Update Flavour Name";
      private const string UpdateValidFlavourDescription = "Update Flavour Description";
      private string _token;
      private int _flavourId;

      [SetUp]
      public void BeforeEachTest()
      {
         var claims = new List<Claim>
         {
            new Claim(ClaimTypes.Email, "test@email.com")
         };
         _token = TestEnvAuthentication.GenerateToken(claims);
         _updateFlavour = new UpdateFlavour { Name = UpdateValidFlavourName, Description = UpdateValidFlavourDescription };
         _flavourId = 1;
      }

      #region Tests

      [Test]
      public async Task UpdateFlavour_NoTokenProvided_ReturnsUnauthorized401()
      {
         //act
         _token = string.Empty;
         var response = await Exec(_token, _flavourId);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
      }

      [Test]
      public async Task UpdateFlavour_NoEmailClaim_ReturnsBadRequest400()
      {
         //arrange
         var claims = new List<Claim>
         {
            new Claim(JwtClaimTypes.GivenName, "testUser"),
            new Claim(JwtClaimTypes.Role, "User"),
            new Claim(ClaimTypes.NameIdentifier, "test-wrong-email")
         };

         _token = TestEnvAuthentication.GenerateToken(claims);

         //act
         var response = await Exec(_token, _flavourId).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Test]
      public async Task UpdateFlavour_NoBeersApiAdmin_ReturnsForbidden403()
      {
         //arrange
         var claims = new List<Claim>
         {
            new Claim(ClaimTypes.Email, "wrongemail@test.com")
         };

         _token = TestEnvAuthentication.GenerateToken(claims);

         //act
         var response = await Exec(_token, _flavourId).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
      }

      [Test]
      public async Task UpdateFlavour_FlavourWithSameNameAlreadyExists_ReturnsBadRequest400()
      {
         //arrange
         _updateFlavour.Name = "Apples";

         //act
         var response = await Exec(_token, _flavourId).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Test]
      public async Task UpdateFlavour_NoExistingFlavour_ReturnsNotFound404()
      {
         //arrange
         _flavourId = 9999;

         //act
         var response = await Exec(_token, _flavourId).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
      }

      [Test]
      public async Task UpdateFlavour__ValidInput_ReturnsOkAndUpdatedFlavour()
      {
         //act
         var response = await Exec(_token, _flavourId).ConfigureAwait(false);
         var category = await response.BodyAs<Models.Output.Flavours.Flavour>();

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.OK);
         category.UId.Should().NotBeEmpty();
         category.Id.Should().Be(_flavourId);
         category.Name.Should().Be(_updateFlavour.Name, "Is the same name sended on Exec();");
         category.Description.Should().Be(_updateFlavour.Description, "Is the same description sended on Exec();");
      }


      #endregion

      #region Private Methods

      private async Task<HttpResponseMessage> Exec(string token, int flavourId)
      {
         Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
         return await Client.PutAsJsonAsync($"flavours/{flavourId}", _updateFlavour).ConfigureAwait(false);
      }

      #endregion
   }
}
