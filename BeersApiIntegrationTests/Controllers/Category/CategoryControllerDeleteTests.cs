using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using BeersApi.IntegrationTests.Extensions;
using BeersApi.IntegrationTests.Helpers;
using DataAccess;
using FluentAssertions;
using IdentityModel;
using Xunit;

namespace BeersApi.IntegrationTests.Controllers.Category
{
   public class CategoryControllerDeleteTests : IClassFixture<CustomWebApplicationFactory<Startup>>, IClassFixture<DbContextFactory>, IDisposable
   {
      private readonly CustomWebApplicationFactory<Startup> _customWebApplicationFactory;
      private readonly BeersApiContext _beersApiContext;
      private string _token;
      private int _id;
      private const string ValidCategoryName = "Valid Category Name";
      private const string ValidCategoryDescription = "Valid Category Description";

      public CategoryControllerDeleteTests(CustomWebApplicationFactory<Startup> customWebApplicationFactory, DbContextFactory contextFactory)
      {
         _customWebApplicationFactory = customWebApplicationFactory;
         _beersApiContext = contextFactory.Context;

         var category = Domain.Entities.Category.Create(ValidCategoryName, ValidCategoryDescription);
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

      private Task<HttpResponseMessage> Exec() =>
         _customWebApplicationFactory.AddAuth(_token).Delete($"/categories/{_id}");

      [Fact]
      public async Task DeleteCategory_UserNotAuthenticated_ReturnsUnauthorized401()
      {
         //arrange
         _token = string.Empty;
         //act
         var response = await Exec().ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
      }

      [Fact]
      public async Task DeleteCategory_UserNotAdmin_ReturnsForbidden403()
      {
         //arrange
         var claims = new List<Claim>
         {
            new Claim(JwtClaimTypes.GivenName, "testUser"),
            new Claim(JwtClaimTypes.Role, "User")
         };
         var token = _customWebApplicationFactory.Jwt.GenerateToken(claims);

         //act
         var response = await _customWebApplicationFactory.AddAuth(token).Delete($"/categories/{_id}");

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
      }

      [Fact]
      public async Task DeleteCategory_WrongIdFormat_ReturnsBadRequest()
      {
         //act
         var response = await _customWebApplicationFactory.AddAuth(_token).Delete($"/categories/{Guid.NewGuid()}").ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task DeleteCategory_WrongId_ReturnsNotFound()
      {
         //arrange
         _id = 0;

         //act
         var response = await Exec().ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
      }

      [Fact]
      public async Task DeleteCategory_AdminUser_ReturnsDeletedCategory()
      {
         //act
         var response = await Exec().ConfigureAwait(false);
         var categoryModel = await response.BodyAs<Models.Output.Categories.Category>();

         //assert
         categoryModel.Id.Should().Be(_id);
         categoryModel.Name.Should().Be(ValidCategoryName);
         categoryModel.Description.Should().Be(ValidCategoryDescription);
      }
      public void Dispose()
      {
         _beersApiContext.Categories.RemoveRange(_beersApiContext.Categories);
         _beersApiContext.SaveChanges();
      }
   }
}
