using BeersApi.Models.Input.Beers.Create;
using BeersApi.Models.Input.Beers.Update;
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
   [TestFixture]
   public class UpdateBeers : NUnitFeatureTestBase
   {
      private UpdateBeer _updateBeer;
      private string _token;
      private int _beerId;

      [SetUp]
      public async Task BeforeEachTest()
      {
         var claims = new List<Claim>
         {
            new Claim(ClaimTypes.Email, "test@email.com")
         };
         _token = TestEnvAuthentication.GenerateToken(claims);

         _updateBeer = new UpdateBeer
         {
            Name = "BeerUpdated",
            Description = "BeerUpdatedDescription",
            AlcoholLevel = 6.6,
            CategoryId = 2,
            ColorId = 2,
            CountryId = 2,
            FlavourIds = new List<int> { 2 },
            LogoUrl = "http://127.0.0.1:10000/devstoreaccount1/beersapilogourls/leffe_brun_logo.jpg",
            TiwooRating = 4.6
         };

         var beerToCreate = new CreateBeer
         {
            Name = "BeerToCreate",
            Description = "BeerToCreateDescription",
            AlcoholLevel = 6.5,
            CategoryId = 1,
            ColorId = 1,
            CountryId = 1,
            FlavourIds = new List<int> { 1, 2 },
            LogoUrl = "http://127.0.0.1:10000/devstoreaccount1/beersapilogourls/leffe_brun_logo.jpg",
            TiwooRating = 4.6
         };

         var secondBeerToCreate = new CreateBeer
         {
            Name = "SecondBeerToCreate",
            Description = "SecondBeerToCreateDescription",
            AlcoholLevel = 6.4,
            CategoryId = 1,
            ColorId = 1,
            CountryId = 1,
            FlavourIds = new List<int> { 1, 2 },
            LogoUrl = "http://127.0.0.1:10000/devstoreaccount1/beersapilogourls/leffe_brun_logo.jpg",
            TiwooRating = 4.7
         };

         Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
         var createBeerResponse = await Client.PostAsJsonAsync("beers", beerToCreate).ConfigureAwait(false);
         await Client.PostAsJsonAsync("beers", secondBeerToCreate).ConfigureAwait(false);
         var beerToUpdate = await createBeerResponse.BodyAs<Models.Output.Beers.Beer>();
         _beerId = beerToUpdate.Id;
      }

      #region Tests

      [Test]
      public async Task UpdateBeer_NoTokenProvided_ReturnsUnauthorized401()
      {
         //act
         _token = string.Empty;
         var response = await Exec(_token, _beerId);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
      }

      [Test]
      public async Task UpdateBeer_NoEmailClaim_ReturnsBadRequest400()
      {
         //arrange
         var claims = new List<Claim>
         {
            new Claim(JwtClaimTypes.GivenName, "testUser"),
            new Claim(JwtClaimTypes.Role, "User"),
            new Claim(ClaimTypes.NameIdentifier, "testwrong@email.com")
         };

         _token = TestEnvAuthentication.GenerateToken(claims);

         //act
         var response = await Exec(_token, _beerId).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Test]
      public async Task UpdateBeer_NoBeersApiAdmin_ReturnsForbidden403()
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
      public async Task UpdateBeer_NoExistingBeer_ReturnsNotFound404()
      {
         //arrange
         _beerId = 9999;

         //act
         var response = await Exec(_token, _beerId).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
      }

      [Test]
      public async Task UpdateBeer_NoExistingCategory_ReturnsNotFound404()
      {
         //arrange
         _updateBeer.CategoryId = 9999;

         //act
         var response = await Exec(_token, _beerId).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
      }

      [Test]
      public async Task UpdateBeer_NoExistingColor_ReturnsNotFound404()
      {
         //arrange
         _updateBeer.ColorId = 9999;

         //act
         var response = await Exec(_token, _beerId).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
      }

      [Test]
      public async Task UpdateBeer_NoExistingCountry_ReturnsNotFound404()
      {
         //arrange
         _updateBeer.CountryId = 9999;

         //act
         var response = await Exec(_token, _beerId).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
      }

      [Test]
      public async Task UpdateBeer_BeerWithSameNameAlreadyExists_ReturnsBadRequest400()
      {
         //arrange
         _updateBeer.Name = "SecondBeerToCreate";

         //act
         var response = await Exec(_token, _beerId).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Test]
      public async Task UpdateBeer_ValidData_ReturnsUpdatedBeer()
      {
         //arrange

         var beerToCreate = new CreateBeer
         {
            Name = "ValidBeerToCreate",
            Description = "BeerToCreateDescription",
            AlcoholLevel = 6.5,
            CategoryId = 1,
            ColorId = 1,
            CountryId = 1,
            FlavourIds = new List<int> { 1, 2 },
            LogoUrl = "http://127.0.0.1:10000/devstoreaccount1/beersapilogourls/leffe_brun_logo.jpg",
            TiwooRating = 4.6
         };

         Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
         var createBeerResponse = await Client.PostAsJsonAsync("beers", beerToCreate).ConfigureAwait(false);
         var beerToUpdate = await createBeerResponse.BodyAs<Beer>();
         var beerId = beerToUpdate.Id;

         //act
         var response = await Exec(_token, beerId).ConfigureAwait(false);
         var beer = await response.BodyAs<Beer>();

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.OK);
         beer.Should().NotBeNull();
         beer.Name.Should().Be("BeerUpdated");
         beer.Description.Should().Be("BeerUpdatedDescription");
         beer.AlcoholLevel.Should().Be(6.6);
         beer.TiwooRating.Should().Be(4.6);
         beer.Category.Id.Should().Be(2);
         beer.Category.Name.Should().Be("Altbier");
         beer.Color.Id.Should().Be(2);
         beer.Color.Name.Should().Be("Brown");
         beer.Country.Id.Should().Be(2);
         beer.Country.Name.Should().Be("France");
      }

      #endregion

      #region Private Methods

      private async Task<HttpResponseMessage> Exec(string token, int beerId)
      {
         Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
         return await Client.PutAsJsonAsync($"beers/{beerId}", _updateBeer).ConfigureAwait(false);
      }

      #endregion

   }
}
