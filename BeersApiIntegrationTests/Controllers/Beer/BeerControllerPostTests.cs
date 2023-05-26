using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using BeersApi.IntegrationTests.Extensions;
using BeersApi.IntegrationTests.Helpers;
using BeersApi.Models.Input.Beers.Create;
using DataAccess;
using FluentAssertions;
using IdentityModel;
using Xunit;

namespace BeersApi.IntegrationTests.Controllers.Beer
{
   public class BeerControllerPostTests : IClassFixture<CustomWebApplicationFactory<Startup>>, IClassFixture<DbContextFactory>, IDisposable
   {
      private readonly CustomWebApplicationFactory<Startup> _customWebApplicationFactory;
      private readonly BeersApiContext _beersApiContext;
      private string _token;
      private CreateBeer _createBeer;
      private const string Name = "Beer name";
      private const string Description = "Beer description";
      private const string LogoUrl = "http://127.0.0.1:10000/devstoreaccount1/beers/default_beer_edited.jpg";
      private const double AlcoholLevel = 9.6;
      private const double TiwooRating = 4.5;


      public BeerControllerPostTests(CustomWebApplicationFactory<Startup> customWebApplicationFactory, DbContextFactory contextFactory)
      {
         _customWebApplicationFactory = customWebApplicationFactory;
         _beersApiContext = contextFactory.Context;

         _createBeer = new CreateBeer
         {
            Name = Name,
            AlcoholLevel = AlcoholLevel,
            TiwooRating = TiwooRating,
            Description = Description,
            LogoUrl = LogoUrl
         };

         var claims = new List<Claim>
         {
            new Claim(JwtClaimTypes.GivenName, "testUser"),
            new Claim(JwtClaimTypes.Role, "admin")
         };

         _token = _customWebApplicationFactory.Jwt.GenerateToken(claims);
      }

      #region TESTS

      [Fact]
      public async Task CreateBeer_NoTokenProvided_ReturnUnauthorized401()
      {
         //arrange
         _token = "";
         _createBeer.CategoryId = await GetCategoryId().ConfigureAwait(false);
         _createBeer.ColorId = await GetColorId().ConfigureAwait(false);
         _createBeer.CountryId = await GetCountryId().ConfigureAwait(false);
         _createBeer.FlavourIds = await GetFlavourIds().ConfigureAwait(false);

         //act
         var response = await Exec(_createBeer);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
      }

      [Fact]
      public async Task CreateBeer_NotAdmin_ReturnUnauthorized403()
      {
         //arrange
         var claims = new List<Claim>
         {
            new Claim(JwtClaimTypes.GivenName, "testUser"),
            new Claim(JwtClaimTypes.Role, "User")
         };

         _token = _customWebApplicationFactory.Jwt.GenerateToken(claims);
         _createBeer.CategoryId = await GetCategoryId().ConfigureAwait(false);
         _createBeer.ColorId = await GetColorId().ConfigureAwait(false);
         _createBeer.CountryId = await GetCountryId().ConfigureAwait(false);
         _createBeer.FlavourIds = await GetFlavourIds().ConfigureAwait(false);

         //act
         var response = await Exec(_createBeer);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
      }

