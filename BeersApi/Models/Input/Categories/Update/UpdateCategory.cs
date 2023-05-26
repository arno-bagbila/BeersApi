using Domain.Entities;
using FluentValidation;

namespace BeersApi.Models.Input.Categories.Update
{
   /// <summary>
   /// Values to update <see cref="Category"/>
   /// </summary>
   public class UpdateCategory : IEntity
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

   public class UpdateCategoryValidator : AbstractValidator<UpdateCategory>
   {

      private const int DescriptionMaxLength = 3000;
      private const int NameMaxLength = 50;
      private const int MinimumLength = 3;
      public UpdateCategoryValidator()
      {
         RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage(GetNullOrEmptyErrorMessage(nameof(UpdateCategory.Name)))
            .MinimumLength(3)
            .WithMessage(GetMinimumLengthErrorMessage(nameof(UpdateCategory.Name), MinimumLength))
            .MaximumLength(50)
            .WithMessage(GetMaximumLengthErrorMessage(nameof(UpdateCategory.Name), NameMaxLength));

         RuleFor(c => c.Description)
            .NotEmpty()
            .WithMessage(GetNullOrEmptyErrorMessage(nameof(UpdateCategory.Description)))
            .MinimumLength(3)
            .WithMessage(GetMinimumLengthErrorMessage(nameof(UpdateCategory.Description), MinimumLength))
            .MaximumLength(3000)
            .WithMessage(GetMaximumLengthErrorMessage(nameof(UpdateCategory.Description), DescriptionMaxLength));
      }

      private static string GetNullOrEmptyErrorMessage(string propertyName) => $"'{propertyName}' must not be null or empty.";

      private static string GetMinimumLengthErrorMessage(string propertyName, int minimumLength) =>
         $"'{propertyName}' length must be greater than {minimumLength}.";

      private static string GetMaximumLengthErrorMessage(string propertyName, int maximumLength) =>
         $"'{propertyName}' length cannot be greater than {maximumLength}.";
   }
}
