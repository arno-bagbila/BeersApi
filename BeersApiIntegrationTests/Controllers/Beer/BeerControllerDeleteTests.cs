using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BeersApi.IntegrationTests.Helpers;
using DataAccess;
using FluentAssertions;
using Xunit;

namespace BeersApi.IntegrationTests.Controllers.Beer
{
   public class BeerControllerDeleteTests : IClassFixture<CustomWebApplicationFactory<Startup>>, IClassFixture<DbContextFactory>, IDisposable
   {
      #region Data

      private readonly CustomWebApplicationFactory<Startup> _customWebApplicationFactory;
      private readonly BeersApiContext _beersApiContext;
      private const string Name = "Beer delete name";
      private const string Description = "Beer delete description";
      private const double AlcoholLevel = 9.6;
      private const double TiwooRating = 4.5;
      private const string LogoUrl = "http://127.0.0.1:10000/devstoreaccount1/beers/default_beer_edited.jpg";

      #endregion

      #region Constructors

      public BeerControllerDeleteTests(CustomWebApplicationFactory<Startup> customWebApplicationFactory, DbContextFactory contextFactory)
      {
         _customWebApplicationFactory = customWebApplicationFactory;
         _beersApiContext = contextFactory.Context;
      }

      #endregion

      #region Tests

      [Fact]
      public async Task DeleteBeer_WrongBeerId_ReturnsNotFound404()
      {
         //act
         var response = await Exec(5000).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
      }

      [Fact]
      public async Task DeleteBeer_WrongBeerIdForm_ReturnsBadRequest()
      {
         //act
         var response = await _customWebApplicationFactory.Delete($"/beers/{Guid.NewGuid()}").ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task DeleteBeer_ValidId_Returns200OK()
      {
         //arrange
         var beerId = await GetBeerId().ConfigureAwait(false);

         //act
         var response = await Exec(beerId).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.OK);
      }



      #endregion
      public void Dispose()
      {
         _beersApiContext.Beers.RemoveRange(_beersApiContext.Beers);
         _beersApiContext.Categories.RemoveRange(_beersApiContext.Categories);
         _beersApiContext.Colors.RemoveRange(_beersApiContext.Colors);
         _beersApiContext.Flavours.RemoveRange(_beersApiContext.Flavours);
         _beersApiContext.Countries.RemoveRange(_beersApiContext.Countries);
         _beersApiContext.SaveChangesAsync();
      }

      #region Private Methods

      private Task<HttpResponseMessage> Exec(int id) =>
         _customWebApplicationFactory.Delete($"/beers/{id}");

      private async Task<int> GetBeerId()
      {
         var category = Domain.Entities.Category.Create("category name", "category description");
         await _beersApiContext.AddAsync(category).ConfigureAwait(false);
         await _beersApiContext.SaveChangesAsync().ConfigureAwait(false);

         var color = Domain.Entities.Color.Create("Color name");

         await _beersApiContext.AddAsync(color).ConfigureAwait(false);
         await _beersApiContext.SaveChangesAsync().ConfigureAwait(false);


         var country = Domain.Entities.Country.Create("Country", "co");
         await _beersApiContext.AddAsync(country).ConfigureAwait(false);
         await _beersApiContext.SaveChangesAsync().ConfigureAwait(false);

         var flavours = new List<Domain.Entities.Flavour>
         {
            Domain.Entities.Flavour.Create("flavourOneName", "flavourOneDescription"),
            Domain.Entities.Flavour.Create("flavourTwoName", "flavourTwoDescription")
         };

         await _beersApiContext.AddRangeAsync(flavours).ConfigureAwait(false);

         var beer = Domain.Entities.Beer.Create(Name, Description, LogoUrl, AlcoholLevel, TiwooRating, category, color, country);
         await _beersApiContext.SaveChangesAsync().ConfigureAwait(false);
         beer.SetFlavours(flavours);
         _beersApiContext.Add(beer);
         await _beersApiContext.SaveChangesAsync().ConfigureAwait(false);

         return beer.Id;
      }

      #endregion
   }
}
