using BeersApi.NUnit.FeatureTests.Extensions;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Net;
using System.Threading.Tasks;

namespace BeersApi.NUnit.FeatureTests.Colors.ColorTests
{
   [TestFixture]
   public class GetColors : NUnitFeatureTestBase
   {
      [Test]
      public async Task GetColor_WithWrongId_ReturnsNotFound()
      {
         //act
         var response = await Client.GetAsync("colors/9999").ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
      }

      [Test]
      public async Task GetColor_IdWrongFormat_ReturnsNotFound()
      {
         //act
         var response = await Client.GetAsync($"/colors/{Guid.NewGuid()}").ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
      }

      [Test]
      public async Task GetColor_ReturnsOkAndColor()
      {
         //act
         var response = await Client.GetAsync("colors/1").ConfigureAwait(false);
         var color = await response.BodyAs<Models.Output.Colors.Color>();

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.OK);
         color.Id.Should().Be(1);
         color.Name.Should().Be("Black");
      }
   }
}
