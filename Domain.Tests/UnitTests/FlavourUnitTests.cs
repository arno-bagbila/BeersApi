using Domain.Entities;
using FluentAssertions;
using System;
using Xunit;

namespace Domain.Tests.UnitTests
{
   public class FlavourUnitTests
   {
      private const string Name = "Flavour name test";
      private const string Description = "Flavour description test";

      [Theory]
      [InlineData("")]
      [InlineData(" ")]
      public void CreateFlavour_DescriptionEmpty_ThrowsBeersApiException(string description)
      {
         //act
         Action action = () => Flavour.Create(Name, description);

         //assert
         var exception = Assert.Throws<BeersApiException>(() => action());
         Assert.Contains(nameof(Flavour.Description), exception.InvalidData);
      }

      [Theory]
      [InlineData((""))]
      [InlineData(" ")]
      public void CreateFlavour_NameEmpty_ThrowsBeersException(string name)
      {
         //act
         Action action = () => Flavour.Create(name, Description);

         //assert
         var exception = Assert.Throws<BeersApiException>(() => action());
         Assert.Contains(nameof(Flavour.Name), exception.InvalidData);
      }

      [Fact]
      public void CreateFlavour_ValidInput_CreatesFlavour()
      {
         //act
         var flavour = Flavour.Create(Name, Description);

         //assert
         flavour.Name.Should().Be(Name);
         flavour.Description.Should().Be(Description);
      }

   }
}
