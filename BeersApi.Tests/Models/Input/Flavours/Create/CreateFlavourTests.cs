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

      private readonly CreateFlavourValidator _createFlavourValidator = new();



      [Fact]
      public void CreateFlavour_InvalidNames_ShouldHaveError()
      {
          var invalidNames = new List<string> { null, string.Empty, " ", new('a', NameMaxLength + 1) };

          foreach (var invalidName in invalidNames)
          {
              var model = new CreateFlavour { Name = invalidName };
              var validator = _createFlavourValidator.TestValidate(model);
              validator.ShouldHaveValidationErrorFor(c => c.Name);
          }
      }

      [Fact]
      public void CreateFlavour_NameTooLong_ShouldHaveError()
      {
          var model = new CreateFlavour { Name = new string('a', NameMaxLength + 1) };
          var validator = _createFlavourValidator.TestValidate(model);
          validator.ShouldHaveValidationErrorFor(c => c.Name)
              .WithErrorMessage($"'Name' length cannot be greater than {NameMaxLength}.");
      }

      [Fact]
      public void CreateFlavour_DescriptionTooLong_ShouldHaveError()
      { 
          var model = new CreateFlavour { Description = new string('a', DescriptionMaxLength + 1) };
          var validator = _createFlavourValidator.TestValidate(model);
          validator.ShouldHaveValidationErrorFor(c => c.Description)
              .WithErrorMessage($"'Description' length cannot be greater than {DescriptionMaxLength}.");
      }

      [Fact]
      public void CreateFlavour_DescriptionTooShort_ShouldHaveError()
      {
          var model = new CreateFlavour { Description = "12" };
          var validator = _createFlavourValidator.TestValidate(model);
          validator.ShouldHaveValidationErrorFor(c => c.Description)
                .WithErrorMessage($"'Description' length must be greater than {MinimumLength}.");
      }

      [Fact]
      public void CreateFlavour_InvalidDescriptions_ShouldHaveError()
      {
          var invalidDescriptions = new List<string> { null, string.Empty, " ", new('a', DescriptionMaxLength + 1) };

          foreach (var invalidDescription in invalidDescriptions)
          {
              var model = new CreateFlavour { Description = invalidDescription };
              var validator = _createFlavourValidator.TestValidate(model);
              validator.ShouldHaveValidationErrorFor(c => c.Description);
          }
      }
   }
}
