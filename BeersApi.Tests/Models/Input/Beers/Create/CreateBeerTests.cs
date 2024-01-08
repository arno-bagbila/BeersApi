using BeersApi.Models.Input.Beers.Create;
using FluentValidation.TestHelper;
using System.Collections.Generic;
using Xunit;

namespace BeersApi.Tests.Models.Input.Beers.Create
{
   public class CreateBeerTests
   {

      private const int DescriptionMaxLength = 3000;
      private const int NameMaxLength = 50;
      private const int MinimumLength = 3;
      private const int UrlMaxLength = 2048;

      private readonly CreateBeerValidator _createBeerValidator = new();

      [Fact]
      public void CreateBeer_InvalidNames_ShouldHaveError()
      {
         var invalidNames = new List<string> { null, string.Empty, " ", new string('a', 51), "12" };

         foreach (var invalidName in invalidNames)
         {
            var model = new CreateBeer { Name = invalidName };
            var validator = _createBeerValidator.TestValidate(model);
            validator.ShouldHaveValidationErrorFor(c => c.Name);
         }
      }

      [Fact]
      public void CreateBeer_NameTooLong_ShouldHaveError()
      {
          var model = new CreateBeer { Name = new string('a', 51) };
          var validator = _createBeerValidator.TestValidate(model);
          validator.ShouldHaveValidationErrorFor(c => c.Name)
              .WithErrorMessage($"The length of 'Name' must be {NameMaxLength} characters or fewer. You entered {model.Name.Length} characters.");
      }

      [Fact]
      public void CreateBeer_NameTooShort_ShouldHaveError()
      {
          var model = new CreateBeer { Name = "12" };
          var validator = _createBeerValidator.TestValidate(model);
          validator.ShouldHaveValidationErrorFor(c => c.Name)
              .WithErrorMessage($"The length of 'Name' must be at least {MinimumLength} characters. You entered {model.Name.Length} characters.");
      }

      [Fact]
      public void CreateBeer_DescriptionTooLong_ShouldHaveError()
      {
          var model = new CreateBeer { Description = new string('a', 3001) };
          var validator = _createBeerValidator.TestValidate(model);
          validator.ShouldHaveValidationErrorFor(c => c.Description)
              .WithErrorMessage($"The length of 'Description' must be {DescriptionMaxLength} characters or fewer. You entered {model.Description.Length} characters.");
      }

      [Fact]
      public void CreateBeer_DescriptionTooShort_ShouldHaveError()
      {
          var model = new CreateBeer { Description = "12" };
          var validator = _createBeerValidator.TestValidate(model);
          validator.ShouldHaveValidationErrorFor(c => c.Description)
              .WithErrorMessage($"The length of 'Description' must be at least {MinimumLength} characters. You entered {model.Description.Length} characters.");
      }

      [Fact]
      public void CreateBeer_InvalidDescriptions_ShouldHaveError()
      {
         var invalidDescriptions = new List<string> { null, string.Empty, " ", new string('a', 3001), "12" };

         foreach (var invalidDescription in invalidDescriptions)
         {
            var model = new CreateBeer { Description = invalidDescription };
            var validator = _createBeerValidator.TestValidate(model);
            validator.ShouldHaveValidationErrorFor(c => c.Description);
         }
      }

      [Fact]
      public void CreateBeer_EmptyLogoUrl_ShouldHaveError()
      {
          var invalidLogoUrls = new List<string> { null, string.Empty, " " };

          foreach (var invalidLogoUrl in invalidLogoUrls)
          {
              var model = new CreateBeer { LogoUrl = invalidLogoUrl };
              var validator = _createBeerValidator.TestValidate(model);
              validator.ShouldHaveValidationErrorFor(c => c.LogoUrl)
                  .WithErrorMessage("'LogoUrl' must not be null or empty.");
          }
      }

      [Fact]
      public void CreateBeer_LogoUrlTooLong_ShouldHaveError()
      {
          var model = new CreateBeer { LogoUrl = new string('a', 2049) };
          var validator = _createBeerValidator.TestValidate(model);
          validator.ShouldHaveValidationErrorFor(c => c.LogoUrl)
              .WithErrorMessage($"The length of 'Logo Url' must be {UrlMaxLength} characters or fewer. You entered {model.LogoUrl.Length} characters.");
      }

      [Fact]
      public void CreateBeer_InvalidLogoUrl_ShouldHaveError()
      {
          var model = new CreateBeer { LogoUrl = "LogoUrl" };
          var validator = _createBeerValidator.TestValidate(model);
          validator.ShouldHaveValidationErrorFor(c => c.LogoUrl);
      }

      [Fact]
      public void CreateBeer_InvalidTiwooRating_ShouldHaveError()
      {
         var invalidTiwooRatings = new List<double> { 5.1, -1, 0.0 };

         foreach (var invalidTiwooRating in invalidTiwooRatings)
         {
            var model = new CreateBeer { TiwooRating = invalidTiwooRating };
            var validator = _createBeerValidator.TestValidate(model);
            validator.ShouldHaveValidationErrorFor(c => c.TiwooRating);
         }
      }

      [Fact]
      public void CreateBeer_InvalidAlcoholLevel_ShouldHaveError()
      {
         var invalidAlcoholLevels = new List<double> { 0.0, 100.1, -1.0 };

         foreach (double invalidAlcoholLevel in invalidAlcoholLevels)
         {
            var model = new CreateBeer { AlcoholLevel = invalidAlcoholLevel };
            var validator = _createBeerValidator.TestValidate(model);
            validator.ShouldHaveValidationErrorFor(c => c.AlcoholLevel);
         }      
      }

      [Fact]
      public void CreateBeer_InvalidCategoryId_ShouldHaveError()
      {
         var model = new CreateBeer { CategoryId = 0 };
         var validator = _createBeerValidator.TestValidate(model);
         validator.ShouldHaveValidationErrorFor(c => c.CategoryId);
      }

      [Fact]
      public void CreateBeer_InvalidColorId_ShouldHaveError()
      {
         var model = new CreateBeer { ColorId = 0 };
         var validator = _createBeerValidator.TestValidate(model);
         validator.ShouldHaveValidationErrorFor(c => c.ColorId)
            .WithErrorCode("NotEmptyValidator")
            .WithErrorMessage("'Color Id' must not be empty.");
      }

      [Fact]
      public void CreateBeer_InvalidCountryId_ShouldHaveError()
      {
         var model = new CreateBeer { CountryId = 0 };
         var validator = _createBeerValidator.TestValidate(model);
         validator.ShouldHaveValidationErrorFor(c => c.CountryId)
            .WithErrorCode("NotEmptyValidator")
            .WithErrorMessage("'Country Id' must not be empty.");
      }

      [Fact]
      public void CreateBeer_InvalidFlavourIds_ShouldHaveError()
      {
         var model = new CreateBeer { FlavourIds = new List<int>() };
         var validator = _createBeerValidator.TestValidate(model);
         validator.ShouldHaveValidationErrorFor(c => c.FlavourIds)
            .WithErrorCode("NotEmptyValidator")
            .WithErrorMessage("'Flavour Ids' must not be empty.");
      }
   }
}
