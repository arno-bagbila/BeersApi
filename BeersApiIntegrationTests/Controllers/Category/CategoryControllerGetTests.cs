using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BeersApi.IntegrationTests.Extensions;
using BeersApi.IntegrationTests.Helpers;
using DataAccess;
using FluentAssertions;
using Xunit;

namespace BeersApi.IntegrationTests.Controllers.Category
{
   public class CategoryControllerGetTests : IClassFixture<CustomWebApplicationFactory<Startup>>, IClassFixture<DbContextFactory>, IDisposable
   {
      private readonly CustomWebApplicationFactory<Startup> _customWebApplicationFactory;
      private readonly BeersApiContext _beersApiContext;
      private readonly string _token;

      public CategoryControllerGetTests(CustomWebApplicationFactory<Startup> customWebApplicationFactory, DbContextFactory contextFactory)
      {
         _customWebApplicationFactory = customWebApplicationFactory;
         _beersApiContext = contextFactory.Context;
      }

      [Fact]
      public async Task GetCategories_ReturnsAllCategories()
      {
         //act
         var response = await _customWebApplicationFactory.AddAuth(_token).Get("/categories");
         var body = await response.BodyAs<IEnumerable<Models.Output.Categories.Category>>();

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.OK);
         body.Count().Should().Be(2);
      }

      public void Dispose()
      {
         _beersApiContext.Categories.RemoveRange(_beersApiContext.Categories);
         _beersApiContext.SaveChanges();
      }
   }
}
