using BeersApi.Models.Input.Beers.Update;
using FluentValidation.TestHelper;
using System.Collections.Generic;
using Xunit;

namespace BeersApi.Tests.Models.Input.Beers.Update
{
   public class UpdateBeerTests
   {

      private readonly UpdateBeerValidator _updateBeerValidator = new();
      private const int DescriptionMaxLength = 3000;
      private const int NameMaxLength = 50;
      private const int MinimumLength = 3;
      private const int UrlMaxLength = 2048;

      [Fact]
      public void UpdateBeer_InvalidNames_ShouldHaveError()
      {
         var invalidNames = new List<string> { null, string.Empty, " " };

         foreach (var invalidName in invalidNames)
         {
            var model = new UpdateBeer { Name = invalidName };
            var validator = _updateBeerValidator.TestValidate(model);
            validator.ShouldHaveValidationErrorFor(c => c.Name)
               .WithErrorMessage("'Name' must not be null or empty.");
         }
      }

      [Fact]
      public void UpdateBeer_NameTooLong_ShouldHaveError()
      {
         var model = new UpdateBeer { Name = new string('a', 51) };
         var validator = _updateBeerValidator.TestValidate(model);
         validator.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage($"The length of 'Name' must be {NameMaxLength} characters or fewer. You entered {model.Name.Length} characters.");
      }

      [Fact]
      public void UpdateBeer_NameTooShort_ShouldHaveError()
      {
         var model = new UpdateBeer { Name = "12" };
         var validator = _updateBeerValidator.TestValidate(model);
         validator.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage($"The length of 'Name' must be at least {MinimumLength} characters. You entered {model.Name.Length} characters.");
      }

      [Fact]
      public void UpdateBeer_InvalidDescriptions_ShouldHaveError()
      {
         var invalidDescriptions = new List<string> { null, string.Empty, " " };

         foreach (var invalidDescription in invalidDescriptions)
         {
            var model = new UpdateBeer { Description = invalidDescription };
            var validator = _updateBeerValidator.TestValidate(model);
            validator.ShouldHaveValidationErrorFor(c => c.Description)
               .WithErrorMessage("'Description' must not be null or empty.");
         }
      }

      [Fact]
      public void UpdateBeer_DescriptionTooLong_ShouldHaveError()
      {
         var model = new UpdateBeer { Description = new string('a', 3001) };
         var validator = _updateBeerValidator.TestValidate(model);
         validator.ShouldHaveValidationErrorFor(c => c.Description)
            .WithErrorMessage($"The length of 'Description' must be {DescriptionMaxLength} characters or fewer. You entered {model.Description.Length} characters.");
      }

      [Fact]
      public void UpdateBeer_DescriptionTooShort_ShouldHaveError()
      {
         var model = new UpdateBeer { Description = "12" };
         var validator = _updateBeerValidator.TestValidate(model);
         validator.ShouldHaveValidationErrorFor(c => c.Description)
            .WithErrorMessage($"The length of 'Description' must be at least {MinimumLength} characters. You entered {model.Description.Length} characters.");
      }

      [Fact]
      public void UpdateBeer_EmptyLogoUrl_ShouldHaveError()
      {
         var invalidLogoUrls = new List<string> { null, string.Empty, " " };

         foreach (var invalidLogoUrl in invalidLogoUrls)
         {
            var model = new UpdateBeer { LogoUrl = invalidLogoUrl };
            var validator = _updateBeerValidator.TestValidate(model);
            validator.ShouldHaveValidationErrorFor(c => c.LogoUrl)
               .WithErrorMessage("'LogoUrl' must not be null or empty.");
         }
      }

      [Fact]
      public void UpdateBeer_LogoUrlTooLong_ShouldHaveError()
      {
         var model = new UpdateBeer { LogoUrl = new string('a', 2049) };
         var validator = _updateBeerValidator.TestValidate(model);
         validator.ShouldHaveValidationErrorFor(c => c.LogoUrl)
            .WithErrorMessage($"The length of 'Logo Url' must be {UrlMaxLength} characters or fewer. You entered {model.LogoUrl.Length} characters.");
      }

