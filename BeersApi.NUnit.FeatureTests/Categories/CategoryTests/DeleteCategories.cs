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
   public class DeleteCategories : NUnitFeatureTestBase
   {
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

      }

      [Test]
      public async Task DeleteCategory_NoTokenProvided_ReturnsUnauthorized401()
      {
         //act
         _token = string.Empty;
         var response = await Exec(_token, _categoryId);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
      }

      [Test]
      public async Task DeleteCategory_NoEmailClaim_ReturnsBadRequest400()
      {
         //arrange
         var claims = new List<Claim>
         {
            new Claim(JwtClaimTypes.GivenName, "testUser"),
            new Claim(JwtClaimTypes.Role, "admin")
         };

         _token = TestEnvAuthentication.GenerateToken(claims);

         //act
         var response = await Exec(_token, _categoryId).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Test]
      public async Task DeleteCategory_NoBeersApiAdmin_ReturnsForbidden403()
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
      public async Task DeleteCategory_NoExistingCategory_ReturnsNotFound404()
      {
         //arrange
         _categoryId = 99999;

         //act
         var response = await Exec(_token, _categoryId).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
      }

      [Test]
      public async Task DeleteCategory_ExistingCategory_ReturnedDeletedCategory()
      {
         //arrange
         Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
         var createCategoryResponse = await Client.PostAsJsonAsync("categories", new CreateCategory { Name = "CategoryToDelete", Description = "CategoryToDeleteDescription" }).ConfigureAwait(false);
         var categoryToDelete = await createCategoryResponse.BodyAs<Models.Output.Categories.Category>();
         _categoryId = categoryToDelete.Id;

         //act
         var response = await Exec(_token, _categoryId).ConfigureAwait(false);
         var category = await response.BodyAs<Models.Output.Categories.Category>();

         //assert
         category.Should().NotBeNull();
         category.UId.Should().NotBeEmpty();
         category.Name.Should().Be("CategoryToDelete", "It is the name of the deleted category");
         category.Description.Should().Be("CategoryToDeleteDescription", "It is the description of the deleted category");
         category.Id.Should().Be(_categoryId);
      }

      private async Task<HttpResponseMessage> Exec(string token, int categoryId)
      {
         Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
         return await Client.DeleteAsync($"categories/{categoryId}").ConfigureAwait(false);
      }
   }
}
