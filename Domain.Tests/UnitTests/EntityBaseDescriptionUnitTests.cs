using Domain.Entities;
using FluentAssertions;
using System;
using Xunit;

namespace Domain.Tests.UnitTests
{
   public class EntityBaseDescriptionUnitTests
   {
      const string ValidName = "EntityBaseDescription name test";
      const string ValidDescription = "EntityBaseDescription description test";

      [Fact]
      public void Create_EntityBaseDescription_Description_Empty_Throw_ArgumentNullException()
      {
         //act
         var exception = Record.Exception(() => new EntityBaseDescription(ValidName, string.Empty));

         //assert
         exception.Should().BeOfType<ArgumentNullException>();
      }

      [Fact]
      public void Create_EntityBaseDescription_Description_Less_Than_3_Characters_Throw_ArgumentException()
      {
         //act
         var exception = Record.Exception(() => new EntityBaseDescription(ValidName, "12"));

         //assert
         exception.Should().BeOfType<ArgumentException>();
      }
   }
}
