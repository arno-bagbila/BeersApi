using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BeersApi;
using BeersApi.IntegrationTests.Extensions;
using BeersApi.IntegrationTests.Helpers;
using DataAccess;
using FluentAssertions;
using Xunit;

namespace BeersApi.IntegrationTests.Controllers.Country
{
   public class CountryControllerGetTests : IClassFixture<CustomWebApplicationFactory<Startup>>, IClassFixture<DbContextFactory>, IDisposable
   {
      private readonly CustomWebApplicationFactory<Startup> _customWebApplicationFactory;
      private readonly BeersApiContext _beersApiContext;

      public CountryControllerGetTests(CustomWebApplicationFactory<Startup> customWebApplicationFactory, DbContextFactory contextFactory)
      {
         _customWebApplicationFactory = customWebApplicationFactory;
         _beersApiContext = contextFactory.Context;
      }

      public void Dispose()
      {
         _beersApiContext.Countries.RemoveRange(_beersApiContext.Countries);
         _beersApiContext.SaveChanges();
      }

      [Fact]
      public async Task GetCountries_ReturnsAllCountries()
      {
         //act
         var response = await _customWebApplicationFactory.Get("/countries");
         var body = await response.BodyAs<IEnumerable<BeersApi.Models.Output.Countries.Country>>();

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.OK);
         body.Count().Should().Be(2);
      }
   }
}
