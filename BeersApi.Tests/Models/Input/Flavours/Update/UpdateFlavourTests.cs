using BeersApi.Models.Input.Flavours.Update;
using FluentValidation.TestHelper;
using System.Collections.Generic;
using Xunit;

namespace BeersApi.Tests.Models.Input.Flavours.Update
{
   public class UpdateFlavourTests
   {
      private const int DescriptionMaxLength = 3000;
      private const int NameMaxLength = 50;
      private const int MinimumLength = 3;
      private readonly UpdateFlavourValidator _updateFlavourValidator;

      public UpdateFlavourTests()
      {
         _updateFlavourValidator = new UpdateFlavourValidator();
      }

      public static IEnumerable<object[]> GetInvalidNames()
      {
         yield return new object[] { null, string.Empty, " ", new string('a', 51) };
      }

      public static IEnumerable<object[]> GetInvalidDescription()
      {
         yield return new object[] { null, string.Empty, " ", new string('a', 3001), "12" };
      }

      [Theory]
      [MemberData(nameof(GetInvalidDescription))]
      public void CreateFlavour_InvalidDescriptions_ShouldHaveError(string isNull, string empty, string whiteSpace, string maxLength, string minLength)
      {
         _updateFlavourValidator.ShouldHaveValidationErrorFor(u => u.Description, isNull)
            .WithErrorMessage("'Description' must not be null or empty.")
            .WithErrorCode("NotEmptyValidator");

         _updateFlavourValidator.ShouldHaveValidationErrorFor(u => u.Description, empty)
            .WithErrorMessage("'Description' must not be null or empty.")
            .WithErrorCode("NotEmptyValidator");

         _updateFlavourValidator.ShouldHaveValidationErrorFor(u => u.Description, whiteSpace)
            .WithErrorMessage("'Description' must not be null or empty.")
            .WithErrorCode("NotEmptyValidator");

         _updateFlavourValidator.ShouldHaveValidationErrorFor(u => u.Description, maxLength)
            .WithErrorMessage($"'Description' length cannot be greater than {DescriptionMaxLength}.");

         _updateFlavourValidator.ShouldHaveValidationErrorFor(u => u.Description, minLength)
            .WithErrorMessage($"'Description' length must be greater than {MinimumLength}.");
      }

      [Theory]
      [MemberData(nameof(GetInvalidNames))]
      public void CreateFlavour_InvalidNames_ShouldHaveError(string isNull, string empty, string whiteSpace, string maxLength)
      {
         _updateFlavourValidator.ShouldHaveValidationErrorFor(c => c.Name, isNull)
            .WithErrorMessage("'Name' must not be null or empty.")
            .WithErrorCode("NotEmptyValidator");

         _updateFlavourValidator.ShouldHaveValidationErrorFor(c => c.Name, empty)
            .WithErrorMessage("'Name' must not be null or empty.")
            .WithErrorCode("NotEmptyValidator");

         _updateFlavourValidator.ShouldHaveValidationErrorFor(c => c.Name, whiteSpace)
            .WithErrorMessage("'Name' must not be null or empty.")
            .WithErrorCode("NotEmptyValidator");

         _updateFlavourValidator.ShouldHaveValidationErrorFor(c => c.Name, maxLength)
            .WithErrorMessage($"'Name' length cannot be greater than {NameMaxLength}.");
      }
   }
}
