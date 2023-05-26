using Domain.Entities;
using FluentAssertions;
using System;
using Xunit;

namespace Domain.Tests.UnitTests
{
   public class EntityBaseUnitTests
   {
      const string ValidName = "Entity name test";

      [Fact]
      public void Create_EntityBase_Empty_Name_Throw_ArgumentNullException()
      {
         //act
         var exception = Record.Exception(() => new EntityBase(string.Empty));

         //assert
         exception.Should().BeOfType<ArgumentNullException>();
      }

      [Fact]
      public void Create_EntityBase_Name_More_Than_50_Characters()
      {
         //act
         var exception = Record.Exception(() => new EntityBase(new string('a', 51)));

         //assert
         exception.Should().BeOfType<ArgumentException>();
      }
   }
}
