using Domain.Authorization;
using FluentValidation;
using System;

namespace BeersApi.Models.Input.Users.Create
{
   public class CreateUser
   {
      /// <summary>
      /// User unique identifier
      /// </summary>
      public Guid UId { get; set; }

      /// <summary>
      /// User Email
      /// </summary>
      public string Email { get; set; }

      /// <summary>
      /// User Role
      /// </summary>
      public string RoleName { get; set; }

      /// <summary>
      /// user 's firstname from identity server
      /// </summary>
      public string FirstName { get; set; }
   }

   public class CreateUserValidator : AbstractValidator<CreateUser>
   {
      public CreateUserValidator()
      {
         RuleFor(u => u.UId)
            .NotEmpty()
            .Must(uid => uid != Guid.Empty);

         RuleFor(u => u.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(256);

         RuleFor(u => u.RoleName)
            .NotEmpty()
            .IsEnumName(typeof(Role));

         RuleFor(u => u.FirstName)
            .NotEmpty()
            .MaximumLength(256);
      }
   }
}
