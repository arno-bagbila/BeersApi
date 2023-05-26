using BeersApi.Models.Input.Flavours.Create;
using FluentValidation.TestHelper;
using System.Collections.Generic;
using Xunit;

namespace BeersApi.Tests.Models.Input.Flavours.Create
{
   public class CreateFlavourTests
   {
      private const int DescriptionMaxLength = 3000;
      private const int NameMaxLength = 50;
      private const int MinimumLength = 3;
      private readonly CreateFlavourValidator _createFlavourValidator;

      public static IEnumerable<object[]> GetInvalidNames()
      {
         yield return new object[] { null, string.Empty, " ", new string('a', 51) };
      }

      public static IEnumerable<object[]> GetInvalidDescription()
      {
         yield return new object[] { null, string.Empty, " ", new string('a', 3001), "12" };
      }

      public CreateFlavourTests()
      {
         _createFlavourValidator = new CreateFlavourValidator();
      }

      [Theory]
      [MemberData(nameof(GetInvalidDescription))]
      public void CreateFlavour_InvalidDescriptions_ShouldHaveError(string isNull, string empty, string whiteSpace, string maxLength, string minLength)
      {
         _createFlavourValidator.ShouldHaveValidationErrorFor(c => c.Description, isNull)
            .WithErrorMessage("'Description' must not be null or empty.")
            .WithErrorCode("NotEmptyValidator");

         _createFlavourValidator.ShouldHaveValidationErrorFor(c => c.Description, empty)
            .WithErrorMessage("'Description' must not be null or empty.")
            .WithErrorCode("NotEmptyValidator");

         _createFlavourValidator.ShouldHaveValidationErrorFor(c => c.Description, whiteSpace)
            .WithErrorMessage("'Description' must not be null or empty.")
            .WithErrorCode("NotEmptyValidator");

         _createFlavourValidator.ShouldHaveValidationErrorFor(c => c.Description, maxLength)
            .WithErrorMessage($"'Description' length cannot be greater than {DescriptionMaxLength}.");

         _createFlavourValidator.ShouldHaveValidationErrorFor(c => c.Description, minLength)
            .WithErrorMessage($"'Description' length must be greater than {MinimumLength}.");
      }

      [Theory]
      [MemberData(nameof(GetInvalidNames))]
      public void CreateFlavour_InvalidNames_ShouldHaveError(string isNull, string empty, string whiteSpace, string maxLength)
      {
         _createFlavourValidator.ShouldHaveValidationErrorFor(c => c.Name, isNull)
            .WithErrorMessage("'Name' must not be null or empty.")
            .WithErrorCode("NotEmptyValidator");

         _createFlavourValidator.ShouldHaveValidationErrorFor(c => c.Name, empty)
            .WithErrorMessage("'Name' must not be null or empty.")
            .WithErrorCode("NotEmptyValidator");

         _createFlavourValidator.ShouldHaveValidationErrorFor(c => c.Name, whiteSpace)
            .WithErrorMessage("'Name' must not be null or empty.")
            .WithErrorCode("NotEmptyValidator");

         _createFlavourValidator.ShouldHaveValidationErrorFor(c => c.Name, maxLength)
            .WithErrorMessage($"'Name' length cannot be greater than {NameMaxLength}.");
      }
   }
}
