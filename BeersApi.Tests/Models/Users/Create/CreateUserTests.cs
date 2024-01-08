using BeersApi.Models.Input.Users.Create;
using FluentValidation.TestHelper;
using System;
using Xunit;

namespace BeersApi.Tests.Models.Users.Create
{
   public class CreateUserTests
   {
      private readonly CreateUserValidator _createUserValidator = new();

      [Fact]
      public void CreateUser_InvalidEmails_ShouldHaveError()
      {
          var invalidEmails = new[] { null, string.Empty, " ", new string('a', 248) + "@test.com", "testemail", "@email"};

          foreach (var invalidEmail in invalidEmails)
          {
                var model = new CreateUser { Email = invalidEmail };
                var validator = _createUserValidator.TestValidate(model);
                validator.ShouldHaveValidationErrorFor(c => c.Email);
          }
      }

      [Fact]
      public void CreateUser_EmptyUId_ShouldHaveError()
      {
          var model = new CreateUser { UId = Guid.Empty };
          var validator = _createUserValidator.TestValidate(model);
          validator.ShouldHaveValidationErrorFor(c => c.UId);
      }

      [Fact]
      public void CreateUser_InvalidRoleName_ShouldHaveError()
      {
          var invalidRoleNames = new[] { null, string.Empty, " ", "beersApiAdmin", "0" };

          foreach (var invalidRoleName in invalidRoleNames)
          {
                    var model = new CreateUser { RoleName = invalidRoleName };
                    var validator = _createUserValidator.TestValidate(model);
                    validator.ShouldHaveValidationErrorFor(c => c.RoleName);
          }
      }

      [Fact]
      public void CreateUser_InvalidUserName_ShouldHaveError()
      {
          var invalidUserNames = new[] { null, string.Empty, " ", new string('a', 257) };

          foreach (var invalidUserName in invalidUserNames)
          {
                    var model = new CreateUser { FirstName = invalidUserName };
                    var validator = _createUserValidator.TestValidate(model);
                    validator.ShouldHaveValidationErrorFor(c => c.FirstName);
          }
      }
   }
}
