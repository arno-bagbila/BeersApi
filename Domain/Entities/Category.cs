using Domain.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
   public class Category
   {

      private Category()
      {
         UId = Guid.NewGuid();
      }

      #region Data

      private string _name;
      private string _description;

      #endregion

      /// <summary>
      /// Category Id
      /// </summary>
      public int Id { get; private set; }

      /// <summary>
      /// Category unique Id
      /// </summary>
      public Guid UId { get; private set; }

      /// <summary>
      /// Category name
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
      /// Category description
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
      /// Create a Category object
      /// </summary>
      /// <param name="name"></param>
      /// <param name="description"></param>
      /// <returns>a <see cref="Category"/></returns>
      public static Category Create(string name, string description)
      {
         return new Category
         {
            Name = name,
            Description = description
         };
      }
   }
}
