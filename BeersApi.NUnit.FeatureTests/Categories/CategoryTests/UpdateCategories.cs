using BeersApi.Models.Input.Categories.Update;
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
   public class UpdateCategories : NUnitFeatureTestBase
   {
      private UpdateCategory _updateCategory;
      private const string UpdateValidCategoryName = "Update Category Name";
      private const string UpdateValidCategoryDescription = "Update Category Description";
      private string _token;
      private int _categoryId;


      [SetUp]
      public void BeforeEachTest()
      {
         var claims = new List<Claim>
         {
            new Claim(ClaimTypes.Email, "test@email.com")
         };
         _token = TestEnvAuthentication.GenerateToken(claims);
         _updateCategory = new UpdateCategory { Name = UpdateValidCategoryName, Description = UpdateValidCategoryDescription };
         _categoryId = 1;
      }

      [Test]
      public async Task UpdateCategory_NoTokenProvided_ReturnsUnauthorized401()
      {
         //act
         _token = string.Empty;
         var response = await Exec(_token, _categoryId);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
      }

      [Test]
      public async Task UpdateCategory_NoEmailClaim_ReturnsBadRequest400()
      {
         //arrange
         var claims = new List<Claim>
         {
            new Claim(JwtClaimTypes.GivenName, "testUser"),
            new Claim(JwtClaimTypes.Role, "User"),
            new Claim(ClaimTypes.NameIdentifier, "test-wrong")
         };

         _token = TestEnvAuthentication.GenerateToken(claims);

         //act
         var response = await Exec(_token, _categoryId).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Test]
      public async Task UpdateCategory_NoBeersApiAdmin_ReturnsForbidden403()
      {
         //arrange
         var claims = new List<Claim>
         {
            new Claim(ClaimTypes.Email, "wrongemail@test.com")
         };

         _token = TestEnvAuthentication.GenerateToken(claims);

         //act
         var response = await Exec(_token, _categoryId).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
      }

      [Test]
      public async Task UpdateCategory_CategoryWithSameNameAlreadyExists_ReturnsBadRequest400()
      {
         //arrange
         _updateCategory.Name = "Altbier";

         //act
         var response = await Exec(_token, _categoryId).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Test]
      public async Task UpdateCategory_NoExistingCategory_ReturnsNotFound404()
      {
         //arrange
         _categoryId = 9999;

         //act
         var response = await Exec(_token, _categoryId).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
      }

      [Test]
      public async Task UpdateCategory__ValidInput_ReturnsOkAndUpdatedCategory()
      {
         //act
         var response = await Exec(_token, _categoryId).ConfigureAwait(false);
         var category = await response.BodyAs<Models.Output.Categories.Category>();

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.OK);
         category.UId.Should().NotBeEmpty();
         category.Id.Should().Be(_categoryId);
         category.Name.Should().Be(_updateCategory.Name, "Is the same name sended on Exec();");
         category.Description.Should().Be(_updateCategory.Description, "Is the same description sended on Exec();");
      }

      private async Task<HttpResponseMessage> Exec(string token, int categoryId)
      {
         Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
         return await Client.PutAsJsonAsync($"categories/{categoryId}", _updateCategory).ConfigureAwait(false);
      }
   }
}
