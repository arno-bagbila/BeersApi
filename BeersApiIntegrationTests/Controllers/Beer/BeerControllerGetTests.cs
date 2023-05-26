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

namespace BeersApi.IntegrationTests.Controllers.Beer
{
   public class BeerControllerGetTests : IClassFixture<CustomWebApplicationFactory<Startup>>, IClassFixture<DbContextFactory>, IDisposable
   {
      private readonly CustomWebApplicationFactory<Startup> _customWebApplicationFactory;
      private readonly BeersApiContext _beersApiContext;

      public BeerControllerGetTests(CustomWebApplicationFactory<Startup> customWebApplicationFactory, DbContextFactory contextFactory)
      {
         _beersApiContext = contextFactory.Context;
         _customWebApplicationFactory = customWebApplicationFactory;
      }

      [Fact]
      public async Task ListBeers_ReturnAllBeers()
      {
         //act
         var response = await _customWebApplicationFactory.Get("/beers");
         var beers = await response.BodyAs<IEnumerable<Models.Output.Beers.Beer>>();

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.OK);
         beers.Count().Should().BeGreaterOrEqualTo(2);
      }


      public void Dispose()
      {
         _beersApiContext.Beers.RemoveRange(_beersApiContext.Beers);
         _beersApiContext.Categories.RemoveRange(_beersApiContext.Categories);
         _beersApiContext.SaveChanges();
      }

   }
}
