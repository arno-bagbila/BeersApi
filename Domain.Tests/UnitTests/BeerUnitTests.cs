using Domain.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Domain.Tests.UnitTests
{
   public class BeerUnitTests
   {
      private const string Name = "Beer name test";
      private const string Description = "Beer description test";
      private const double AlcoholLevel = 9.6;
      private const double TiwooRating = 4.5;
      private const string UpdateName = "Beer name test Update";
      private const string UpdateDescription = "Beer description test Update";
      private const double UpdateAlcoholLevel = 10;
      private const double UpdateTiwooRating = 4.0;
      private const string LogoUrl = "http://127.0.0.1:10000/devstoreaccount1/beers/default_beer_edited.jpg";
      private const string UpdateLogoUrl =
         "http://127.0.0.1:10000/devstoreaccount1/beers/default_beer_edited_updated.jpg";
      private const int BrowsersMaxCharactersLimit = 2048;
      private readonly Category _category;
      private readonly Color _color;
      private readonly Country _country;
      //private readonly Category _categoryUpdate;
      //private readonly Color _colorUpdate;
      //private readonly IEnumerable<Flavour> _flavours;
      //private readonly IEnumerable<Flavour> _updateFlavours;

      public BeerUnitTests()
      {
         _category = Category.Create("category name", "category description");
         _color = Color.Create("red");
         _country = Country.Create("country name", "co");

      }

      [Theory]
      [InlineData("")]
      [InlineData(" ")]
      public void CreateBeer_NameEmpty_BeersApiException(string value)
      {
         //act
         Action action = () => Beer.Create(value, Description, LogoUrl, AlcoholLevel, TiwooRating, _category, _color, _country);

         //assert
         var exception = Assert.Throws<BeersApiException>(() => action());
         Assert.Contains(nameof(Beer.Name), exception.InvalidData);
      }

      [Theory]
      [InlineData("")]
      [InlineData(" ")]
      public void CreateBeer_DescriptionEmpty_BeersApiException(string value)
      {
         //act
         Action action = () => Beer.Create(Name, value, LogoUrl, AlcoholLevel, TiwooRating, _category, _color, _country);

         //assert
         var exception = Assert.Throws<BeersApiException>(() => action());
         Assert.Contains(nameof(Beer.Description), exception.InvalidData);
      }

      [Theory]
      [InlineData("")]
      [InlineData(" ")]
      public void CreateBeer_LogoUrlEmpty_BeersApiException(string value)
      {
         //act
         Action action = () => Beer.Create(Name, Description, value, AlcoholLevel, TiwooRating, _category, _color, _country);

         //assert
         var exception = Assert.Throws<BeersApiException>(() => action());
         Assert.Contains(nameof(Beer.LogoUrl), exception.InvalidData);
      }

      [Theory]
      [InlineData(0)]
      [InlineData(0.0)]
      public void CreateBeer_InvalidAlcoholLevel_BeersApiException(double value)
      {
         //act
         Action action = () => Beer.Create(Name, Description, LogoUrl, value, TiwooRating, _category, _color, _country);

         //assert
         var exception = Assert.Throws<BeersApiException>(() => action());
         Assert.Contains(nameof(Beer.AlcoholLevel), exception.InvalidData);
      }

      [Theory]
      [InlineData(0)]
      [InlineData(0.0)]
      public void CreateBeer_InvalidTiwooRating_BeersApiException(double value)
      {
         //act
         Action action = () => Beer.Create(Name, Description, LogoUrl, AlcoholLevel, value, _category, _color, _country);

         //assert
         var exception = Assert.Throws<BeersApiException>(() => action());
         Assert.Contains(nameof(Beer.TiwooRating), exception.InvalidData);
      }

      [Theory]
      [InlineData(null)]
      public void CreateBeer_CategoryNull_BeersApiException(Category value)
      {
         //act
         Action action = () => Beer.Create(Name, Description, LogoUrl, AlcoholLevel, TiwooRating, value, _color, _country);

         //assert
         var exception = Assert.Throws<BeersApiException>(() => action());
         Assert.Contains(nameof(Beer.Category), exception.InvalidData);
      }

      [Theory]
      [InlineData(null)]
      public void CreateBeer_ColorNull_BeersApiException(Color value)
      {
         //act
         Action action = () => Beer.Create(Name, Description, LogoUrl, AlcoholLevel, TiwooRating, _category, value, _country);

         //assert
         var exception = Assert.Throws<BeersApiException>(() => action());
         Assert.Contains(nameof(Beer.Color), exception.InvalidData);
      }

      [Theory]
      [InlineData(null)]
      public void CreateBeer_CountryNull_BeersApiException(Country value)
      {
         //act
         Action action = () => Beer.Create(Name, Description, LogoUrl, AlcoholLevel, TiwooRating, _category, _color, value);

         //assert
         var exception = Assert.Throws<BeersApiException>(() => action());
         Assert.Contains(nameof(Beer.Country), exception.InvalidData);
      }

      [Fact]
      public void SetImages_ValidImages_BeerImageUpdated()
      {
         //arrange
         var beer = Beer.Create(Name, Description, LogoUrl, AlcoholLevel, TiwooRating, _category, _color, _country);
         var images = new List<Image>
         {
            Image.Create("first image", "imageUrl", beer),
            Image.Create("Second image", "imageUrl2", beer)
         };

         //act
         beer.SetImages(images);

         //assert
         beer.Images.Count.Should().Be(2);
      }

      [Fact]
      public void SetFlavours_ValidFlavours_UpdateBeerFlavours()
      {
         //arrange
         var beer = Beer.Create(Name, Description, LogoUrl, AlcoholLevel, TiwooRating, _category, _color, _country);
         var firstFlavour = Flavour.Create("First flavour", "first flavour description");
         firstFlavour.GetType().GetProperty("Id").SetValue(firstFlavour, 1);
         var secondFlavour = Flavour.Create("Second flavour", "Second flavour description");
         secondFlavour.GetType().GetProperty("Id").SetValue(secondFlavour, 2);

         //act
         beer.SetFlavours(new[] { firstFlavour, secondFlavour });

         //assert
         beer.BeerFlavours.Count.Should().Be(2);
      }

      [Fact]
      public void SetImages_DuplicatedImageUrl_DoNotAddDuplicatedImage()
      {
         //arrange
         var beer = Beer.Create(Name, Description, LogoUrl, AlcoholLevel, TiwooRating, _category, _color, _country);
         var images = new List<Image>
         {
            Image.Create("first image", "imageUrl", beer),
            Image.Create("Second image", "imageUrl", beer)
         };

         //act
         beer.SetImages(images);

         //assert
         beer.Images.Count.Should().Be(1);
      }

      [Fact]
      public void SetFlavours_ExistingFlavours_DoNotAddExistingFlavour()
      {
         //arrange
         var beer = Beer.Create(Name, Description, LogoUrl, AlcoholLevel, TiwooRating, _category, _color, _country);
         var firstFlavour = Flavour.Create("First flavour", "first flavour description");
         firstFlavour.GetType().GetProperty("Id").SetValue(firstFlavour, 1);
         var secondFlavour = Flavour.Create("Second flavour", "Second flavour description");
         //setting the id to be the same as the first flavour
         secondFlavour.GetType().GetProperty("Id").SetValue(secondFlavour, 1);

         //act
         beer.SetFlavours(new[] { firstFlavour, secondFlavour });

         //assert
         beer.BeerFlavours.Count.Should().Be(1);
      }

      [Fact]
      public void RemoveFlavours_ExistingFlavours_UpdateBeerFlavours()
      {
         //arrange
         var beer = Beer.Create(Name, Description, LogoUrl, AlcoholLevel, TiwooRating, _category, _color, _country);
         var firstFlavour = Flavour.Create("First flavour", "first flavour description");
         firstFlavour.GetType().GetProperty("Id").SetValue(firstFlavour, 1);
         var secondFlavour = Flavour.Create("Second flavour", "Second flavour description");
         secondFlavour.GetType().GetProperty("Id").SetValue(secondFlavour, 2);
         beer.SetFlavours(new[] { firstFlavour, secondFlavour });

         //act
         beer.RemoveFlavours(new[] { firstFlavour });

         //assert
         beer.BeerFlavours.Count.Should().Be(1);
         beer.BeerFlavours.First().Flavour.Name.Should().Be("Second flavour");
      }

      [Fact]
      public void AddComment_OnDifferentBeer_ThrowsBeersApiException()
      {
         //arrange
         var beer = Beer.Create(Name, Description, LogoUrl, AlcoholLevel, TiwooRating, _category, _color, _country);
         var secondBeer = Beer.Create(Name, Description, LogoUrl, AlcoholLevel, TiwooRating, _category, _color, _country);
         secondBeer.GetType().GetProperty("Id").SetValue(secondBeer, 1);
         var comment = Comment.Create("Comment body", "userFirstName", secondBeer, Guid.NewGuid());


         //act
         Action action = () => beer.AddComment(comment);

         //assert
         var exception = Assert.Throws<BeersApiException>(() => action());
         Assert.Contains($"{nameof(Beer.Comments)}", exception.Message);
      }

      [Fact]
      public void AddComment_ValidComment_AddCommentToBeer()
      {
         //arrange
         var beer = Beer.Create(Name, Description, LogoUrl, AlcoholLevel, TiwooRating, _category, _color, _country);
         var comment = Comment.Create("Comment body", "userFirstName", beer, Guid.NewGuid());


         //act
         beer.AddComment(comment);

         //assert
         beer.Comments.Count.Should().Be(1);
      }
   }
}
