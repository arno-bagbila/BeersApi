using BeersApi.NUnit.FeatureTests.Extensions;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BeersApi.NUnit.FeatureTests.Colors.ColorTests
{
   [TestFixture]
   public class ListColors : NUnitFeatureTestBase
   {
      [Test]
      public async Task ListColors_ReturnsAllColors()
      {
         //act
         var response = await Client.GetAsync("colors");
         var colors = await response.BodyAs<IEnumerable<Models.Output.Colors.Color>>();

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.OK);
         colors.Count().Should().Be(2);
      }
   }
}
