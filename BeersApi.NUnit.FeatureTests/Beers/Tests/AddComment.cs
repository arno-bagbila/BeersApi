using BeersApi.Models.Input.Beers.Comments;
using BeersApi.Models.Input.Beers.Create;
using BeersApi.Models.Output.Beers;
using BeersApi.NUnit.FeatureTests.Extensions;
using BeersApi.NUnit.FeatureTests.Infrastructure.Authentication;
using FluentAssertions;
using IdentityModel;
using NUnit.Framework;
using System;
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
   public class AddComment : NUnitFeatureTestBase
   {
      private CreateComment _createComment;
      private string _token;

      [SetUp]
      public async Task BeforeEachTest()
      {
         var claims = new List<Claim>
         {
            new Claim(ClaimTypes.Email, "test@email.com")
         };
         _token = TestEnvAuthentication.GenerateToken(claims);

         var beerToCreate = new CreateBeer
         {
            Name = "CommentBeerToCreate",
            Description = "CommentBeerToCreateDescription",
            AlcoholLevel = 6.5,
            CategoryId = 1,
            ColorId = 1,
            CountryId = 1,
            FlavourIds = new List<int> { 1 },
            LogoUrl = "http://127.0.0.1:10000/devstoreaccount1/beersapilogourls/leffe_brun_logo.jpg",
            TiwooRating = 4.5
         };


         Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
         var createBeerResponse = await Client.PostAsJsonAsync("beers", beerToCreate).ConfigureAwait(false);
         var beerCreated = await createBeerResponse.BodyAs<Beer>();
         var beerId = beerCreated.Id;

         _createComment = new CreateComment
         {
            BeerId = beerId,
            Body = "This is an integration test",
            UserFirstName = "username",
            UserUId = Guid.Parse("5fd4d406-3161-4bc6-a4c3-5672b3d205a9")
         };
      }

      #region Tests

      [Test]
      public async Task AddComment_NoTokenProvided_ReturnsUnauthorized401()
      {
         //act
         _token = string.Empty;
         var response = await Exec(_token);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
      }

      [Test]
      public async Task AddComment_NoEmailClaim_ReturnsBadRequest400()
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
         var response = await Exec(_token).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Test]
      public async Task AddComment_NoExistingBeer_ReturnsNotFound404()
      {
         //arrange
         _createComment.BeerId = 9999;

         //act
         var response = await Exec(_token).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
      }

      [Test]
      public async Task AddComment_NoExistingUser_ReturnsNotFound404()
      {
         //arrange
         _createComment.UserUId = Guid.NewGuid();

         //act
         var response = await Exec(_token).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
      }

      [Test]
      public async Task AddComment_ValidData_ReturnsBeerWithAddedComment()
      {
         //arrange
         var beerToCreate = new CreateBeer
         {
            Name = "AnotherValidBeerToCreate",
            Description = "AnotherBeerToCreateDescription",
            AlcoholLevel = 6.5,
            CategoryId = 1,
            ColorId = 1,
            CountryId = 1,
            FlavourIds = new List<int> { 1, 2 },
            LogoUrl = "http://127.0.0.1:10000/devstoreaccount1/beersapilogourls/leffe_brun_logo.jpg",
            TiwooRating = 4.5
         };

         Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
         var createBeerResponse = await Client.PostAsJsonAsync("beers", beerToCreate).ConfigureAwait(false);
         var beerCreated = await createBeerResponse.BodyAs<Beer>();
         _createComment.BeerId = beerCreated.Id;

         //act
         var response = await Exec(_token).ConfigureAwait(false);
         var beer = await response.BodyAs<Beer>();

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.OK);
         beer.Comments.Count().Should().Be(1);
      }

      #endregion

      #region Private Methods

      private async Task<HttpResponseMessage> Exec(string token)
      {
         Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
         return await Client.PostAsJsonAsync("beers/comment", _createComment).ConfigureAwait(false);
      }

      #endregion
   }
}
