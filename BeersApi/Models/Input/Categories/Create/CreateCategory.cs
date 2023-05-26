using Domain.Entities;
using FluentValidation;

namespace BeersApi.Models.Input.Categories.Create
{
   /// <summary>
   /// Values to create <see cref="Category"/>
   /// </summary>
   public class CreateCategory : IEntity
   {
      /// <summary>
      /// Category name
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Category description
      /// </summary>
      public string Description { get; set; }
   }

   public class CreateCategoryValidator : AbstractValidator<CreateCategory>
   {
      private const int DescriptionMaxLength = 3000;
      private const int NameMaxLength = 50;
      private const int MinimumLength = 3;

      public CreateCategoryValidator()
      {
         RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage(GetNullOrEmptyErrorMessage(nameof(CreateCategory.Name)))
            .MinimumLength(MinimumLength)
            .WithMessage(GetMinimumLengthErrorMessage(nameof(CreateCategory.Name), MinimumLength))
            .MaximumLength(NameMaxLength)
            .WithMessage(GetMaximumLengthErrorMessage(nameof(CreateCategory.Name), NameMaxLength));

         RuleFor(c => c.Description)
            .NotEmpty()
            .WithMessage(GetNullOrEmptyErrorMessage(nameof(CreateCategory.Description)))
            .MinimumLength(MinimumLength)
            .WithMessage(GetMinimumLengthErrorMessage(nameof(CreateCategory.Description), MinimumLength))
            .MaximumLength(DescriptionMaxLength)
            .WithMessage(GetMaximumLengthErrorMessage(nameof(CreateCategory.Description), DescriptionMaxLength));
      }

      private static string GetNullOrEmptyErrorMessage(string propertyName) => $"'{propertyName}' must not be null or empty.";

      private static string GetMinimumLengthErrorMessage(string propertyName, int minimumLength) =>
         $"'{propertyName}' length must be greater than {minimumLength}.";

      private static string GetMaximumLengthErrorMessage(string propertyName, int maximumLength) =>
         $"'{propertyName}' length cannot be greater than {maximumLength}.";
   }
}
