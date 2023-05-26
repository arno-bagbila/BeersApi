using Domain.Entities;
using FluentAssertions;
using System;
using Xunit;

namespace Domain.Tests.UnitTests
{
   public class ImageUnitTests
   {
      const string Title = "Title";
      const string ImageUrl = "Filename";
      private Beer _beer;
      private Category _category;
      private Color _color;
      private Country _country;

      public ImageUnitTests()
      {
         _category = Category.Create("category name", "category description");
         _color = Color.Create("red");
         _country = Country.Create("Country name", "co");
         _beer = Beer.Create("Beer name", "Beer description", "logoUrl", 3.7, 2.5, _category, _color, _country);
      }

      [Theory]
      [InlineData("")]
      [InlineData(" ")]
      public void CreateImage_TitleEmpty_BeersApiException(string value)
      {
         //act
         Action action = () => Image.Create(value, ImageUrl, _beer);

         //assert
         var exception = Assert.Throws<BeersApiException>(() => action());
         Assert.Contains(nameof(Image.Title), exception.InvalidData);
      }

      [Theory]
      [InlineData("")]
      [InlineData(" ")]
      public void CreateImage_ImageUrlEmpty_BeersApiException(string value)
      {
         //act
         Action action = () => Image.Create(Title, value, _beer);

         //assert
         var exception = Assert.Throws<BeersApiException>(() => action());
         Assert.Contains(nameof(Image.ImageUrl), exception.InvalidData);
      }

      [Theory]
      [InlineData(null)]
      public void CreateImage_BeerNull_BeersApiException(Beer value)
      {
         //act
         Action action = () => Image.Create(Title, ImageUrl, value);

         //assert
         var exception = Assert.Throws<BeersApiException>(() => action());
         Assert.Contains(nameof(Image.Beer), exception.InvalidData);
      }

      [Fact]
      public void CreateImage_WithValidParameters_CreateImage()
      {
         //act
         var image = Image.Create(Title, ImageUrl, _beer);

         //assert
         image.UId.Should().NotBe(Guid.Empty);
         image.Title.Should().Be(Title);
         image.ImageUrl.Should().Be(ImageUrl);
         image.Beer.Name.Should().Be("Beer name");
      }
   }
}
