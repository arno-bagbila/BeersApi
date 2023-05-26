using Domain.Entities;
using FluentValidation;

namespace BeersApi.Models.Input.Flavours.Update
{
   /// <summary>
   /// Values to update <see cref="Flavour"/>
   /// </summary>
   public class UpdateFlavour : IEntity
   {

      /// <summary>
      /// Flavour name
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// description of the flavour
      /// </summary>
      public string Description { get; set; }
   }

   public class UpdateFlavourValidator : AbstractValidator<UpdateFlavour>
   {
      private const int DescriptionMaxLength = 3000;
      private const int NameMaxLength = 50;
      private const int MinimumLength = 3;

      public UpdateFlavourValidator()
      {
         RuleFor(u => u.Name)
            .NotEmpty()
            .WithMessage(GetNullOrEmptyErrorMessage(nameof(UpdateFlavour.Name)))
            .MaximumLength(50)
            .WithMessage(GetMaximumLengthErrorMessage(nameof(UpdateFlavour.Name), NameMaxLength));

         RuleFor(u => u.Description)
            .NotEmpty()
            .WithMessage(GetNullOrEmptyErrorMessage(nameof(UpdateFlavour.Description)))
            .MinimumLength(3)
            .WithMessage(GetMinimumLengthErrorMessage(nameof(UpdateFlavour.Description), MinimumLength))
            .MaximumLength(3000)
            .WithMessage(GetMaximumLengthErrorMessage(nameof(UpdateFlavour.Description), DescriptionMaxLength));
      }

      private static string GetNullOrEmptyErrorMessage(string propertyName) => $"'{propertyName}' must not be null or empty.";

      private static string GetMinimumLengthErrorMessage(string propertyName, int minimumLength) =>
         $"'{propertyName}' length must be greater than {minimumLength}.";

      private static string GetMaximumLengthErrorMessage(string propertyName, int maximumLength) =>
         $"'{propertyName}' length cannot be greater than {maximumLength}.";
   }
}