      [Fact]
      public void UpdateBeer_InvalidLogoUrl_ShouldHaveError()
      {
         var model = new UpdateBeer { LogoUrl = "LogoUrl" };
         var validator = _updateBeerValidator.TestValidate(model);
         validator.ShouldHaveValidationErrorFor(c => c.LogoUrl);
      }

      [Fact]
      public void UpdateBeer_TiwooRatingGreaterThan5_ShouldHaveError()
      {
         var model = new UpdateBeer { TiwooRating = 5.1 };
         var validator = _updateBeerValidator.TestValidate(model);
         validator.ShouldHaveValidationErrorFor(c => c.TiwooRating)
            .WithErrorMessage("'Tiwoo Rating' must be less than or equal to '5'.");
      }

      [Fact]
      public void UpdateBeer_TiwooRatingLessThan0_ShouldHaveError()
      {
         var model = new UpdateBeer { TiwooRating = -0.1 };
         var validator = _updateBeerValidator.TestValidate(model);
         validator.ShouldHaveValidationErrorFor(c => c.TiwooRating)
            .WithErrorMessage("'Tiwoo Rating' must be greater than '0'.");
      }

      [Fact]
      public void UpdateBeer_TiwooRatingEmptyValue_ShouldHaveError()
      {
         var model = new UpdateBeer { TiwooRating = 0.0 };
         var validator = _updateBeerValidator.TestValidate(model);
         validator.ShouldHaveValidationErrorFor(c => c.TiwooRating)
            .WithErrorMessage("'Tiwoo Rating' must be greater than '0'.");
      }

      [Fact]
      public void UpdateBeer_AlcoholLevelGreaterThan100_ShouldHaveError()
      {
         var model = new UpdateBeer { AlcoholLevel = 100.1 };
         var validator = _updateBeerValidator.TestValidate(model);
         validator.ShouldHaveValidationErrorFor(c => c.AlcoholLevel)
            .WithErrorMessage("'Alcohol Level' must be less than or equal to '100'.");
      }

      [Fact]
      public void UpdateBeer_AlcoholLevelLessThanZero_ShouldHaveError()
      {
         var model = new UpdateBeer { AlcoholLevel = -0.1 };
         var validator = _updateBeerValidator.TestValidate(model);
         validator.ShouldHaveValidationErrorFor(c => c.AlcoholLevel)
            .WithErrorMessage("'Alcohol Level' must be greater than '0'.");
      }

      [Fact]
      public void UpdateBeer_InvalidCategoryId_ShouldHaveError()
      {
          var model = new UpdateBeer { CategoryId = -0 };
          var validator = _updateBeerValidator.TestValidate(model);
          validator.ShouldHaveValidationErrorFor(c => c.CategoryId)
              .WithErrorMessage("'Category Id' must not be empty.");
      }

      [Fact]
      public void UpdateBeer_InvalidColorId_ShouldHaveError()
      {
          var model = new UpdateBeer { ColorId = -0 };
            var validator = _updateBeerValidator.TestValidate(model);
            validator.ShouldHaveValidationErrorFor(c => c.ColorId)
                .WithErrorMessage("'Color Id' must not be empty.");
      }

      [Fact]
      public void UpdateBeer_InvalidCountryId_ShouldHaveError()
      {
          var model = new UpdateBeer { CountryId = -0 };
          var validator = _updateBeerValidator.TestValidate(model);
            validator.ShouldHaveValidationErrorFor(c => c.CountryId)
                .WithErrorMessage("'Country Id' must not be empty.");
      }

      [Fact]
      public void UpdateBeer_InvalidFlavourIds_ShouldHaveError()
      {
          var model = new UpdateBeer { FlavourIds = new List<int>() };
            var validator = _updateBeerValidator.TestValidate(model);
                validator.ShouldHaveValidationErrorFor(c => c.FlavourIds)
                    .WithErrorMessage("'Flavour Ids' must not be empty.");
      }
   }
}
