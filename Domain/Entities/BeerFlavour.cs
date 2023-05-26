using Domain.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
   public class BeerFlavour
   {
      public BeerFlavour() { }

      #region Data

      private Beer _beer;
      private Flavour _flavour;

      #endregion


      public int BeerId { get; private set; }

      public Beer Beer
      {
         get => _beer;
         set
         {
            value.ThrowIfNull(nameof(Beer));
            _beer = value;
         }
      }

      public int FlavourId { get; private set; }

      public Flavour Flavour
      {
         get => _flavour;
         set
         {
            value.ThrowIfNull(nameof(Flavour));
            _flavour = value;
         }
      }

      /// <summary>
      /// Check if we can create a <see cref="BeerFlavour"/>
      /// </summary>
      /// <param name="beer">the beer object</param>
      /// <param name="flavour">the flavour object</param>
      /// <returns> <see cref="ValidationResult"/></returns>
      public static ValidationResult CanCreate(Beer beer, Flavour flavour)
      {
         var errors = new List<(string Name, string Msg)>();

         beer.CheckMandatory(nameof(Beer), errors);
         flavour.CheckMandatory(nameof(Flavour), errors);

         return errors.ToValidationResult();
      }

      /// <summary>
      /// Create a <see cref="BeerFlavour"/>
      /// </summary>
      /// <param name="beer">beer</param>
      /// <param name="flavour">flavour</param>
      /// <returns></returns>
      public static BeerFlavour Create(Beer beer, Flavour flavour)
      {
         var validationResult = CanCreate(beer, flavour);
         if (validationResult != ValidationResult.Success)
            throw BeersApiException.Create(BeersApiException.InvalidDataCode, validationResult.ErrorMessage,
               validationResult.MemberNames);

         return new BeerFlavour
         {
            Beer = beer,
            BeerId = beer.Id,
            Flavour = flavour,
            FlavourId = flavour.Id
         };
      }
   }
}
