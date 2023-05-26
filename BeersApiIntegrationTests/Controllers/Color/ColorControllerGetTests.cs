using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using DataAccess;
using Xunit;
using System.Threading.Tasks;
using BeersApi.IntegrationTests.Extensions;
using BeersApi.IntegrationTests.Helpers;
using FluentAssertions;

namespace BeersApi.IntegrationTests.Controllers.Color
{
   public class ColorControllerGetTests : IClassFixture<CustomWebApplicationFactory<Startup>>, IClassFixture<DbContextFactory>, IDisposable
   {
      private readonly CustomWebApplicationFactory<Startup> _customWebApplicationFactory;
      private readonly BeersApiContext _beersApiContext;

      public ColorControllerGetTests(CustomWebApplicationFactory<Startup> customWebApplicationFactory, DbContextFactory contextFactory)
      {
         _customWebApplicationFactory = customWebApplicationFactory;
         _beersApiContext = contextFactory.Context;
      }

      [Fact]
      public async Task GetColors_ReturnsAllColors()
      {
         //act
         var response = await _customWebApplicationFactory.Get("/colors");
         var body = await response.BodyAs<IEnumerable<BeersApi.Models.Output.Colors.Color>>();

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.OK);
         body.Count().Should().Be(2);
      }

      public void Dispose()
      {
         _beersApiContext.Colors.RemoveRange(_beersApiContext.Colors);
         _beersApiContext.SaveChanges();
      }
   }
}
