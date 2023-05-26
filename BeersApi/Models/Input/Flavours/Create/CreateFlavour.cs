using Domain.Entities;
using FluentValidation;

namespace BeersApi.Models.Input.Flavours.Create
{
   /// <summary>
   /// Values to create a <see cref="Flavour"/>
   /// </summary>
   public class CreateFlavour : IEntity
   {
      /// <summary>
      /// Flavour name
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Flavour description
      /// </summary>
      public string Description { get; set; }
   }

   public class CreateFlavourValidator : AbstractValidator<CreateFlavour>
   {
      private const int DescriptionMaxLength = 3000;
      private const int NameMaxLength = 50;
      private const int MinimumLength = 3;

      public CreateFlavourValidator()
      {
         RuleFor(f => f.Name)
            .NotEmpty()
            .WithMessage(GetNullOrEmptyErrorMessage(nameof(CreateFlavour.Name)))
            .MaximumLength(NameMaxLength)
            .WithMessage(GetMaximumLengthErrorMessage(nameof(CreateFlavour.Name), NameMaxLength));

         RuleFor(f => f.Description)
            .NotEmpty()
            .WithMessage(GetNullOrEmptyErrorMessage(nameof(CreateFlavour.Description)))
            .MaximumLength(DescriptionMaxLength)
            .WithMessage(GetMaximumLengthErrorMessage(nameof(CreateFlavour.Description), DescriptionMaxLength))
            .MinimumLength(MinimumLength)
            .WithMessage(GetMinimumLengthErrorMessage(nameof(CreateFlavour.Description), MinimumLength));
      }

      private static string GetNullOrEmptyErrorMessage(string propertyName) => $"'{propertyName}' must not be null or empty.";

      private static string GetMinimumLengthErrorMessage(string propertyName, int minimumLength) =>
         $"'{propertyName}' length must be greater than {minimumLength}.";

      private static string GetMaximumLengthErrorMessage(string propertyName, int maximumLength) =>
         $"'{propertyName}' length cannot be greater than {maximumLength}.";
   }
}
