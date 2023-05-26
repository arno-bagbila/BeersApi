using BeersApi.NUnit.FeatureTests.Extensions;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BeersApi.NUnit.FeatureTests.Categories.CategoryTests
{
   [TestFixture]
   public class ListCategories : NUnitFeatureTestBase
   {
      [Test]
      public async Task ListCategories_ReturnsAllCategories()
      {
         //act
         var response = await Client.GetAsync("categories");
         var body = await response.BodyAs<IEnumerable<Models.Output.Categories.Category>>();

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.OK);
         body.Count().Should().BeGreaterOrEqualTo(2);
      }
   }
}
