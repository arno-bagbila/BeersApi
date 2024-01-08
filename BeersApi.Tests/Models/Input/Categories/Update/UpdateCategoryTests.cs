using BeersApi.Models.Input.Categories.Update;
using FluentValidation.TestHelper;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BeersApi.Tests.Models.Input.Categories.Update
{
   public class UpdateCategoryTests
   {
      private const int DescriptionMaxLength = 3000;
      private const int NameMaxLength = 50;
      private const int MinimumLength = 3;

      private readonly UpdateCategoryValidator _updateCategoryValidator = new();

      [Fact]
      public void UpdateCategory_InvalidDescriptions_ShouldHaveError()
      {
          var invalidDescriptions = new List<string> { null, string.Empty, " " };

          foreach (var validator in invalidDescriptions.Select(invalidDescription => new UpdateCategory { Description = invalidDescription })
                       .Select(model => _updateCategoryValidator.TestValidate(model)))
          {
              validator.ShouldHaveValidationErrorFor(c => c.Description)
                  .WithErrorMessage("'Description' must not be null or empty.");
          }
      }

      [Fact]
      public void UpdateCategory_DescriptionTooLong_ShouldHaveError()
      {
          var model = new UpdateCategory { Description = new string('a', 3001) };
          var validator = _updateCategoryValidator.TestValidate(model);
          validator.ShouldHaveValidationErrorFor(c => c.Description)
              .WithErrorMessage($"'Description' length cannot be greater than {DescriptionMaxLength}.");
      }

      [Fact]
      public void UpdateCategory_DescriptionTooShort_ShouldHaveError()
      {
          var model = new UpdateCategory { Description = "12" };
          var validator = _updateCategoryValidator.TestValidate(model);
          validator.ShouldHaveValidationErrorFor(c => c.Description)
              .WithErrorMessage($"'Description' length must be greater than {MinimumLength}.");
      }

      [Fact]
      public void UpdateCategory_InvalidNames_ShouldHaveError()
      {
          var invalidNames = new List<string> { null, string.Empty, " " };

          foreach (var invalidName in invalidNames)
          {
              var model = new UpdateCategory { Name = invalidName };
              var validator = _updateCategoryValidator.TestValidate(model);
              validator.ShouldHaveValidationErrorFor(c => c.Name)
                  .WithErrorMessage("'Name' must not be null or empty.");
          }
      }

      [Fact]
      public void UpdateCategory_NameTooLong_ShouldHaveError()
      {
          var model = new UpdateCategory { Name = new string('a', 51) };
          var validator = _updateCategoryValidator.TestValidate(model);
          validator.ShouldHaveValidationErrorFor(c => c.Name)
              .WithErrorMessage($"'Name' length cannot be greater than {NameMaxLength}.");
      }

      [Fact]
      public void UpdateCategory_NameTooShort_ShouldHaveError()
      {
          var model = new UpdateCategory { Name = "12" };
          var validator = _updateCategoryValidator.TestValidate(model);
          validator.ShouldHaveValidationErrorFor(c => c.Name)
              .WithErrorMessage($"'Name' length must be greater than {MinimumLength}.");
      }

   }
}
