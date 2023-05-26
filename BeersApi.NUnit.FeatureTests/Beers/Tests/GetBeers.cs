using BeersApi.NUnit.FeatureTests.Extensions;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BeersApi.NUnit.FeatureTests.Beers.Tests
{

   [TestFixture]
   public class GetBeers : NUnitFeatureTestBase
   {
      [Test]
      public async Task GetBeer_WithWrongId_ReturnsNotFound()
      {
         //act
         var response = await Client.GetAsync("beers/999").ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
      }

      [Test]
      public async Task GetBeer_IdWrongFormat_ReturnsNotFound()
      {
         //act
         var response = await Client.GetAsync($"/beers/{Guid.NewGuid()}").ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
      }

      [Test]
      public async Task GetBeer_ReturnsBeer()
      {
         //act
         var response = await Client.GetAsync("beers/1").ConfigureAwait(false);
         var beer = await response.BodyAs<Models.Output.Beers.Beer>();

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.OK);
         beer.Id.Should().Be(1);
         beer.Name.Should().Be("BeerTest");
         beer.Description.Should().Be("BeerTest description");
         beer.Color.Name.Should().Be("Black");
         beer.Category.Name.Should().Be("Abbey (Abbaye, Abdji)");
         beer.Country.Name.Should().Be("Belgium");
         beer.Flavours.First().Name.Should().Be("Acidity");
      }
   }
}
