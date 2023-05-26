using BeersApi.NUnit.FeatureTests.Extensions;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BeersApi.NUnit.FeatureTests.Beers.Tests
{

   [TestFixture]
   public class ListBeers : NUnitFeatureTestBase
   {
      [Test]
      public async Task ListBeers_ReturnsAllBeers()
      {
         //act
         var response = await Client.GetAsync("beers");
         var beers = await response.BodyAs<IEnumerable<Models.Output.Beers.Beer>>();

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.OK);
         beers.Count().Should().BeGreaterOrEqualTo(1);
      }

      [Test]
      public async Task ListBeers_FilterByExistingCategory_ReturnsCategoryBeers()
      {
         //act

         var response = await Client.GetAsync("beers?CategoryIds=1");
         var beers = await response.BodyAs<IEnumerable<Models.Output.Beers.Beer>>();

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.OK);
         beers.Count().Should().BeGreaterOrEqualTo(1);
      }

      [Test]
      public async Task ListBeers_FilterByNonExistingCategory_ReturnsEmptyBeersList()
      {
         //act

         var response = await Client.GetAsync("beers?CategoryIds=9999");
         var beers = await response.BodyAs<IEnumerable<Models.Output.Beers.Beer>>();

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.OK);
         beers.Count().Should().Be(0);
      }

      [Test]
      public async Task ListBeers_FilterByExistingColor_ReturnsColorBeers()
      {
         //act

         var response = await Client.GetAsync("beers?ColorIds=1");
         var beers = await response.BodyAs<IEnumerable<Models.Output.Beers.Beer>>();

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.OK);
         beers.Count().Should().BeGreaterOrEqualTo(1);
      }

      [Test]
      public async Task ListBeers_FilterByNonExistingColor_ReturnsEmptyBeersList()
      {
         //act

         var response = await Client.GetAsync("beers?ColorIds=9999");
         var beers = await response.BodyAs<IEnumerable<Models.Output.Beers.Beer>>();

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.OK);
         beers.Count().Should().Be(0);
      }

      [Test]
      public async Task ListBeers_FilterByExistingCountry_ReturnsCountryBeers()
      {
         //act

         var response = await Client.GetAsync("beers?CountryIds=1");
         var beers = await response.BodyAs<IEnumerable<Models.Output.Beers.Beer>>();

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.OK);
         beers.Count().Should().BeGreaterOrEqualTo(1);
      }

      [Test]
      public async Task ListBeers_FilterByNonExistingCountry_ReturnsEmptyBeersList()
      {
         //act

         var response = await Client.GetAsync("beers?CountryIds=9999");
         var beers = await response.BodyAs<IEnumerable<Models.Output.Beers.Beer>>();

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.OK);
         beers.Count().Should().Be(0);
      }

   }
}
