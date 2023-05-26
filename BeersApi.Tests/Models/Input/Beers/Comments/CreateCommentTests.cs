using BeersApi.Models.Input.Beers.Comments;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using Xunit;

namespace BeersApi.Tests.Models.Input.Beers.Comments
{
   public class CreateCommentTests
   {
      private readonly CreateCommentValidator _createCommentValidator;
      private const int BodyMaxLength = 3000;
      private const int UserFirstNameMaxLength = 256;

      public CreateCommentTests()
      {
         _createCommentValidator = new CreateCommentValidator();
      }

      public static IEnumerable<object[]> GetInvalidUserFirstName()
      {
         yield return new object[] { null, string.Empty, " ", new string('a', UserFirstNameMaxLength + 1) };
      }

      [Fact]
      public void CreateComment_InvalidBody_ShouldHaveErrors()
      {
         var invalidBodies = new List<string> { null, string.Empty, " ", new string('a', BodyMaxLength + 1) };

         foreach (var invalidBody in invalidBodies)
         {
            var model = new CreateComment { Body = invalidBody };
            var validator = _createCommentValidator.TestValidate(model);
            validator.ShouldHaveValidationErrorFor(c => c.Body);
         }
      }

      [Fact]
      public void CreateComment_InvalidUserFirstName_ShouldHaveErrors()
      {
         var invalidUserFirstNames = new List<string> { null, string.Empty, " ", new string('a', UserFirstNameMaxLength + 1) };

         foreach (var invalidUserFirstName  in invalidUserFirstNames)
         {
            var model = new CreateComment { UserFirstName = invalidUserFirstName };
            var validator = _createCommentValidator.TestValidate(model);
            validator.ShouldHaveValidationErrorFor(c => c.UserFirstName);
         }
      }

      [Fact]
      public void CreateComment_InvalidBeerId_ShouldHaveError()
      {
         var model = new CreateComment { BeerId = 0 };
         var validator = _createCommentValidator.TestValidate(model);
         validator.ShouldHaveValidationErrorFor(c => c.BeerId);
      }

      [Fact]
      public void CreateComment_InvalidUserUId_ShouldHaveError()
      {
         var model = new CreateComment { UserUId = Guid.Empty };
         var validator = _createCommentValidator.TestValidate(model);
         validator.ShouldHaveValidationErrorFor(c => c.UserUId);
      }
   }
}
