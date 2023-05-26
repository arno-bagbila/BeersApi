using Domain.Entities;
using FluentAssertions;
using System;
using Xunit;

namespace Domain.Tests.UnitTests
{
   public class ColorUnitTests
   {
      private const string Name = "Color name";

      [Theory]
      [InlineData("")]
      [InlineData(" ")]
      public void CreateColor_NameEmpty_ThrowsBeersApiException(string value)
      {
         //act
         Action action = () => Color.Create(value);

         //assert
         var exception = Assert.Throws<BeersApiException>(() => action());
         Assert.Contains(nameof(Category.Name), exception.InvalidData);
      }

      [Fact]
      public void CreateColor_ValidInput_ReturnsCreatedColor()
      {
         //act
         var color = Color.Create(Name);

         //assert
         color.Name.Should().Be(Name);
      }
   }
}
