using BeersApi.NUnit.FeatureTests.Extensions;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BeersApi.NUnit.FeatureTests.Countries.CountryTests
{
   [TestFixture]
   public class ListCountries : NUnitFeatureTestBase
   {
      [Test]
      public async Task ListCountries_ReturnsAllCountries()
      {
         //act
         var response = await Client.GetAsync("countries");
         var countries = await response.BodyAs<IEnumerable<Models.Output.Countries.Country>>();

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.OK);
         countries.Count().Should().Be(2);
      }
   }
}
