using Domain.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
   public class Flavour
   {
      public Flavour()
      {
         UId = Guid.NewGuid();
      }

      #region Data

      private string _name;
      private string _description;

      #endregion

      /// <summary>
      /// Flavour Id
      /// </summary>
      public int Id { get; private set; }

      /// <summary>
      /// Name of the flavour
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
      /// description of the flavour
      /// </summary>
      public string Description
      {
         get => _description;
         set
         {
            value.ThrowIfNullOrWhiteSpace(nameof(Description));
            _description = value;
         }
      }

      /// <summary>
      /// Flavour unique identifier
      /// </summary>
      public Guid UId { get; set; }

      /// <summary>
      /// Check if Flavour can be created
      /// </summary>
      /// <param name="name"></param>
      /// <param name="description"></param>
      /// <returns> <see cref="ValidationResult"/></returns>
      public static ValidationResult CanCreate(string name, string description)
      {
         var errors = new List<(string Name, string Msg)>();

         name.CheckMandatory(nameof(Name), errors);
         description.CheckMandatory(nameof(Description), errors);

         return errors.ToValidationResult();
      }

      /// <summary>
      /// Create a Flavour object
      /// </summary>
      /// <param name="name"></param>
      /// <param name="description"></param>
      /// <returns>a <see cref="Flavour"/></returns>
      public static Flavour Create(string name, string description)
      {
         var validationResult = CanCreate(name, description);
         if (validationResult != ValidationResult.Success)
         {
            throw BeersApiException.Create(BeersApiException.InvalidDataCode, validationResult.ErrorMessage,
               validationResult.MemberNames);
         }

         return new Flavour
         {
            Name = name,
            Description = description
         };
      }
   }
}
