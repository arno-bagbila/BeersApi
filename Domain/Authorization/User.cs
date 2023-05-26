using Domain.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Authorization
{
   public class User
   {

      #region Data

      private string _email;
      private Guid _guid;
      private string _firstName;

      #endregion
      /// <summary>
      /// User Id
      /// </summary>
      public int Id { get; private set; }

      /// <summary>
      /// Unique Id set in UserStorage
      /// </summary>
      public Guid UId
      {
         get => _guid;
         set => _guid = value;
      }

      /// <summary>
      /// User email
      /// </summary>
      public string Email
      {
         get => _email;
         set
         {
            value.ThrowIfNullOrWhiteSpace(nameof(Email));
            _email = value;
         }
      }

      /// <summary>
      /// User role
      /// </summary>
      public Role Role { get; private set; }

      /// <summary>
      /// User userName
      /// </summary>
      public string FirstName
      {
         get => _firstName;
         set
         {
            value.ThrowIfNullOrWhiteSpace(nameof(FirstName));
            _firstName = value;
         }
      }

      /// <summary>
      /// Create a new instance of <see cref="User"/>
      /// </summary>
      /// <param name="uId">User unique Id</param>
      /// <param name="email">User email</param>
      /// <param name="role">User role</param>
      /// <param name="userName">user username</param>
      /// <returns>A new instance of <see cref="User"/></returns>
      public static User Create(Guid uId, string email, Role role, string userName)
      {
         var validationResult = CanCreate(uId, email, userName);
         if (validationResult != ValidationResult.Success)
         {
            throw BeersApiException.Create(BeersApiException.InvalidDataCode, validationResult.ErrorMessage,
               validationResult.MemberNames);
         }

         return new User
         {
            UId = uId,
            Email = email,
            Role = role,
            FirstName = userName
         };
      }

      /// <summary>
      /// Check if Category can be created
      /// </summary>
      /// <param name="uId">User unique Id</param>
      /// <param name="email">User email</param>
      /// <param name="userName">user username</param>
      /// <returns> <see cref="ValidationResult"/></returns>
      public static ValidationResult CanCreate(Guid uId, string email, string userName)
      {
         var errors = new List<(string Name, string Msg)>();

         uId.CheckGuidNotEmpty(nameof(UId), errors);
         email.CheckMandatory(nameof(Email), errors);
         email.CheckValidEmail(nameof(Email), errors);
         userName.CheckMandatory(nameof(FirstName), errors);
         userName.CheckMaxLength(nameof(FirstName), 256, errors);

         return errors.ToValidationResult();
      }
   }
}
