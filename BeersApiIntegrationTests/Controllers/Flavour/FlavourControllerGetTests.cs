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

namespace BeersApi.IntegrationTests.Controllers.Flavour
{
   public class FlavourControllerGetTests : IClassFixture<CustomWebApplicationFactory<Startup>>, IClassFixture<DbContextFactory>, IDisposable
   {
      private readonly CustomWebApplicationFactory<Startup> _customWebApplicationFactory;
      private readonly BeersApiContext _beersApiContext;

      public FlavourControllerGetTests(CustomWebApplicationFactory<Startup> customWebApplicationFactory, DbContextFactory contextFactory)
      {
         _customWebApplicationFactory = customWebApplicationFactory;
         _beersApiContext = contextFactory.Context;
      }

      [Fact]
      public async Task GetFlavours_ReturnAllFlavours()
      {
         //act
         var response = await _customWebApplicationFactory.Get("/flavours");
         var body = await response.BodyAs<IEnumerable<BeersApi.Models.Output.Flavours.Flavour>>();

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.OK);
         body.Count().Should().Be(2);
      }

      public void Dispose()
      {
         _beersApiContext.Flavours.RemoveRange(_beersApiContext.Flavours);
         _beersApiContext.SaveChanges();
      }
   }
}
