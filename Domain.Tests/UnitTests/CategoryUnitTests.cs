using Domain.Entities;
using FluentAssertions;
using System;
using Xunit;

namespace Domain.Tests.UnitTests
{
   public class CategoryUnitTests
   {
      private const string Name = "Category name test";
      private const string Description = "Category description test";

      [Theory]
      [InlineData("")]
      [InlineData(" ")]
      [InlineData(null)]
      public void CreateCategory_DescriptionEmpty_BeersApiException(string value)
      {
         //act
         Action action = () => Category.Create(Name, value);

         //assert
         var exception = Assert.Throws<BeersApiException>(() => action());
         Assert.Contains(nameof(Category.Description), exception.InvalidData);
      }

      [Theory]
      [InlineData("")]
      [InlineData(" ")]
      [InlineData(null)]
      public void CreateCategory_EmptyName_ThrowsBeersApiException(string value)
      {
         //act
         Action action = () => Category.Create(value, Description);

         //assert
         var exception = Assert.Throws<BeersApiException>(() => action());
         Assert.Contains(nameof(Category.Name), exception.InvalidData);
      }


      [Fact]
      public void CreateCategory_ValidInput_CreatesCategory()
      {
         //act
         var category = Category.Create(Name, Description);

         //assert
         category.Name.Should().NotBeNullOrWhiteSpace();
         category.Name.Should().Be(Name);
         category.Description.Should().NotBeNullOrWhiteSpace();
         category.Description.Should().Be(Description);
         category.UId.Should().NotBeEmpty();
      }
      
      [Theory]
      [InlineData("")]
      [InlineData(" ")]
      [InlineData(null)]
      public void Set_Invalid_Name(string name)
      {
         //arrange
         var category = Category.Create(Name, Description);
         
         //act
         Action action = () => category.Name = name;
         
         //assert
         var exception = Assert.Throws<BeersApiException>(() => action());
         Assert.Contains(nameof(Category.Name), exception.InvalidData);
      }
      
      [Theory]
      [InlineData("")]
      [InlineData(" ")]
      [InlineData(null)]
      public void Set_Invalid_Description(string description)
      {
         //arrange
         var category = Category.Create(Name, Description);
         
         //act
         Action action = () => category.Description = description;
         
         //assert
         var exception = Assert.Throws<BeersApiException>(() => action());
         Assert.Contains(nameof(Category.Description), exception.InvalidData);
      }
   }
}
