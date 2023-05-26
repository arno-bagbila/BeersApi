using BeersApi.NUnit.FeatureTests.Extensions;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Net;
using System.Threading.Tasks;

namespace BeersApi.NUnit.FeatureTests.Categories.CategoryTests
{
   [TestFixture]
   public class GetCategories : NUnitFeatureTestBase
   {
      [Test]
      public async Task GetCategory_ReturnsCategory()
      {
         //act
         var response = await Client.GetAsync("categories/1").ConfigureAwait(false);
         var category = await response.BodyAs<Models.Output.Categories.Category>();

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.OK);
         category.Id.Should().Be(1);
         category.Name.Should().Be("Abbey (Abbaye, Abdji)");
      }

      [Test]
      public async Task GetCategory_WithWrongId_ReturnsNotFound()
      {
         //act
         var response = await Client.GetAsync("categories/999").ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
      }

      [Test]
      public async Task GetCategory_IdWrongFormat_ReturnsBadRequest()
      {
         //act
         var response = await Client.GetAsync($"/categories/{Guid.NewGuid()}").ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }
   }
}
