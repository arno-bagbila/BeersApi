using FluentValidation;
using System;
using System.Linq;

namespace BeersApi.Models.Input.Users
{
   public class User
   {
      public string Email { get; set; }

      public string Password { get; set; }

      public string Firstname { get; set; }

      public string Lastname { get; set; }

      public DateTime DateOfBirth { get; set; }

      public string City { get; set; }

      public Guid CountryId { get; set; }
   }

   public class UserValidator : AbstractValidator<User>
   {
      public UserValidator()
      {
         var cutOffDate = DateTime.Now.AddYears(-18);
         RuleFor(e => e.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format.");

         RuleFor(p => p.Password)
            .MinimumLength(8).WithMessage("Password must be at least 8 characters")
            .MaximumLength(26).WithMessage("Password cannot be longer than 26 characters")
            .Must(p => p.Any(char.IsUpper)).WithMessage("Password must contain at least one Upper case")
            .Must(p => p.Any(char.IsLower)).WithMessage("Password must contain at least one lower characters")
            .Must(p => p.Any(char.IsDigit)).WithMessage("Password must contain at least one digit");

         RuleFor(u => u.Firstname)
            .NotEmpty().WithMessage("Firstname is required")
            .MinimumLength(3).WithMessage("Firstname must contain at least 3 characters")
            .MaximumLength(255).WithMessage("Firstname cannot br longer than 255 characters");

         RuleFor(u => u.Lastname)
            .NotEmpty().WithMessage("Lastname is required")
            .MinimumLength(3).WithMessage("Lastname should contain at least 3 characters")
            .MaximumLength(255).WithMessage("Lastname cannot be longer than 255 characters");

         RuleFor(u => u.City)
            .NotEmpty().WithMessage("City is required")
            .MinimumLength(3).WithMessage("City name must contain at least 3 characters")
            .MaximumLength(255).WithMessage("City name cannot contain more than 255 characters");

         RuleFor(u => u.CountryId)
            .NotEmpty().WithMessage("Country is required");

         RuleFor(u => u.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required")
            .LessThan(cutOffDate).WithMessage("User must be at least 18 years old");
      }
   }

}
