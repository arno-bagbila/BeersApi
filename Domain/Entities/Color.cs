using Domain.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
   public class Color
   {

      private Color()
      {
         UId = Guid.NewGuid();
      }

      #region Data

      private string _name;

      #endregion


      /// <summary>
      /// Color Id
      /// </summary>
      public int Id { get; private set; }

      /// <summary>
      /// Unique identifier for the color
      /// </summary>
      public Guid UId { get; private set; }

      /// <summary>
      /// Color Name
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
      /// Check if Color can be created
      /// </summary>
      /// <param name="name"></param>
      /// <returns> <see cref="ValidationResult"/></returns>
      private static ValidationResult CanCreate(string name)
      {
         var errors = new List<(string Name, string Msg)>();

         name.CheckMandatory(nameof(Name), errors);

         return errors.ToValidationResult();
      }

      /// <summary>
      /// Create a Color object
      /// </summary>
      /// <param name="name"></param>
      /// <returns>a <see cref="Color"/></returns>
      public static Color Create(string name)
      {
         var validationResult = CanCreate(name);

         if (validationResult != ValidationResult.Success)
            throw BeersApiException.Create(BeersApiException.InvalidDataCode, validationResult.ErrorMessage,
               validationResult.MemberNames);
         else
            return new Color
            {
               Name = name
            };
      }
   }
}
