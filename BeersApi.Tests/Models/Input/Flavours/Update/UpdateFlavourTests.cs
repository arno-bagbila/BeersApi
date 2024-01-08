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

        private readonly UpdateFlavourValidator _updateFlavourValidator = new();


        [Fact]
        public void UpdateFlavour_InvalidNames_ShouldHaveError()
        {
            var invalidNames = new List<string> { null, string.Empty, " ", new('a', NameMaxLength + 1) };

            foreach (var invalidName in invalidNames)
            {
                var model = new UpdateFlavour { Name = invalidName };
                var validator = _updateFlavourValidator.TestValidate(model);
                validator.ShouldHaveValidationErrorFor(c => c.Name);
            }
        }

        [Fact]
        public void UpdateFlavour_NameTooLong_ShouldHaveError()
        {
            var model = new UpdateFlavour { Name = new string('a', NameMaxLength + 1) };
            var validator = _updateFlavourValidator.TestValidate(model);
            validator.ShouldHaveValidationErrorFor(c => c.Name)
                .WithErrorMessage($"'Name' length cannot be greater than {NameMaxLength}.");
        }

        [Fact]
        public void UpdateFlavour_DescriptionTooLong_ShouldHaveError()
        {
            var model = new UpdateFlavour { Description = new string('a', DescriptionMaxLength + 1) };
            var validator = _updateFlavourValidator.TestValidate(model);
            validator.ShouldHaveValidationErrorFor(c => c.Description)
                .WithErrorMessage($"'Description' length cannot be greater than {DescriptionMaxLength}.");
        }

        [Fact]
        public void UpdateFlavour_DescriptionTooShort_ShouldHaveError()
        {
            var model = new UpdateFlavour { Description = "12" };
            var validator = _updateFlavourValidator.TestValidate(model);
            validator.ShouldHaveValidationErrorFor(c => c.Description)
                  .WithErrorMessage($"'Description' length must be greater than {MinimumLength}.");
        }

        [Fact]
        public void UpdateFlavour_InvalidDescriptions_ShouldHaveError()
        {
            var invalidDescriptions = new List<string> { null, string.Empty, " ", new('a', 3001), "12" };

            foreach (var invalidDescription in invalidDescriptions)
            {
                var model = new UpdateFlavour { Description = invalidDescription };
                var validator = _updateFlavourValidator.TestValidate(model);
                validator.ShouldHaveValidationErrorFor(c => c.Description);
            }
        }
    }
}
