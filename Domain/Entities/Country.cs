using Domain.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
   public class Country
   {
      public Country()
      {
         UId = Guid.NewGuid();
      }

      #region Data

      private string _name;
      private string _code;

      #endregion

      /// <summary>
      /// Country Id
      /// </summary>
      public int Id { get; private set; }

      /// <summary>
      /// Country unique Id
      /// </summary>
      public Guid UId { get; private set; }

      /// <summary>
      /// Country name
      /// </summary>
      public string Name
      {
         get => _name;
         set
         {
            value.ThrowIfNullOrWhiteSpace(nameof(Name));
            _name = value;
         }
      }

      /// <summary>
      /// Country code
      /// </summary>
      public string Code
      {
         get => _code;
         set
         {
            value.ThrowIfNullOrWhiteSpace(nameof(Code));
            _code = value;
         }

      }

      /// <summary>
      /// Check that we can create an <see cref="Country"/>
      /// </summary>
      /// <param name="name"></param>
      /// <param name="countryCode"></param>
      /// <returns><see cref="ValidationResult"/></returns>
      public static ValidationResult CanCreate(string name, string countryCode)
      {
         var errors = new List<(string Name, string Msg)>();

         name.CheckMandatory(nameof(Name), errors);
         countryCode.CheckMandatory(nameof(Code), errors);

         return errors.ToValidationResult();
      }

      /// <summary>
      /// Create a <see cref="Country"/>
      /// </summary>
      /// <param name="name"></param>
      /// <param name="countryCode"></param>
      /// <returns><see cref="Country"/></returns>
      public static Country Create(string name, string countryCode)
      {
         var validationResult = CanCreate(name, countryCode);
         if (validationResult != ValidationResult.Success)
            throw BeersApiException.Create(BeersApiException.InvalidDataCode, validationResult.ErrorMessage,
               validationResult.MemberNames);

         return new Country
         {
            Name = name,
            Code = countryCode
         };
      }
   }
}
