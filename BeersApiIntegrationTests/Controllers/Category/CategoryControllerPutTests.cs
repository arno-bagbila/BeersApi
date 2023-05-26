using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using BeersApi.IntegrationTests.Extensions;
using BeersApi.IntegrationTests.Helpers;
using BeersApi.Models.Input.Categories.Update;
using DataAccess;
using FluentAssertions;
using IdentityModel;
using Xunit;

namespace BeersApi.IntegrationTests.Controllers.Category
{
   public class CategoryControllerPutTests : IClassFixture<CustomWebApplicationFactory<Startup>>, IClassFixture<DbContextFactory>, IDisposable
   {
      private readonly CustomWebApplicationFactory<Startup> _customWebApplicationFactory;
      private readonly BeersApiContext _beersApiContext;
      private UpdateCategory _updateCategory;
      private readonly string _token;
      private int _id;
      private const string UpdateValidCategoryName = "Update Category Name";
      private const string UpdateValidCategoryDescription = "Update Category Description";

      public CategoryControllerPutTests(CustomWebApplicationFactory<Startup> customWebApplicationFactory, DbContextFactory contextFactory)
      {
         _beersApiContext = contextFactory.Context;
         _customWebApplicationFactory = customWebApplicationFactory;
         _updateCategory = new UpdateCategory
         { Name = UpdateValidCategoryName, Description = UpdateValidCategoryDescription};

         var category = Domain.Entities.Category.Create("Category Name", "Category Description");
         _beersApiContext.Add(category);
         _beersApiContext.SaveChanges();
         _id = category.Id;

         var claims = new List<Claim>
         {
            new Claim(JwtClaimTypes.GivenName, "testUser"),
            new Claim(JwtClaimTypes.Role, "admin")
         };

         _token = _customWebApplicationFactory.Jwt.GenerateToken(claims);
      }

      private Task<HttpResponseMessage> Exec(string token, int id) =>
         _customWebApplicationFactory.AddAuth(token).Put($"/categories/{id}", _updateCategory);

      [Fact]
      public async Task UpdateCategory_UserNotAuthenticate_ReturnsUnauthorized401_()
      {
         //arrange
         var token = string.Empty;
         //act
         var response = await Exec(token, _id).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
      }

      [Fact]
      public async Task UpdateCategory_UserNotAdmin_ReturnsForbidden403_()
      {
         //arrange
         var claims = new List<Claim>
         {
            new Claim(JwtClaimTypes.GivenName, "testUser"),
            new Claim(JwtClaimTypes.Role, "User")
         };
     
         var token = _customWebApplicationFactory.Jwt.GenerateToken(claims);

         //act
         var response = await Exec(token, _id).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
      }

      [Fact]
      public async Task UpdateCategory_NameNotProvided_ReturnsBadRequest_I()
      {
         //arrange
         _updateCategory = new UpdateCategory { Description = UpdateValidCategoryDescription};

         //act
         var response = await Exec(_token, _id).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task UpdateCategory_NameEmpty_ReturnsBadRequest()
      {
         //arrange
         _updateCategory.Name = string.Empty;

         //act
         var response = await Exec(_token, _id).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task UpdateCategory_NameLengthLessThan3Characters_ReturnsBadRequest()
      {
         //arrange
         _updateCategory.Name = "12";

         //act
         var response = await Exec(_token, _id).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task UpdateCategory_NameLengthMoreThan50Characters_ReturnsBadRequest()
      {
         //arrange
         _updateCategory.Name = new string('a', 51);

         //act
         var response = await Exec(_token, _id).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task UpdateCategory_DescriptionNotProvided_ReturnsBadRequest()
      {
         //arrange
         _updateCategory = new UpdateCategory {Name = UpdateValidCategoryName};

         //act
         var response = await Exec(_token, _id).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task UpdateCategory_DescriptionEmpty_ReturnsBadRequest()
      {
         //arrange
         _updateCategory.Description = string.Empty;

         //act
         var response = await Exec(_token, _id).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task UpdateCategory_DescriptionLengthLessThan3Characters_ReturnsBadRequest()
      {
         //arrange
         _updateCategory.Description = "12";

         //act
         var response = await Exec(_token, _id).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task UpdateCategory_DescriptionLengthMoreThan3000Characters_ReturnsBadRequest()
      {
         //arrange
         _updateCategory.Description = new string('a', 3001);

         //act
         var response = await Exec(_token, _id).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task UpdateCategory_NameAlreadyExistForAnotherCategory_ReturnsBadRequest()
      {
         //arrange
         _beersApiContext.Add(Domain.Entities.Category.Create("Another Category Name", "Another Category Description"));
         await _beersApiContext.SaveChangesAsync();
         _updateCategory.Name = "Another Category Name";

         //act
         var response = await Exec(_token, _id).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task UpdateCategory_NoExistingCategory_ReturnsNotFound()
      {
         //arrange
         _id = 20;

         //act
         var response = await Exec(_token, _id).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
      }

      [Fact]
      public async Task UpdateCategory_ValidInput_ReturnsUpdatedCategory()
      {
         //act
         var response = await Exec(_token, _id).ConfigureAwait(false);
         var categoryModel = await response.BodyAs<Models.Output.Categories.Category>();

         //assert
         categoryModel.Name.Should().Be(_updateCategory.Name);
         categoryModel.Description.Should().Be(_updateCategory.Description);
      }

      public void Dispose()
      {
         _beersApiContext.Categories.RemoveRange(_beersApiContext.Categories);
         _beersApiContext.SaveChanges();
      }
   }
}