      [Fact]
      public async Task CreateBeer_EmptyName_ReturnsBadRequest()
      {
         //arrange
         _createBeer.Name = string.Empty;
         _createBeer.CategoryId = await GetCategoryId().ConfigureAwait(false);
         _createBeer.ColorId = await GetColorId().ConfigureAwait(false);
         _createBeer.CountryId = await GetCountryId().ConfigureAwait(false);
         _createBeer.FlavourIds = await GetFlavourIds().ConfigureAwait(false);

         //act
         var response = await Exec(_createBeer).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task CreateBeer_NameMoreThan50Characters_ReturnBadRequest()
      {
         //arrange
         _createBeer.Name = new string('a', 51);
         _createBeer.CategoryId = await GetCategoryId().ConfigureAwait(false);
         _createBeer.ColorId = await GetColorId().ConfigureAwait(false);
         _createBeer.CountryId = await GetCountryId().ConfigureAwait(false);
         _createBeer.FlavourIds = await GetFlavourIds().ConfigureAwait(false);

         //act
         var response = await Exec(_createBeer).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task CreateBeer_DescriptionEmpty_ReturnsBadRequest()
      {
         //arrange
         _createBeer.Description = string.Empty;
         _createBeer.CategoryId = await GetCategoryId().ConfigureAwait(false);
         _createBeer.ColorId = await GetColorId().ConfigureAwait(false);
         _createBeer.CountryId = await GetCountryId().ConfigureAwait(false);
         _createBeer.FlavourIds = await GetFlavourIds().ConfigureAwait(false);

         //act
         var response = await Exec(_createBeer).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task CreateBeer_DescriptionMoreThan3000Characters_ReturnsBadRequest()
      {
         //arrange
         _createBeer.Description = new string('a', 3001);
         _createBeer.CategoryId = await GetCategoryId().ConfigureAwait(false);
         _createBeer.ColorId = await GetColorId().ConfigureAwait(false);
         _createBeer.CountryId = await GetCountryId().ConfigureAwait(false);
         _createBeer.FlavourIds = await GetFlavourIds().ConfigureAwait(false);

         //act
         var response = await Exec(_createBeer).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task CreateBeer_AlcoholLevelLessThan0_ReturnsBadRequest()
      {
         //arrange
         _createBeer.AlcoholLevel = -0.1;
         _createBeer.CategoryId = await GetCategoryId().ConfigureAwait(false);
         _createBeer.ColorId = await GetColorId().ConfigureAwait(false);
         _createBeer.CountryId = await GetCountryId().ConfigureAwait(false);
         _createBeer.FlavourIds = await GetFlavourIds().ConfigureAwait(false);

         //act
         var response = await Exec(_createBeer).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task CreateBeer_AlcoholLevelMoreThan100_ReturnsBadRequest_()
      {
         //arrange
         _createBeer.AlcoholLevel = 100.1;
         _createBeer.CategoryId = await GetCategoryId().ConfigureAwait(false);
         _createBeer.ColorId = await GetColorId().ConfigureAwait(false);
         _createBeer.CountryId = await GetCountryId().ConfigureAwait(false);
         _createBeer.FlavourIds = await GetFlavourIds().ConfigureAwait(false);

         //act
         var response = await Exec(_createBeer).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task CreateBeer_TiwooRatingMoreThan5_ReturnsBadRequest_()
      {
         //arrange
         _createBeer.TiwooRating = 5.1;
         _createBeer.CategoryId = await GetCategoryId().ConfigureAwait(false);
         _createBeer.ColorId = await GetColorId().ConfigureAwait(false);
         _createBeer.CountryId = await GetCountryId().ConfigureAwait(false);
         _createBeer.FlavourIds = await GetFlavourIds().ConfigureAwait(false);

         //act
         var response = await Exec(_createBeer).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task CreateBeer_TiwooRatingLessThan0_ReturnsBadRequest_()
      {
         //arrange
         _createBeer.TiwooRating = -0.1;
         _createBeer.CategoryId = await GetCategoryId().ConfigureAwait(false);
         _createBeer.ColorId = await GetColorId().ConfigureAwait(false);
         _createBeer.CountryId = await GetCountryId().ConfigureAwait(false);
         _createBeer.FlavourIds = await GetFlavourIds().ConfigureAwait(false);

         //act
         var response = await Exec(_createBeer).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      //LogoUrl
      [Fact]
      public async Task CreateBeer_EmptyLogoUrl_ReturnsBadRequest()
      {
         //arrange
         _createBeer.LogoUrl = string.Empty;
         _createBeer.CategoryId = await GetCategoryId().ConfigureAwait(false);
         _createBeer.ColorId = await GetColorId().ConfigureAwait(false);
         _createBeer.CountryId = await GetCountryId().ConfigureAwait(false);
         _createBeer.FlavourIds = await GetFlavourIds().ConfigureAwait(false);

         //act
         var response = await Exec(_createBeer).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task CreateBeer_LogoUrlLengthMoreThan2048_ReturnsBadRequest()
      {
         //arrange
         _createBeer.LogoUrl = new string('a', 2049);
         _createBeer.CategoryId = await GetCategoryId().ConfigureAwait(false);
         _createBeer.ColorId = await GetColorId().ConfigureAwait(false);
         _createBeer.CountryId = await GetCountryId().ConfigureAwait(false);
         _createBeer.FlavourIds = await GetFlavourIds().ConfigureAwait(false);

         //act
         var response = await Exec(_createBeer).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task CreateBeer_LogoUrlWrongUriFormat_ReturnsBadRequest()
      {
         //arrange
         _createBeer.LogoUrl = "logoUrl";
         _createBeer.CategoryId = await GetCategoryId().ConfigureAwait(false);
         _createBeer.ColorId = await GetColorId().ConfigureAwait(false);
         _createBeer.CountryId = await GetCountryId().ConfigureAwait(false);
         _createBeer.FlavourIds = await GetFlavourIds().ConfigureAwait(false);

         //act
         var response = await Exec(_createBeer).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task CreateBeer_CategoryIdEqual0_ReturnsBadRequest()
      {
         //arrange
         _createBeer.CategoryId = 0;
         _createBeer.ColorId = await GetColorId().ConfigureAwait(false);
         _createBeer.CountryId = await GetCountryId().ConfigureAwait(false);
         _createBeer.FlavourIds = await GetFlavourIds().ConfigureAwait(false);

         //act
         var response = await Exec(_createBeer).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task CreateBeer_CategoryIdNotProvided_ReturnsBadRequest()
      {
         //arrange
         _createBeer.ColorId = await GetColorId();
         _createBeer.FlavourIds = await GetFlavourIds().ConfigureAwait(false);
         _createBeer.CountryId = await GetCountryId().ConfigureAwait(false);

         //act
         var response = await Exec(_createBeer).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task CreateBeer_WrongCategoryId_ReturnsNotFound404()
      {
         //arrange
         _createBeer.CategoryId = 5000;
         _createBeer.ColorId = await GetColorId();
         _createBeer.FlavourIds = await GetFlavourIds().ConfigureAwait(false);
         _createBeer.CountryId = await GetCountryId().ConfigureAwait(false);

         //act
         var response = await Exec(_createBeer).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
      }

      [Fact]
      public async Task CreateBeer_FlavourIdsNull_ReturnsBadRequest()
      {
         //arrange
         _createBeer.CategoryId = await GetCategoryId().ConfigureAwait(false);
         _createBeer.ColorId = await GetColorId().ConfigureAwait(false);
         _createBeer.FlavourIds = null;
         _createBeer.CountryId = await GetCountryId().ConfigureAwait(false);

         //act
         var response = await Exec(_createBeer).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task CreateBeer_FlavourIdsNotProvided_ReturnsBadRequest()
      {
         //arrange
         _createBeer.CategoryId = await GetCategoryId();
         _createBeer.ColorId = await GetColorId();
         _createBeer.CountryId = await GetCountryId().ConfigureAwait(false);

         //act
         var response = await Exec(_createBeer).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task CreateBeer_WrongFlavourIds_ReturnsNotFound404()
      {
         //arrange
         _createBeer.CategoryId = await GetCategoryId();
         _createBeer.ColorId = await GetColorId();
         _createBeer.CountryId = await GetCountryId().ConfigureAwait(false);
         _createBeer.FlavourIds = new List<int> {5000, 5001};

         //act
         var response = await Exec(_createBeer).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
      }

      [Fact]
      public async Task CreateBeer_ColorIdEquals0_ReturnsBadRequest()
      {
         //arrange
         _createBeer.ColorId = 0;
         _createBeer.CategoryId = await GetCategoryId().ConfigureAwait(false);
         _createBeer.CountryId = await GetCountryId().ConfigureAwait(false);
         _createBeer.FlavourIds = await GetFlavourIds().ConfigureAwait(false);

         //act
         var response = await Exec(_createBeer).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task CreateBeer_ColorIdNotProvided_ReturnsBadRequest()
      {
         //arrange
         _createBeer.CategoryId = await GetCategoryId();
         _createBeer.FlavourIds = await GetFlavourIds().ConfigureAwait(false);
         _createBeer.CategoryId = await GetCategoryId().ConfigureAwait(false);
         _createBeer.CountryId = await GetCountryId().ConfigureAwait(false);

         //act
         var response = await Exec(_createBeer).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task CreateBeer_WrongColorId_ReturnsNotFound404()
      {
         //arrange
         _createBeer.CategoryId = await GetCategoryId();
         _createBeer.FlavourIds = await GetFlavourIds().ConfigureAwait(false);
         _createBeer.CategoryId = await GetCategoryId().ConfigureAwait(false);
         _createBeer.CountryId = await GetCountryId().ConfigureAwait(false);
         _createBeer.ColorId = 5000;

         //act
         var response = await Exec(_createBeer).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
      }

      [Fact]
      public async Task CreateBeer_CountryIdEquals0_ReturnsBadRequest()
      {
         //arrange
         _createBeer.ColorId = await GetColorId().ConfigureAwait(false);
         _createBeer.CategoryId = await GetCategoryId().ConfigureAwait(false);
         _createBeer.CountryId = 0;
         _createBeer.FlavourIds = await GetFlavourIds().ConfigureAwait(false);

         //act
         var response = await Exec(_createBeer).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task CreateBeer_CountryIdNotProvided_ReturnsBadRequest()
      {
         //arrange
         _createBeer.CategoryId = await GetCategoryId();
         _createBeer.FlavourIds = await GetFlavourIds().ConfigureAwait(false);
         _createBeer.CategoryId = await GetCategoryId().ConfigureAwait(false);

         //act
         var response = await Exec(_createBeer).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task CreateBeer_WrongCountryId_ReturnsNotFound404()
      {
         //arrange
         _createBeer.CategoryId = await GetCategoryId();
         _createBeer.FlavourIds = await GetFlavourIds().ConfigureAwait(false);
         _createBeer.CategoryId = await GetCategoryId().ConfigureAwait(false);
         _createBeer.CountryId = 5000;
         _createBeer.ColorId = await GetColorId().ConfigureAwait(false);

         //act
         var response = await Exec(_createBeer).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
      }

      [Fact]
      public async Task CreateBeer_ValidInput_ReturnsCreatedBeer()
      {
         //arrange
         _createBeer.CategoryId = await GetCategoryId().ConfigureAwait(false);
         _createBeer.ColorId = await GetColorId().ConfigureAwait(false);
         _createBeer.CountryId = await GetCountryId().ConfigureAwait(false);
         _createBeer.FlavourIds = await GetFlavourIds().ConfigureAwait(false);

         //act
         var response = await Exec(_createBeer).ConfigureAwait(false);
         var beer = await response.BodyAs<Models.Output.Beers.Beer>();

         //assert
         beer.Id.Should().BeGreaterThan(0);
         beer.Name.Should().Be(Name);
         beer.Category.Should().NotBeNull();
         beer.Category.Name.Should().Be("category name");
         beer.Color.Should().NotBeNull();
         beer.Color.Name.Should().Be("color name");
         beer.Flavours.Count().Should().Be(2);
         beer.Flavours.FirstOrDefault().Should().NotBeNull();
      }

      #endregion


      public void Dispose()
      {
         _beersApiContext.Beers.RemoveRange(_beersApiContext.Beers);
         _beersApiContext.Categories.RemoveRange(_beersApiContext.Categories);
         _beersApiContext.Colors.RemoveRange(_beersApiContext.Colors);
         _beersApiContext.Flavours.RemoveRange(_beersApiContext.Flavours);
         _beersApiContext.Countries.RemoveRange(_beersApiContext.Countries);
         _beersApiContext.SaveChanges();
      }

      #region Private Methods

      private Task<HttpResponseMessage> Exec(CreateBeer beer) => _customWebApplicationFactory.AddAuth(_token).Post("/beers", beer);

      private async Task<int> GetCategoryId()
      {
         var category = Domain.Entities.Category.Create("category name", "category description");
         await _beersApiContext.Categories.AddAsync(category);
         await _beersApiContext.SaveChangesAsync();

         return category.Id;
      }

      private async Task<int> GetColorId()
      {
         var color = Domain.Entities.Color.Create("color name");
         await _beersApiContext.AddAsync(color);
         await _beersApiContext.SaveChangesAsync();

         return color.Id;
      }

      private async Task<IEnumerable<int>> GetFlavourIds()
      {
         var flavours = new List<Domain.Entities.Flavour>
         {
            Domain.Entities.Flavour.Create("flavourOneName", "flavourOneDescription"),
            Domain.Entities.Flavour.Create("flavourTwoName", "flavourTwoDescription")
         };
         _beersApiContext.AddRange(flavours);
         await _beersApiContext.SaveChangesAsync();

         return flavours.Select(f => f.Id);
      }

      private async Task<int> GetCountryId()
      {
         var country = Domain.Entities.Country.Create("country name", "co");
         await _beersApiContext.AddAsync(country);
         await _beersApiContext.SaveChangesAsync();

         return country.Id;
      }

      #endregion
   }
}
