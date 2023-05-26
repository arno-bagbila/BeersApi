using BeersApi.NUnit.FeatureTests.Extensions;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Net;
using System.Threading.Tasks;

namespace BeersApi.NUnit.FeatureTests.Countries.CountryTests
{
   [TestFixture]
   public class GetCountries : NUnitFeatureTestBase
   {
      [Test]
      public async Task GetCountry_WithWrongId_ReturnsNotFound()
      {
         //act
         var response = await Client.GetAsync("countries/9999").ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
      }

      [Test]
      public async Task GetCountry_IdWrongFormat_ReturnsNotFound()
      {
         //act
         var response = await Client.GetAsync($"/countries/{Guid.NewGuid()}").ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
      }

      [Test]
      public async Task GetCountry_ReturnsOkAndCountry()
      {
         //act
         var response = await Client.GetAsync("countries/1").ConfigureAwait(false);
         var country = await response.BodyAs<Models.Output.Countries.Country>();

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.OK);
         country.Id.Should().Be(1);
         country.Name.Should().Be("Belgium");
         country.Code.Should().Be("be");
      }
   }
}
