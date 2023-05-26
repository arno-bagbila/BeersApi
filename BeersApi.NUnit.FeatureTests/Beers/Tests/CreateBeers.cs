using BeersApi.Models.Input.Beers.Create;
using BeersApi.Models.Output.Beers;
using BeersApi.NUnit.FeatureTests.Extensions;
using FluentAssertions;
using IdentityModel;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BeersApi.NUnit.FeatureTests.Beers.Tests
{

   [TestFixture]
   public class CreateBeers : NUnitFeatureTestBase
   {
      private CreateBeer _createBeer;
      private const string BeerName = "Beer Name";
      private const string BeerDescription = "Beer Description";
      private string _token;

      [SetUp]
      public void BeforeEachTest()
      {
         var claims = new List<Claim>
         {
            new Claim(ClaimTypes.Email, "test@email.com")
         };
         _token = TestEnvAuthentication.GenerateToken(claims);

         _createBeer = new CreateBeer
         {
            Name = BeerName,
            Description = BeerDescription,
            AlcoholLevel = 7.5,
            TiwooRating = 4.6,
            CategoryId = 1,
            ColorId = 1,
            CountryId = 1,
            FlavourIds = new List<int> { 1 },
            LogoUrl = "http://127.0.0.1:10000/devstoreaccount1/beersapilogourls/leffe_brun_logo.jpg"
         };
      }

      #region Tests


      [Test]
      public async Task CreateBeer_NoTokenProvided_ReturnsUnauthorized401()
      {
         //act
         _token = string.Empty;
         var response = await Exec(_token);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
      }

      [Test]
      public async Task CreateBeer_NoEmailClaim_ReturnsBadRequest400()
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
      public async Task CreateBeer_CategoryNoExisting_ReturnsBadRequest()
      {
         //arrange
         _createBeer.CountryId = 9999;

         //act
         var response = await Exec(_token).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
      }

      [Test]
      public async Task CreateBeer_NoBeersApiAdmin_ReturnsForbidden403()
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
      public async Task CreateBeer_ColorNoExisting_ReturnsBadRequest()
      {
         //arrange
         _createBeer.ColorId = 9999;

         //act
         var response = await Exec(_token).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
      }

      [Test]
      public async Task CreateBeer_CountryNoExisting_ReturnsBadRequest()
      {
         //arrange
         _createBeer.CountryId = 9999;

         //act
         var response = await Exec(_token).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
      }

      [Test]
      public async Task CreateBeer_FlavourNoExisting_ReturnsBadRequest()
      {
         //arrange
         _createBeer.FlavourIds = new List<int> { 9998, 9999 };

         //act
         var response = await Exec(_token).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
      }

      [Test]
      public async Task CreateBeer_BeerWithSameNameAlreadyExists_ReturnsBadRequest400()
      {
         //arrange
         _createBeer.Name = "BeerTest";

         //act
         var response = await Exec(_token).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Test]
      public async Task CreateBeer_ValidInputWithBeersApiAdmin_ReturnsCreatedBeer201()
      {
         //act
         var response = await Exec(_token).ConfigureAwait(false);
         var beer = await response.BodyAs<Beer>();

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.Created);
         beer.UId.Should().NotBeEmpty();
         beer.Name.Should().Be(_createBeer.Name, "Is the same name sended on Exec();");
         beer.Description.Should().Be(_createBeer.Description, "Is the same description sended on Exec();");
         beer.Color.Name.Should().Be("Black");
         beer.Category.Name.Should().Be("Abbey (Abbaye, Abdji)");
         beer.Country.Name.Should().Be("Belgium");
         beer.Flavours.First().Name.Should().Be("Acidity");
      }

      #endregion

      #region Private Methods

      private async Task<HttpResponseMessage> Exec(string token)
      {
         Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
         return await Client.PostAsJsonAsync("beers", _createBeer).ConfigureAwait(false);
      }

      #endregion
   }
}
