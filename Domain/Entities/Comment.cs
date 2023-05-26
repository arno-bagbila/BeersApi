using Domain.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
   public class Comment
   {

      #region Data

      private string _body;
      private string _userFirstName;
      private Guid _userUId;
      private Beer _beer;
      private const int BODY_MAX_LENGTH = 3000;
      private const int USERFIRSTNAME_MAX_LENGTH = 256;

      #endregion

      public int Id { get; private set; }

      /// <summary>
      /// Comment text
      /// </summary>
      public string Body
      {
         get => _body;
         set
         {
            value.ThrowIfNullOrWhiteSpace(nameof(Body));
            _body = value;
         }
      }

      /// <summary>
      /// username of user who set the comment
      /// </summary>
      public string UserFirstName
      {
         get => _userFirstName;
         set
         {
            value.ThrowIfNullOrWhiteSpace(nameof(UserFirstName));
            _userFirstName = value;
         }
      }

      /// <summary>
      /// User UId
      /// </summary>
      public Guid UserUId
      {
         get => _userUId;
         set
         {
            value.ThrowIfEmpty(nameof(UserUId));
            _userUId = value;
         }
      }

      /// <summary>
      /// Commented beer
      /// </summary>
      public Beer Beer
      {
         get => _beer;
         set
         {
            value.ThrowIfNull(nameof(Beer));
            _beer = value;
         }
      }

      public DateTime DatePosted { get; private set; }

      /// <summary>
      /// Check if Comment can be created
      /// </summary>
      /// <param name="body">Comment body</param>
      /// <param name="userFirstName">Comment author's username in identity server</param>
      /// <param name="beer">beer to which this comment belong</param>
      /// <param name="userUId">Comment author's uniqueId in identity server</param>
      /// <returns> <see cref="ValidationResult"/></returns>
      private static ValidationResult CanCreate(string body, string userFirstName, Beer beer, Guid userUId)
      {
         var errors = new List<(string Name, string Msg)>();

         body.CheckMandatory(nameof(Body), errors);
         body.CheckMaxLength(nameof(Body), BODY_MAX_LENGTH, errors);
         userFirstName.CheckMandatory(nameof(UserFirstName), errors);
         userFirstName.CheckMaxLength(nameof(UserFirstName), USERFIRSTNAME_MAX_LENGTH, errors);
         beer.CheckMandatory(nameof(Beer), errors);
         userUId.CheckGuidNotEmpty(nameof(UserUId), errors);

         return errors.ToValidationResult();
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="body">Comment body</param>
      /// <param name="userFirstName">Comment author's username in identity server</param>
      /// <param name="beer">beer to which this comment belong</param>
      /// <param name="userUId">Comment author's uniqueId in identity server</param>
      /// <returns>a <see cref="Comment"/></returns>
      /// <exception cref="BeersApiException"></exception>
      public static Comment Create(string body, string userFirstName, Beer beer, Guid userUId)
      {
         var validationResult = CanCreate(body, userFirstName, beer, userUId);
         if (validationResult != ValidationResult.Success)
         {
            throw BeersApiException.Create(BeersApiException.InvalidDataCode, validationResult.ErrorMessage,
               validationResult.MemberNames);
         }

         return new Comment
         {
            Beer = beer,
            Body = body,
            UserFirstName = userFirstName,
            UserUId = userUId,
            DatePosted = DateTime.Now
         };
      }
   }
}
