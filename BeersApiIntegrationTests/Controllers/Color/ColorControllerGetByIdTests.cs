using System;
using System.Net;
using System.Threading.Tasks;
using BeersApi.IntegrationTests.Extensions;
using BeersApi.IntegrationTests.Helpers;
using DataAccess;
using FluentAssertions;
using Xunit;

namespace BeersApi.IntegrationTests.Controllers.Color
{
   public class ColorControllerGetByIdTests : IClassFixture<CustomWebApplicationFactory<Startup>>, IClassFixture<DbContextFactory>, IDisposable
   {
      private readonly CustomWebApplicationFactory<Startup> _customWebApplicationFactory;
      private readonly BeersApiContext _beersApiContext;

      public ColorControllerGetByIdTests(CustomWebApplicationFactory<Startup> customWebApplicationFactory, DbContextFactory contextFactory)
      {
         _customWebApplicationFactory = customWebApplicationFactory;
         _beersApiContext = contextFactory.Context;
      }
      public void Dispose()
      {
         _beersApiContext.Colors.RemoveRange(_beersApiContext.Colors);
         _beersApiContext.SaveChanges();
      }

      [Fact]
      public async Task GetColor_ValidId_ReturnsColor()
      {
         //arrange
         var color = Domain.Entities.Color.Create("Color 1");
         await _beersApiContext.Colors.AddAsync(color);
         await _beersApiContext.SaveChangesAsync();

         //act
         var response = await _customWebApplicationFactory.Get($"/colors/{color.Id}");
         var body = await response.BodyAs<BeersApi.Models.Output.Colors.Color>();

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.OK);
         body.Name.Should().Be(color.Name);
      }

      [Fact]
      public async Task GetColor_WrongId_ReturnsNotFound()
      {
         //act
         var response = await _customWebApplicationFactory.Get("/colors/0").ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
      }

      [Fact]
      public async Task GetColor_WrongIdFormat_ReturnsBadRequest()
      {
         //act
         var response = await _customWebApplicationFactory.Get($"/colors/{Guid.NewGuid()}");

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }
   }
}
