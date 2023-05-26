using BeersApi.Models.Input.Users.Create;
using FluentValidation.TestHelper;
using System;
using Xunit;

namespace BeersApi.Tests.Models.Users.Create
{
   public class CreateUserTests
   {
      private readonly CreateUserValidator _createUserValidator;

      public CreateUserTests()
      {
         _createUserValidator = new CreateUserValidator();
      }

      [Fact]
      public void CreateUser_InvalidEmails_ShouldHaveError()
      {
         _createUserValidator.ShouldHaveValidationErrorFor(u => u.Email, "");
         _createUserValidator.ShouldHaveValidationErrorFor(u => u.Email, string.Empty);
         _createUserValidator.ShouldHaveValidationErrorFor(u => u.Email, " ");
         _createUserValidator.ShouldHaveValidationErrorFor(u => u.Email, new string('a', 248) + "@test.com");
         _createUserValidator.ShouldHaveValidationErrorFor(u => u.Email, "testemail");
         _createUserValidator.ShouldHaveValidationErrorFor(u => u.Email, "@email");
      }

      [Fact]
      public void CreateUser_EmptyUId_ShouldHaveError()
      {
         _createUserValidator.ShouldHaveValidationErrorFor(u => u.UId, Guid.Empty);
      }

      [Fact]
      public void CreateUser_InvalidRoleName_ShouldHaveError()
      {
         _createUserValidator.ShouldHaveValidationErrorFor(u => u.RoleName, "");
         _createUserValidator.ShouldHaveValidationErrorFor(u => u.RoleName, "beersApiAdmin");
         _createUserValidator.ShouldHaveValidationErrorFor(u => u.RoleName, " ");
         _createUserValidator.ShouldHaveValidationErrorFor(u => u.RoleName, string.Empty);
         _createUserValidator.ShouldHaveValidationErrorFor(u => u.RoleName, "0");
      }

      [Fact]
      public void CreateUser_InvalidUserName_ShouldHaveError()
      {
         _createUserValidator.ShouldHaveValidationErrorFor(u => u.FirstName, "");
         _createUserValidator.ShouldHaveValidationErrorFor(u => u.FirstName, " ");
         _createUserValidator.ShouldHaveValidationErrorFor(u => u.FirstName, string.Empty);
         _createUserValidator.ShouldHaveValidationErrorFor(u => u.FirstName, new string('a', 257));
      }
   }
}
