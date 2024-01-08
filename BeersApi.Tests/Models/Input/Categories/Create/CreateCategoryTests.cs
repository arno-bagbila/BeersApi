using BeersApi.Models.Input.Categories.Create;
using FluentValidation.TestHelper;
using System.Collections.Generic;
using Xunit;

namespace BeersApi.Tests.Models.Input.Categories.Create
{

   public class CreateCategoryTests
   {
      private const int DescriptionMaxLength = 3000;
      private const int NameMaxLength = 50;

      private readonly CreateCategoryValidator _createCategoryValidator = new();

      [Fact]
      public void CreateCategory_InvalidDescription_ShouldHaveError()
      {
          var invalidDescriptions = new List<string> { null, string.Empty, " ", new('a', DescriptionMaxLength + 1), "12" };

          foreach (var invalidDescription in invalidDescriptions)
          {
              var model = new CreateCategory { Description = invalidDescription };
              var validator = _createCategoryValidator.TestValidate(model);
              validator.ShouldHaveValidationErrorFor(c => c.Description);
          }
      }

      [Fact]
      public void CreateCategory_InvalidName_ShouldHaveError()
      {
          var invalidNames = new List<string> { null, string.Empty, " ", new('a', NameMaxLength + 1), "12" };

          foreach (var invalidName in invalidNames)
          {
              var model = new CreateCategory { Name = invalidName };
              var validator = _createCategoryValidator.TestValidate(model);
              validator.ShouldHaveValidationErrorFor(c => c.Name);
          }
      }
   }
}
