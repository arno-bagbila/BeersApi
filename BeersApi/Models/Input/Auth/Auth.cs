using FluentValidation;
//using FluentValidation.Attributes;

namespace BeersApi.Models.Input.Auth
{
   public class Auth
   {
      public string Email { get; set; }
      public string Password { get; set; }
   }

   public class AuthValidator : AbstractValidator<Auth>
   {
      public AuthValidator()
      {
         RuleFor(a => a.Email).NotEmpty().MinimumLength(3).MaximumLength(254).EmailAddress();
         RuleFor(a => a.Password).NotEmpty().MinimumLength(3).MaximumLength(128);
      }
   }
}
