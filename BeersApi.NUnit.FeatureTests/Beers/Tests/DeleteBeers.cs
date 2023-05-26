using BeersApi.Models.Input.Beers.Create;
using BeersApi.Models.Output.Beers;
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

namespace BeersApi.NUnit.FeatureTests.Beers.Tests
{
   public class DeleteBeers : NUnitFeatureTestBase
   {

      private string _token;
      private int _beerId;

      [SetUp]
      public void BeforeEachTest()
      {
         var claims = new List<Claim>
         {
            new Claim(ClaimTypes.Email, "test@email.com")
         };
         _token = TestEnvAuthentication.GenerateToken(claims);

      }

      #region Tests

      [Test]
      public async Task DeleteBeer_NoTokenProvided_ReturnsUnauthorized401()
      {
         //act
         _token = string.Empty;
         var response = await Exec(_token, _beerId);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
      }

      [Test]
      public async Task DeleteBeer_NoEmailClaim_ReturnsBadRequest400()
      {
         //arrange
         var claims = new List<Claim>
         {
            new Claim(JwtClaimTypes.GivenName, "testUser"),
            new Claim(JwtClaimTypes.Role, "admin")
         };

         _token = TestEnvAuthentication.GenerateToken(claims);

         //act
         var response = await Exec(_token, _beerId).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Test]
      public async Task DeleteBeer_NoBeersApiAdmin_ReturnsForbidden403()
      {
         //arrange
         var claims = new List<Claim>
         {
            new Claim(ClaimTypes.Email, "wrongemail@test.com")
         };

         _token = TestEnvAuthentication.GenerateToken(claims);

         //act
         var response = await Exec(_token, _beerId).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
      }

      [Test]
      public async Task DeleteBeer_NoExistingCategory_ReturnsNotFound404()
      {
         //arrange
         _beerId = 99999;

         //act
         var response = await Exec(_token, _beerId).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
      }

      [Test]
      public async Task DeleteBeer_ExistingBeer_ReturnedDeletedCategory()
      {
         //arrange
         var beerToDelete = new CreateBeer
         {
            Name = "ValidBeerToDelete",
            Description = "BeerToDeleteDescription",
            AlcoholLevel = 6.5,
            CategoryId = 1,
            ColorId = 1,
            CountryId = 1,
            FlavourIds = new List<int> { 1, 2 },
            LogoUrl = "http://127.0.0.1:10000/devstoreaccount1/beersapilogourls/leffe_brun_logo.jpg",
            TiwooRating = 4.6
         };

         Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
         var createBeerResponse = await Client.PostAsJsonAsync("beers", beerToDelete).ConfigureAwait(false);
         var beerCreated = await createBeerResponse.BodyAs<Beer>();
         _beerId = beerCreated.Id;

         //act
         var response = await Exec(_token, _beerId).ConfigureAwait(false);
         var beer = await response.BodyAs<Beer>();

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.OK);
         beer.Should().NotBeNull();
         beer.UId.Should().NotBeEmpty();
         beer.Name.Should().Be("ValidBeerToDelete", "It is the name of the deleted beer");
         beer.Description.Should().Be("BeerToDeleteDescription", "It is the description of the deleted beer");
         beer.Id.Should().Be(_beerId);
      }

      #endregion


      #region Private Methods

      private async Task<HttpResponseMessage> Exec(string token, int beerId)
      {
         Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
         return await Client.DeleteAsync($"beers/{beerId}").ConfigureAwait(false);
      }

      #endregion
   }
}
