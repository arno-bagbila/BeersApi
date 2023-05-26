using BeersApi.Models.Input.Categories.Create;
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

namespace BeersApi.NUnit.FeatureTests.Categories.CategoryTests
{
   [TestFixture]
   public class CreateCategories : NUnitFeatureTestBase
   {
      private CreateCategory _createCategory;
      private const string CategoryName = "Category Name";
      private const string CategoryDescription = "Category Description";
      private string _token;


      [SetUp]
      public void BeforeEachTest()
      {
         var claims = new List<Claim>
         {
            new Claim(ClaimTypes.Email, "test@email.com")
         };
         _token = TestEnvAuthentication.GenerateToken(claims);
         _createCategory = new CreateCategory { Name = CategoryName, Description = CategoryDescription };
      }

      [Test]
      public async Task CreateCategory_NoTokenProvided_ReturnsUnauthorized401()
      {
         //act
         _token = string.Empty;
         var response = await Exec(_token);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
      }

      [Test]
      public async Task CreateCategory_NoEmailClaim_ReturnsBadRequest400()
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
      public async Task CreateCategory_NoBeersApiAdmin_ReturnsForbidden403()
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
      public async Task CreateCategory_CategoryWithSameNameAlreadyExists_ReturnsBadRequest400()
      {
         //arrange
         _createCategory.Name = "Altbier";

         //act
         var response = await Exec(_token).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Test]
      public async Task CreateCategory_ValidInputWithBeersApiAdmin_ReturnsCreatedCategory201()
      {
         //act
         var response = await Exec(_token).ConfigureAwait(false);
         var category = await response.BodyAs<Models.Output.Categories.Category>();

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.Created);
         category.UId.Should().NotBeEmpty();
         category.Name.Should().Be(_createCategory.Name, "Is the same name sended on Exec();");
         category.Description.Should().Be(_createCategory.Description, "Is the same description sended on Exec();");
      }

      private async Task<HttpResponseMessage> Exec(string token)
      {
         Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
         return await Client.PostAsJsonAsync("categories", _createCategory).ConfigureAwait(false);
      }
   }
}
