using FluentValidation;
using System;
using System.Collections.Generic;

namespace BeersApi.Models.Input.Beers.Create
{
   public class CreateBeer : IEntity
   {

      /// <summary>
      /// Beer's name
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Beer's description
      /// </summary>
      public string Description { get; set; }

      /// <summary>
      /// Beer's alcohol level in percentage
      /// </summary>
      public double AlcoholLevel { get; set; }

      /// <summary>
      /// Rating cannot be more than 5
      /// </summary>
      public double TiwooRating { get; set; }

      /// <summary>
      /// Id of the category to which the beer belong
      /// </summary>
      public int CategoryId { get; set; }

      /// <summary>
      /// Id of the color of the beer
      /// </summary>
      public int ColorId { get; set; }

      /// <summary>
      /// Id of the country of the beer
      /// </summary>
      public int CountryId { get; set; }

      /// <summary>
      /// Ids of the beer different flavours
      /// </summary>
      public IEnumerable<int> FlavourIds { get; set; }

      /// <summary>
      /// url of the logo of the beer
      /// </summary>
      public string LogoUrl { get; set; }
   }

   public class CreateBeerValidator : AbstractValidator<CreateBeer>
   {
      private const int DescriptionMaxLength = 3000;
      private const int NameMaxLength = 50;
      private const int MinimumLength = 3;
      private const int UrlMaxLength = 2048;

      public CreateBeerValidator()
      {
         RuleFor(b => b.Name)
            .NotEmpty()
            .WithMessage(GetNullOrEmptyErrorMessage(nameof(CreateBeer.Name)))
            .MinimumLength(MinimumLength)
            .MaximumLength(NameMaxLength);

         RuleFor(b => b.Description)
            .NotEmpty()
            .WithMessage(GetNullOrEmptyErrorMessage(nameof(CreateBeer.Description)))
            .MinimumLength(MinimumLength)
            .MaximumLength(DescriptionMaxLength);

         RuleFor(b => b.AlcoholLevel)
            .NotEmpty()
            .LessThanOrEqualTo(100)
            .GreaterThanOrEqualTo(0);

         RuleFor(b => b.CategoryId)
            .NotEmpty();

         RuleFor(b => b.ColorId)
            .NotEmpty();

         RuleFor(b => b.FlavourIds)
            .NotEmpty();

         RuleFor(b => b.LogoUrl)
            .NotEmpty()
            .WithMessage(GetNullOrEmptyErrorMessage(nameof(CreateBeer.LogoUrl)))
            .Custom(((s, context) =>
            {
               if (!Uri.IsWellFormedUriString(s, UriKind.Absolute))
                  context.AddFailure($"'LogoUrl' has a wrong format. It should be an url, but you entered {s}.");
            }))
            .MaximumLength(UrlMaxLength);

         RuleFor(b => b.TiwooRating)
            .NotEmpty()
            .LessThanOrEqualTo(5)
            .GreaterThanOrEqualTo(0);

         RuleFor(b => b.CountryId)
            .NotEmpty();
      }

      private static string GetNullOrEmptyErrorMessage(string propertyName) => $"'{propertyName}' must not be null or empty.";
   }
}
