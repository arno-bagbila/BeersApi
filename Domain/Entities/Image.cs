using Domain.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
   public class Image
   {

      public Image()
      {
         UId = Guid.NewGuid();
         DateRegistered = DateTime.Now;
      }

      #region Data

      private string _title;
      private string _imageUrl;
      private Beer _beer;


      #endregion

      /// <summary>
      /// Country Id
      /// </summary>
      public int Id { get; private set; }

      /// <summary>
      /// Image unique identifier
      /// </summary>
      public Guid UId { get; private set; }

      /// <summary>
      /// Image title
      /// </summary>
      public string Title
      {
         get => _title;
         set
         {
            value.ThrowIfNullOrWhiteSpace(nameof(Title));
            _title = value;
         }
      }

      /// <summary>
      /// image url to azure
      /// </summary>
      public string ImageUrl
      {
         get => _imageUrl;
         set
         {
            value.ThrowIfNullOrWhiteSpace(nameof(ImageUrl));
            _imageUrl = value;
         }
      }

      public Beer Beer
      {
         get => _beer;
         set
         {
            value.ThrowIfNull(nameof(Beer));
            _beer = value;
         }
      }

      public DateTime DateRegistered { get; private set; }

      /// <summary>
      /// Check if Image can be created
      /// </summary>
      /// <param name="title"></param>
      /// <param name="imageUrl"></param>
      /// <param name="beer">beer to which the image belongs</param>
      /// <returns> <see cref="ValidationResult"/></returns>
      public static ValidationResult CanCreate(string title, string imageUrl, Beer beer)
      {
         var errors = new List<(string Name, string Msg)>();

         title.CheckMandatory(nameof(Title), errors);
         imageUrl.CheckMandatory(nameof(ImageUrl), errors);
         beer.CheckMandatory(nameof(Beer), errors);

         return errors.ToValidationResult();
      }

      /// <summary>
      /// Create Image
      /// </summary>
      /// <param name="title"></param>
      /// <param name="imageUrl"></param>
      /// <param name="beer">beer to which the image belongs</param>
      /// <returns></returns>
      public static Image Create(string title, string imageUrl, Beer beer)
      {
         var validationResult = CanCreate(title, imageUrl, beer);

         if (validationResult != ValidationResult.Success)
            throw BeersApiException.Create(BeersApiException.InvalidDataCode, validationResult.ErrorMessage,
               validationResult.MemberNames);

         return new Image
         {
            Title = title,
            ImageUrl = imageUrl,
            Beer = beer
         };
      }
   }
}
