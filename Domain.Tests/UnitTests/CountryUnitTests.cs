using Domain.Entities;
using FluentAssertions;
using System;
using Xunit;

namespace Domain.Tests.UnitTests
{
   public class CountryUnitTests
   {
      private const string CountryName = "Country Test";
      private const string CountryCode = "code";

      [Theory]
      [InlineData("")]
      [InlineData(" ")]
      public void CreateCountry_CountryNameEmpty_ThrowsBeersApiException(string value)
      {
         //act
         Action action = () => Country.Create(value, CountryCode);

         //assert
         var exception = Assert.Throws<BeersApiException>(() => action());
         Assert.Contains(nameof(Country.Name), exception.InvalidData);
      }

      [Theory]
      [InlineData("")]
      [InlineData(" ")]
      public void CreateCountry_CountryCodeEmpty_ThrowsBeersApiException(string value)
      {
         //act
         Action action = () => Country.Create(CountryName, value);

         //assert
         var exception = Assert.Throws<BeersApiException>(() => action());
         Assert.Contains(nameof(Country.Code), exception.InvalidData);

      }

      [Fact]
      public void CreateCountry_ValidInput_CreatesCountryObject()
      {
         //act
         var country = Country.Create(CountryName, CountryCode);

         //assert
         country.Name.Should().Be(CountryName);
         country.Code.Should().Be(CountryCode);
      }
   }
}
