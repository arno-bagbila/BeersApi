using BeersApi.NUnit.FeatureTests.Extensions;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Net;
using System.Threading.Tasks;

namespace BeersApi.NUnit.FeatureTests.Flavours.Tests
{
   [TestFixture]
   public class GetFlavours : NUnitFeatureTestBase
   {
      [Test]
      public async Task GetFlavour_WithWrongId_ReturnsNotFound()
      {
         //act
         var response = await Client.GetAsync("flavours/9999").ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
      }

      [Test]
      public async Task GetFlavour_IdWrongFormat_ReturnsBadRequest()
      {
         //act
         var response = await Client.GetAsync($"/flavours/{Guid.NewGuid()}").ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Test]
      public async Task GetFlavour_ReturnsOkAndFlavour()
      {
         //act
         var response = await Client.GetAsync("flavours/1").ConfigureAwait(false);
         var flavour = await response.BodyAs<Models.Output.Flavours.Flavour>();

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.OK);
         flavour.Id.Should().Be(1);
         flavour.Name.Should().Be("Acidity");
      }
   }
}
