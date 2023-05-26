using System;
using System.Collections.Generic;
using System.Linq;
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
   public class BeersControllerPutTests : IClassFixture<CustomWebApplicationFactory<Startup>>, IClassFixture<DbContextFactory>, IDisposable
   {
      #region Data

      private readonly CustomWebApplicationFactory<Startup> _customWebApplicationFactory;
      private readonly BeersApiContext _beersApiContext;
      private Models.Input.Beers.Update.UpdateBeer _updateBeer;
      private string _token;
      private const string UpdatedName = "Updte Beer name";
      private const string UpdatedDescription = "Update Beer description";
      private const double UpdatedAlcoholLevel = 9.6;
      private const double UpdatedTiwooRating = 4.5;
      private const string UpdatedLogoUrl = "http://128.0.0.1:10000/devstoreaccount1/beers/default_beer_edited.jpg";
      private const string Name = "Beer name";
      private const string Description = "Beer description";
      private const double AlcoholLevel = 9.6;
      private const double TiwooRating = 4.5;
      private const string LogoUrl = "http://127.0.0.1:10000/devstoreaccount1/beers/default_beer_edited.jpg";

      #endregion

      #region Constructors

      public BeersControllerPutTests(CustomWebApplicationFactory<Startup> customWebApplicationFactory, DbContextFactory contextFactory)
      {
         _customWebApplicationFactory = customWebApplicationFactory;
         _beersApiContext = contextFactory.Context;

         _updateBeer = new Models.Input.Beers.Update.UpdateBeer
         {
            Name = UpdatedName,
            AlcoholLevel = UpdatedAlcoholLevel,
            TiwooRating = UpdatedTiwooRating,
            Description = UpdatedDescription,
            LogoUrl = UpdatedLogoUrl
         };
      }

      #endregion

      #region Tests

      [Fact]
      public async Task UpdateBeer_WrongBeerId_ReturnsNotFound404()
      {
         //arrange
         _updateBeer.CategoryId = await GetCategoryId().ConfigureAwait(false);
         _updateBeer.ColorId = await GetColorId().ConfigureAwait(false);
         _updateBeer.CountryId = await GetCountryId().ConfigureAwait(false);
         _updateBeer.FlavourIds = await GetFlavourIds().ConfigureAwait(false);

         //act
         var response = await Exec(5000).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
      }

      [Fact]
      public async Task UpdateBeer_WrongCategoryId_ReturnsNotFound404()
      {
         //arrange
         var beerId = await GetBeerId().ConfigureAwait(false);
         _updateBeer.CategoryId = 5000;
         _updateBeer.ColorId = await GetColorId().ConfigureAwait(false);
         _updateBeer.CountryId = await GetCountryId().ConfigureAwait(false);
         _updateBeer.FlavourIds = await GetFlavourIds().ConfigureAwait(false);

         //act
         var response = await Exec(beerId).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
      }

      [Fact]
      public async Task UpdateBeer_WrongColorId_ReturnsNotFound404()
      {
         //arrange
         var beerId = await GetBeerId().ConfigureAwait(false);
         _updateBeer.ColorId = 5000;
         _updateBeer.CategoryId = await GetCategoryId().ConfigureAwait(false);
         _updateBeer.CountryId = await GetCountryId().ConfigureAwait(false);
         _updateBeer.FlavourIds = await GetFlavourIds().ConfigureAwait(false);

         //act
         var response = await Exec(beerId).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
      }

      [Fact]
      public async Task UpdateBeer_WrongCountryId_ReturnsNotFound404()
      {
         //arrange
         var beerId = await GetBeerId().ConfigureAwait(false);
         _updateBeer.CountryId = 5000;
         _updateBeer.CategoryId = await GetCategoryId().ConfigureAwait(false);
         _updateBeer.ColorId = await GetColorId().ConfigureAwait(false);
         _updateBeer.FlavourIds = await GetFlavourIds().ConfigureAwait(false);

         //act
         var response = await Exec(beerId).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
      }

      [Fact]
      public async Task UpdateBeer_WrongFlavoursIds_ReturnsNotFound404()
      {
         //arrange
         var beerId = await GetBeerId().ConfigureAwait(false);
         _updateBeer.FlavourIds = new List<int> { 5000, 5001 };
         _updateBeer.CategoryId = await GetCategoryId().ConfigureAwait(false);
         _updateBeer.ColorId = await GetColorId().ConfigureAwait(false);
         _updateBeer.CountryId = await GetCountryId().ConfigureAwait(false);

         //act
         var response = await Exec(beerId).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
      }

      [Fact]
      public async Task UpdateBeer_EmptyName_ReturnsBadRequest()
      {
         //arrange
         var beerId = await GetBeerId().ConfigureAwait(false);
         _updateBeer.Name = string.Empty;
         _updateBeer.CategoryId = await GetCategoryId().ConfigureAwait(false);
         _updateBeer.ColorId = await GetColorId().ConfigureAwait(false);
         _updateBeer.CountryId = await GetCountryId().ConfigureAwait(false);
         _updateBeer.FlavourIds = await GetFlavourIds().ConfigureAwait(false);

         //act
         var response = await Exec(beerId).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task UpdateBeer_NameMoreThan50Characters_ReturnsBadRequest_()
      {
         //arrange
         var beerId = await GetBeerId().ConfigureAwait(false);
         _updateBeer.Name = new string('a', 51);

         //act
         var response = await Exec(beerId).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task UpdateBeer_EmptyDescription_ReturnsBadRequest()
      {
         //arrange
         var beerId = await GetBeerId().ConfigureAwait(false);
         _updateBeer.Description = string.Empty;
         _updateBeer.CategoryId = await GetCategoryId().ConfigureAwait(false);
         _updateBeer.ColorId = await GetColorId().ConfigureAwait(false);
         _updateBeer.CountryId = await GetCountryId().ConfigureAwait(false);
         _updateBeer.FlavourIds = await GetFlavourIds().ConfigureAwait(false);

         //act
         var response = await Exec(beerId).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task UpdateBeer_DescriptionMoreThan3000Characters_ReturnsBadRequest_()
      {
         //arrange
         var beerId = await GetBeerId().ConfigureAwait(false);
         _updateBeer.Name = new string('a', 3001);
         _updateBeer.CategoryId = await GetCategoryId().ConfigureAwait(false);
         _updateBeer.ColorId = await GetColorId().ConfigureAwait(false);
         _updateBeer.CountryId = await GetCountryId().ConfigureAwait(false);
         _updateBeer.FlavourIds = await GetFlavourIds().ConfigureAwait(false);

         //act
         var response = await Exec(beerId).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task UpdateBeer_AlcoholLevelLessThan0_ReturnsBadRequest()
      {
         //arrange
         var beerId = await GetBeerId().ConfigureAwait(false);
         _updateBeer.AlcoholLevel = -0.1;
         _updateBeer.CategoryId = await GetCategoryId().ConfigureAwait(false);
         _updateBeer.ColorId = await GetColorId().ConfigureAwait(false);
         _updateBeer.CountryId = await GetCountryId().ConfigureAwait(false);
         _updateBeer.FlavourIds = await GetFlavourIds().ConfigureAwait(false);

         //act
         var response = await Exec(beerId).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task UpdateBeer_AlcoholLevelMoreThan100_ReturnsBadRequest()
      {
         //arrange
         var beerId = await GetBeerId().ConfigureAwait(false);
         _updateBeer.AlcoholLevel = 100.1;
         _updateBeer.CategoryId = await GetCategoryId().ConfigureAwait(false);
         _updateBeer.ColorId = await GetColorId().ConfigureAwait(false);
         _updateBeer.CountryId = await GetCountryId().ConfigureAwait(false);
         _updateBeer.FlavourIds = await GetFlavourIds().ConfigureAwait(false);

         //act
         var response = await Exec(beerId).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task UpdateBeer_TiwooRatingLessThan0_ReturnsBadRequest()
      {
         //arrange
         var beerId = await GetBeerId().ConfigureAwait(false);
         _updateBeer.TiwooRating = -0.1;
         _updateBeer.CategoryId = await GetCategoryId().ConfigureAwait(false);
         _updateBeer.ColorId = await GetColorId().ConfigureAwait(false);
         _updateBeer.CountryId = await GetCountryId().ConfigureAwait(false);
         _updateBeer.FlavourIds = await GetFlavourIds().ConfigureAwait(false);

         //act
         var response = await Exec(beerId).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task UpdateBeer_TiwooRatingMoreThan5_ReturnsBadRequest()
      {
         //arrange
         var beerId = await GetBeerId().ConfigureAwait(false);
         _updateBeer.TiwooRating = 5.01;
         _updateBeer.CategoryId = await GetCategoryId().ConfigureAwait(false);
         _updateBeer.ColorId = await GetColorId().ConfigureAwait(false);
         _updateBeer.CountryId = await GetCountryId().ConfigureAwait(false);
         _updateBeer.FlavourIds = await GetFlavourIds().ConfigureAwait(false);

         //act
         var response = await Exec(beerId).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task UpdateBeer_LogoUrlEmpty_ReturnsBadRequest()
      {
         //arrange
         var beerId = await GetBeerId().ConfigureAwait(false);
         _updateBeer.LogoUrl = string.Empty;
         _updateBeer.CategoryId = await GetCategoryId().ConfigureAwait(false);
         _updateBeer.ColorId = await GetColorId().ConfigureAwait(false);
         _updateBeer.CountryId = await GetCountryId().ConfigureAwait(false);
         _updateBeer.FlavourIds = await GetFlavourIds().ConfigureAwait(false);

         //act
         var response = await Exec(beerId).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task UpdateBeer_LogoUrlMoreThan2048_ReturnsBadRequest()
      {
         //arrange
         var beerId = await GetBeerId().ConfigureAwait(false);
         _updateBeer.LogoUrl = new string('a', 2049);
         _updateBeer.CategoryId = await GetCategoryId().ConfigureAwait(false);
         _updateBeer.ColorId = await GetColorId().ConfigureAwait(false);
         _updateBeer.CountryId = await GetCountryId().ConfigureAwait(false);
         _updateBeer.FlavourIds = await GetFlavourIds().ConfigureAwait(false);

         //act
         var response = await Exec(beerId).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task UpdateBeer_LogoUrlWrongFormat_ReturnsBadRequest()
      {
         //arrange
         var beerId = await GetBeerId().ConfigureAwait(false);
         _updateBeer.LogoUrl = "logoUrl";
         _updateBeer.CategoryId = await GetCategoryId().ConfigureAwait(false);
         _updateBeer.ColorId = await GetColorId().ConfigureAwait(false);
         _updateBeer.CountryId = await GetCountryId().ConfigureAwait(false);
         _updateBeer.FlavourIds = await GetFlavourIds().ConfigureAwait(false);

         //act
         var response = await Exec(beerId).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task UpdateBeer_CategoryIdNotProvided_ReturnsBadRequest()
      {
         //arrange
         var beerId = await GetBeerId().ConfigureAwait(false);
         _updateBeer.CategoryId = 0;
         _updateBeer.ColorId = await GetColorId().ConfigureAwait(false);
         _updateBeer.CountryId = await GetCountryId().ConfigureAwait(false);
         _updateBeer.FlavourIds = await GetFlavourIds().ConfigureAwait(false);

         //act
         var response = await Exec(beerId).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task UpdateBeer_ColorIdNotProvided_ReturnsBadRequest()
      {
         //arrange
         var beerId = await GetBeerId().ConfigureAwait(false);
         _updateBeer.ColorId = 0;
         _updateBeer.CategoryId = await GetCategoryId().ConfigureAwait(false);
         _updateBeer.CountryId = await GetCountryId().ConfigureAwait(false);
         _updateBeer.FlavourIds = await GetFlavourIds().ConfigureAwait(false);

         //act
         var response = await Exec(beerId).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task UpdateBeer_CountryIdNotProvided_ReturnsBadRequest()
      {
         //arrange
         var beerId = await GetBeerId().ConfigureAwait(false);
         _updateBeer.CountryId = 0;
         _updateBeer.CategoryId = await GetCategoryId().ConfigureAwait(false);
         _updateBeer.ColorId = await GetColorId().ConfigureAwait(false);
         _updateBeer.FlavourIds = await GetFlavourIds().ConfigureAwait(false);

         //act
         var response = await Exec(beerId).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task UpdateBeer_FlavourIdsNull_ReturnsBadRequest()
      {
         //arrange
         var beerId = await GetBeerId().ConfigureAwait(false);
         _updateBeer.FlavourIds = null;
         _updateBeer.CategoryId = await GetCategoryId().ConfigureAwait(false);
         _updateBeer.ColorId = await GetColorId().ConfigureAwait(false);
         _updateBeer.CountryId = await GetCountryId().ConfigureAwait(false);

         //act
         var response = await Exec(beerId).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task UpdateBeer_FlavourIdsEmptyList_ReturnsBadRequest()
      {
         //arrange
         var beerId = await GetBeerId().ConfigureAwait(false);
         _updateBeer.FlavourIds = new List<int>();
         _updateBeer.CategoryId = await GetCategoryId().ConfigureAwait(false);
         _updateBeer.ColorId = await GetColorId().ConfigureAwait(false);
         _updateBeer.CountryId = await GetCountryId().ConfigureAwait(false);

         //act
         var response = await Exec(beerId).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task UpdateBeer_ValidInputs_ReturnsUpdatedBeer()
      {
         //arrange
         var beerId = await GetBeerId().ConfigureAwait(false);
         _updateBeer.CategoryId = await GetCategoryId().ConfigureAwait(false);
         _updateBeer.ColorId = await GetColorId().ConfigureAwait(false);
         _updateBeer.CountryId = await GetCountryId().ConfigureAwait(false);
         _updateBeer.FlavourIds = await GetFlavourIds().ConfigureAwait(false);

         //act
         var response = await Exec(beerId).ConfigureAwait(false);
         var updatedBeer = await response.BodyAs<Models.Output.Beers.Beer>();

         //assert
         updatedBeer.Id.Should().Be(beerId);
         updatedBeer.Name.Should().Be(UpdatedName);
         updatedBeer.Description.Should().Be(UpdatedDescription);
         updatedBeer.AlcoholLevel.Should().Be(UpdatedAlcoholLevel);
         updatedBeer.TiwooRating.Should().Be(UpdatedTiwooRating);
         updatedBeer.LogoUrl.Should().Be(UpdatedLogoUrl);
         updatedBeer.Category.Name.Should().Be("Update category name");
         updatedBeer.Color.Name.Should().Be("update color name");
         updatedBeer.Country.Name.Should().Be("update country name");
         updatedBeer.Flavours.Count().Should().Be(2);
      }

      #endregion

      public void Dispose()
      {

         //_beersApiContext.Beers.RemoveRange(_beersApiContext.Beers);
         //_beersApiContext.Categories.RemoveRange(_beersApiContext.Categories);
         //_beersApiContext.Colors.RemoveRange(_beersApiContext.Colors);
         //_beersApiContext.Flavours.RemoveRange(_beersApiContext.Flavours);
         //_beersApiContext.Countries.RemoveRange(_beersApiContext.Countries);
         //_beersApiContext.SaveChangesAsync();
      }

      private Task<HttpResponseMessage> Exec(int id) =>
         _customWebApplicationFactory.Put($"/beers/{id}", _updateBeer);

      private async Task<int> GetCategoryId()
      {
         var category = Domain.Entities.Category.Create("Update category name", "update category description");
         await _beersApiContext.Categories.AddAsync(category).ConfigureAwait(false);
         await _beersApiContext.SaveChangesAsync().ConfigureAwait(false);

         return category.Id;
      }

      private async Task<int> GetColorId()
      {
         var color = Domain.Entities.Color.Create("update color name");
         await _beersApiContext.AddAsync(color).ConfigureAwait(false);
         await _beersApiContext.SaveChangesAsync().ConfigureAwait(false);

         return color.Id;
      }

      private async Task<IEnumerable<int>> GetFlavourIds()
      {
         var flavours = new List<Domain.Entities.Flavour>
         {
            Domain.Entities.Flavour.Create("flavour3Name", "flavour3Description"),
            Domain.Entities.Flavour.Create("flavour4Name", "flavour4Description")
         };
         await _beersApiContext.AddRangeAsync(flavours).ConfigureAwait(false);
         await _beersApiContext.SaveChangesAsync().ConfigureAwait(false);

         return flavours.Select(f => f.Id);
      }

      private async Task<int> GetCountryId()
      {
         var country = Domain.Entities.Country.Create("update country name", "uco");
         await _beersApiContext.AddAsync(country).ConfigureAwait(false);
         await _beersApiContext.SaveChangesAsync().ConfigureAwait(false);

         return country.Id;
      }

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

         return  beer.Id;
      }
   }
}
