using System;
using System.Net;
using System.Threading.Tasks;
using BeersApi.IntegrationTests.Extensions;
using BeersApi.IntegrationTests.Helpers;
using DataAccess;
using FluentAssertions;
using Xunit;

namespace BeersApi.IntegrationTests.Controllers.Country
{
   public class CountryControllerGetByIdTests : IClassFixture<CustomWebApplicationFactory<Startup>>, IClassFixture<DbContextFactory>, IDisposable
   {
      private readonly CustomWebApplicationFactory<Startup> _customWebApplicationFactory;
      private readonly BeersApiContext _beersApiContext;

      public CountryControllerGetByIdTests(CustomWebApplicationFactory<Startup> customWebApplicationFactory, DbContextFactory contextFactory)
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
      public async Task GetCountry_ValidId_Returns()
      {
         //arrange
         var country = Domain.Entities.Country.Create("Country 1", "CO");
         await _beersApiContext.Countries.AddAsync(country);
         await _beersApiContext.SaveChangesAsync();

         //act
         var response = await _customWebApplicationFactory.Get($"/countries/{country.Id}");
         var body = await response.BodyAs<BeersApi.Models.Output.Countries.Country>();

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.OK);
         body.Name.Should().Be(country.Name);
         body.Code.Should().Be(country.Code);
      }

      [Fact]
      public async Task GetCountry_WrongId_ReturnsNotFound()
      {
         //act
         var response = await _customWebApplicationFactory.Get("/countries/0").ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
      }

      [Fact]
      public async Task GetCountry_WrongIdFormat_ReturnBadRequest()
      {
         //act
         var response = await _customWebApplicationFactory.Get($"/countries/{Guid.NewGuid()}");

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }
   }
}
