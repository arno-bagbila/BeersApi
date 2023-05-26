using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using BeersApi.IntegrationTests.Extensions;
using BeersApi.IntegrationTests.Helpers;
using BeersApi.Models.Input.Categories.Create;
using DataAccess;
using FluentAssertions;
using IdentityModel;
using Xunit;

namespace BeersApi.IntegrationTests.Controllers.Category
{
   public class CategoryControllerPostTests : IClassFixture<CustomWebApplicationFactory<Startup>>, IClassFixture<DbContextFactory>, IDisposable
   {
      private readonly CustomWebApplicationFactory<Startup> _customWebApplicationFactory;
      private readonly BeersApiContext _beersApiContext;
      private string _token;
      private CreateCategory _createCategory;
      private const string CategoryName = "Category Name";
      private const string CategoryDescription = "Category Description";

      public CategoryControllerPostTests(CustomWebApplicationFactory<Startup> customWebApplicationFactory, DbContextFactory contextFactory)
      {
         _beersApiContext = contextFactory.Context;
         _customWebApplicationFactory = customWebApplicationFactory;

         _createCategory = new CreateCategory { Name = CategoryName, Description = CategoryDescription };

         var claims = new List<Claim>
         {
            new Claim(JwtClaimTypes.GivenName, "testUser"),
            new Claim(JwtClaimTypes.Role, "admin")
         };

         _token = _customWebApplicationFactory.Jwt.GenerateToken(claims);
      }

      private Task<HttpResponseMessage> Exec() => _customWebApplicationFactory.AddAuth(_token).Post("/categories", _createCategory);

      [Fact]
      public async Task CreateCategory_NoTokenProvided_ReturnUnauthorized401()
      {
         //arrange
         _token = "";

         //act
         var response = await Exec();

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
      }

      [Fact]
      public async Task CreateCategory_NotAdmin_ReturnUnauthorized403()
      {
         //arrange
         var claims = new List<Claim>
         {
            new Claim(JwtClaimTypes.GivenName, "testUser"),
            new Claim(JwtClaimTypes.Role, "User")
         };

         _token = _customWebApplicationFactory.Jwt.GenerateToken(claims);

         //act
         var response = await Exec();

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
      }

      [Fact]
      public async Task CreateCategory_EmptyName_ReturnsBadRequest()
      {
         //arrange
         _createCategory.Name = string.Empty;

         //act
         var response = await Exec().ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task CreateCategory_NameNotProvided_ReturnsBadRequest() 
      {
         //arrange
         _createCategory = new CreateCategory { Description = CategoryDescription };

         //act
         var response = await Exec().ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task CreateCategory_NameLessThan3Characters_ReturnsBadRequest() 
      {
         //arrange
         _createCategory.Name = "12";

         //act
         var response = await Exec().ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task CreateCategory_NameMoreThan50Characters() 
      {
         //arrange
         _createCategory.Name = new string('a', 51);

         //act
         var response = await Exec().ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task CreateCategory_DescriptionNotProvided_ReturnsBadRequest()
      {
         //arrange
         _createCategory = new CreateCategory { Name = CategoryName };

         //act
         var response = await Exec().ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task CreateCategory_EmptyDescription_ReturnsBadRequest()
      {
         //arrange
         _createCategory = new CreateCategory { Name = CategoryName, Description = string.Empty };

         //act
         var response = await Exec().ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task CreateCategory_DescriptionLessThan3Characters_ReturnBadRequest()
      {
         //arrange
         _createCategory.Description = "12";

         //act
         var response = await Exec().ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task CreateCategory_DescriptionMoreThan3000Characters_ReturnsBadRequest()
      {
         //arrange
         _createCategory.Name = new string('a', 3001);

         //act
         var response = await Exec().ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task CreateCategory_CategoryWithSameNameAlreadyExists_ReturnsBadRequest()
      {
         //arrange
         var category = Domain.Entities.Category.Create("Category Name", "Category Description");
         _beersApiContext.Add(category);
         await _beersApiContext.SaveChangesAsync();

         //act
         var response = await Exec().ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task CreateCategory_ValidCreateCategoryAndValidToken_SaveCategory()
      {
         //act
         await Exec();
         var categoryInDb = _beersApiContext.Categories.FirstOrDefault(c => c.Name == _createCategory.Name);

         //assert
         categoryInDb.Should().NotBeNull();
      }

      [Fact]
      public async Task CreateCategory_ValidCreateCategoryAndValidToken_ReturnsCreatedCategory()
      {
         //act
         var response = await Exec();
         var category = await response.BodyAs<Models.Output.Categories.Category>();

         //assert
         category.UId.Should().NotBeEmpty();
         category.Name.Should().Be(_createCategory.Name, "Is the same name sended on Exec();");
         category.Description.Should().Be(_createCategory.Description, "Is the same description sended on Exec();");
      }

      public void Dispose()
      {
         _beersApiContext.Categories.RemoveRange(_beersApiContext.Categories);
         _beersApiContext.SaveChanges();
      }
   }
}
