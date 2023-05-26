using BeersApi.NUnit.FeatureTests.Extensions;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BeersApi.NUnit.FeatureTests.Flavours.Tests
{
   [TestFixture]
   public class ListFlavours : NUnitFeatureTestBase
   {
      [Test]
      public async Task ListFlavours_ReturnsAllFlavours()
      {
         //act
         var response = await Client.GetAsync("flavours");
         var countries = await response.BodyAs<IEnumerable<Models.Output.Flavours.Flavour>>();

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.OK);
         countries.Count().Should().BeGreaterOrEqualTo(2);
      }
   }
}
