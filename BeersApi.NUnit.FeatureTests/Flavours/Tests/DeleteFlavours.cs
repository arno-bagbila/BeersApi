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
   public class DeleteFlavours : NUnitFeatureTestBase
   {
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

      }

      [Test]
      public async Task DeleteFlavour_NoTokenProvided_ReturnsUnauthorized401()
      {
         //act
         _token = string.Empty;
         var response = await Exec(_token, _flavourId);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
      }

      [Test]
      public async Task DeleteFlavour_NoEmailClaim_ReturnsBadRequest400()
      {
         //arrange
         var claims = new List<Claim>
         {
            new Claim(JwtClaimTypes.GivenName, "testUser"),
            new Claim(JwtClaimTypes.Role, "admin")
         };

         _token = TestEnvAuthentication.GenerateToken(claims);

         //act
         var response = await Exec(_token, _flavourId).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Test]
      public async Task DeleteFlavour_NoBeersApiAdmin_ReturnsForbidden403()
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
      public async Task DeleteFlavour_NoExistingFlavour_ReturnsNotFound404()
      {
         //arrange
         _flavourId = 99999;

         //act
         var response = await Exec(_token, _flavourId).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
      }

      [Test]
      public async Task DeleteFlavour_ExistingFlavour_ReturnedDeletedFlavour()
      {
         //arrange
         Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
         var createFlavourResponse = await Client.PostAsJsonAsync("flavours", new CreateFlavour { Name = "FlavourToDelete", Description = "FlavourToDeleteDescription" }).ConfigureAwait(false);
         var flavourToDelete = await createFlavourResponse.BodyAs<Models.Output.Flavours.Flavour>();
         _flavourId = flavourToDelete.Id;

         //act
         var response = await Exec(_token, _flavourId).ConfigureAwait(false);
         var flavour = await response.BodyAs<Models.Output.Flavours.Flavour>();

         //assert
         flavour.Should().NotBeNull();
         flavour.UId.Should().NotBeEmpty();
         flavour.Name.Should().Be("FlavourToDelete", "It is the name of the deleted flavour");
         flavour.Description.Should().Be("FlavourToDeleteDescription", "It is the description of the deleted flavour");
         flavour.Id.Should().Be(_flavourId);
      }

      #region Private Methods

      private async Task<HttpResponseMessage> Exec(string token, int flavourId)
      {
         Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
         return await Client.DeleteAsync($"flavours/{flavourId}").ConfigureAwait(false);
      }

      #endregion
   }
}
