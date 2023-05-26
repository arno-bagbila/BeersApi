using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BeersApi.IntegrationTests.Extensions;
using BeersApi.IntegrationTests.Helpers;
using DataAccess;
using FluentAssertions;
using Xunit;

namespace BeersApi.IntegrationTests.Controllers.Flavour
{
   public class FlavourControllerGetByIdTests : IClassFixture<CustomWebApplicationFactory<Startup>>, IClassFixture<DbContextFactory>, IDisposable
   {
      private readonly CustomWebApplicationFactory<Startup> _customWebApplicationFactory;
      private readonly BeersApiContext _beersApiContext;
      private const string ValidFlavourName = "Valid flavour Name";
      private const string ValidFlavourDescription = "Valid flavour Description";
      private int _id;

      public FlavourControllerGetByIdTests(CustomWebApplicationFactory<Startup> customWebApplicationFactory, DbContextFactory contextFactory)
      {
         _customWebApplicationFactory = customWebApplicationFactory;
         _beersApiContext = contextFactory.Context;



         var flavour = Domain.Entities.Flavour.Create(ValidFlavourName, ValidFlavourDescription);
         _beersApiContext.Add(flavour);
         _beersApiContext.SaveChanges();

         _id = flavour.Id;
      }

      private Task<HttpResponseMessage> Exec() => _customWebApplicationFactory.Get($"/flavours/{_id}");

      [Fact]
      public async Task GetFlavour_WrongId_ReturnsNotFound()
      {
         //arrange
         _id = 1000;

         //act
         var response = await Exec().ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
      }

      [Fact]
      public async Task GetFlavour_WrongIdFormat_ReturnsBadRequest()
      {
         //act
         var response = await _customWebApplicationFactory.Get($"/flavours/{Guid.NewGuid()}").ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task GetFlavour_ReturnFlavour()
      {
         //act
         var response = await Exec().ConfigureAwait(false);
         var body = await response.BodyAs<Models.Output.Flavours.Flavour>();

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.OK);
         body.Name.Should().Be(ValidFlavourName);
         body.Description.Should().Be(ValidFlavourDescription);
      }


      public void Dispose()
      {
         _beersApiContext.Flavours.RemoveRange(_beersApiContext.Flavours);
         _beersApiContext.SaveChanges();
      }
   }
}
