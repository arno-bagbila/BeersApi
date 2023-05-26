using System;
using System.Net;
using System.Net.Http;
using DataAccess;
using Xunit;
using System.Threading.Tasks;
using BeersApi.IntegrationTests.Extensions;
using BeersApi.IntegrationTests.Helpers;
using FluentAssertions;

namespace BeersApi.IntegrationTests.Controllers.Category
{
   public class CategoryControllerGetByIdTests : IClassFixture<CustomWebApplicationFactory<Startup>>, IClassFixture<DbContextFactory>, IDisposable
   {
      private readonly CustomWebApplicationFactory<Startup> _customWebApplicationFactory;
      private readonly BeersApiContext _beersApiContext;
      private int _id;
      private const string ValidCategoryName = "Valid Category Name";
      private const string ValidCategoryDescription = "Valid Category Description";

      public CategoryControllerGetByIdTests(CustomWebApplicationFactory<Startup> customWebApplicationFactory, DbContextFactory contextFactory)
      {
         _customWebApplicationFactory = customWebApplicationFactory;
         _beersApiContext = contextFactory.Context;

         var category = Domain.Entities.Category.Create(ValidCategoryName, ValidCategoryDescription);
         _beersApiContext.Add(category);
         _beersApiContext.SaveChanges();

         _id = category.Id;
      }

      private Task<HttpResponseMessage> Exec() =>
         _customWebApplicationFactory.Get($"/categories/{_id}");

      [Fact]
      public async Task GetCategory_WrongId_ReturnsNotFound()
      {
         //arrange
         _id = 0;

         //act
         var response = await Exec().ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
      }

      [Fact]
      public async Task GetCategory_IdWrongFormat_ReturnsBadRequest()
      {
         //act
         var response = await _customWebApplicationFactory.Get($"/categories/{Guid.NewGuid()}").ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task GetCategory_ValidId_ReturnsCategory()
      {
         //act
         var response = await Exec().ConfigureAwait(false);
         var body = await response.BodyAs<BeersApi.Models.Output.Categories.Category>();

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.OK);
         body.Name.Should().Be(ValidCategoryName);
         body.Description.Should().Be(ValidCategoryDescription);
      }

      public void Dispose()
      {
         _beersApiContext.Categories.RemoveRange(_beersApiContext.Categories);
         _beersApiContext.SaveChanges();
      }
   }
}
