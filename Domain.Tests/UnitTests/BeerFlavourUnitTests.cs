using Domain.Entities;
using System;
using Xunit;

namespace Domain.Tests.UnitTests
{
   public class BeerFlavourUnitTests
   {

      private Beer _beer;
      private Flavour _flavour;
      private Category _category;
      private Color _color;
      private Country _country;

      public BeerFlavourUnitTests()
      {
         _category = Category.Create("category name", "category description");
         _color = Color.Create("red");
         _country = Country.Create("Country name", "co");
         _beer = Beer.Create("Beer name", "Beer description", "logoUrl", 3.7, 2.5, _category, _color, _country);
         _flavour = Flavour.Create("Flavour name", "Flavour description");
      }

      [Theory]
      [InlineData(null)]
      public void CreateBeerFlavour_BeerNull_BeersApiException(Beer value)
      {
         //act
         Action action = () => BeerFlavour.Create(value, _flavour);

         //assert
         var exception = Assert.Throws<BeersApiException>(() => action());
         Assert.Contains(nameof(BeerFlavour.Beer), exception.InvalidData);
      }

      [Theory]
      [InlineData(null)]
      public void CreateBeerFlavour_FlavourNull_BeersApiException(Flavour value)
      {
         //act
         Action action = () => BeerFlavour.Create(_beer, value);

         //assert
         var exception = Assert.Throws<BeersApiException>(() => action());
         Assert.Contains(nameof(BeerFlavour.Flavour), exception.InvalidData);
      }
   }
}
