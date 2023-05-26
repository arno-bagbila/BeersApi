using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BeersApi.IntegrationTests.Extensions;
using BeersApi.IntegrationTests.Helpers;
using DataAccess;
using FluentAssertions;
using Xunit;

namespace BeersApi.IntegrationTests.Controllers.Beer
{
   public class BeerControllerGetByIdTests : IClassFixture<CustomWebApplicationFactory<Startup>>, IClassFixture<DbContextFactory>, IDisposable
   {
      private readonly CustomWebApplicationFactory<Startup> _customWebApplicationFactory;
      private readonly BeersApiContext _beersApiContext;
      private int _id;
      private const string ValidBeerName = "Beer name";
      private const string ValidBeerDescription = "Beer description";
      private const double ValidAlcoholLevel = 9.6;
      private const double ValidTiwooRating = 4.5;
      private const string LogoUrl = "http://127.0.0.1:10000/devstoreaccount1/beers/default_beer_edited.jpg";

      public BeerControllerGetByIdTests(CustomWebApplicationFactory<Startup> customWebApplicationFactory, DbContextFactory contextFactory)
      {
         _customWebApplicationFactory = customWebApplicationFactory;
         _beersApiContext = contextFactory.Context;

         var category = Domain.Entities.Category.Create("category name", "category description");
         _beersApiContext.Add(category);

         var color = Domain.Entities.Color.Create("Color name");
         _beersApiContext.Add(color);

         var country = Domain.Entities.Country.Create("Country", "co");
         _beersApiContext.Add(country);

         var flavours = new List<Domain.Entities.Flavour>
         {
            Domain.Entities.Flavour.Create("flavourOneName", "flavourOneDescription"),
            Domain.Entities.Flavour.Create("flavourTwoName", "flavourTwoDescription")
         };
         _beersApiContext.AddRange(flavours);

         var beer = Domain.Entities.Beer.Create(ValidBeerName, ValidBeerDescription, LogoUrl, ValidAlcoholLevel, ValidTiwooRating, category, color, country );
         beer.SetFlavours(flavours);
         _beersApiContext.Add(beer);
         _beersApiContext.SaveChanges();

         _id = beer.Id;
      }

      [Fact]
      public async Task GetBeer_IfWrongId_ReturnsBadRequest()
      {
         //arrange
         _id = 5000;

         //act
         var response = await Exec().ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
      }

      [Fact]
      public async Task GetBeer_IfIdWrongFormat_ReturnsBadRequest()
      {
         //act
         var response = await _customWebApplicationFactory.Get($"/beers/{Guid.NewGuid()}").ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task GetBeer_ValidId_ReturnsBeer()
      {
         //act
         var response = await Exec().ConfigureAwait(false);
         var body = await response.BodyAs<Models.Output.Beers.Beer>();

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.OK);
         body.Name.Should().Be(ValidBeerName);
         body.Description.Should().Be(ValidBeerDescription);
         body.Category.Name.Should().Be("category name");
         body.Category.Description.Should().Be("category description");
         body.Color.Name.Should().Be("Color name");
      }

      public void Dispose()
      {
         _beersApiContext.Beers.RemoveRange(_beersApiContext.Beers);
         _beersApiContext.Categories.RemoveRange(_beersApiContext.Categories);
         _beersApiContext.SaveChanges();
      }


      #region Private Methods

      private Task<HttpResponseMessage> Exec() =>
         _customWebApplicationFactory.Get($"/beers/{_id}");

      #endregion
      
   }
}
