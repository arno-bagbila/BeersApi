using FluentValidation;

namespace BeersApi.Models.Input.Beers.Create
{
   public class BeersFilter
   {
      public int[] CategoryIds { get; set; }

      public int[] ColorIds { get; set; }

      public int[] CountryIds { get; set; }
   }

   public class BeersFilterValidator : AbstractValidator<BeersFilter>
   {
      public BeersFilterValidator()
      {

      }
   }
}
