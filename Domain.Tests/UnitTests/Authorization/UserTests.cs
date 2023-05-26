using Domain.Authorization;
using System;
using Xunit;

namespace Domain.Tests.UnitTests.Authorization
{
   public class UserTests
   {
      private readonly Guid _uId = Guid.NewGuid();
      private readonly string _email = "test@email.com";
      private readonly string _userName = "userName";
      private const int USERNAME_MAX_LENGTH = 256;

      [Theory]
      [InlineData("")]
      [InlineData(" ")]
      [InlineData(null)]
      public void CreateUser_EmailEmpty_ThrowsBeersApiException(string value)
      {
         //act
         void Action() => User.Create(_uId, value, Role.BeersApiAdmin, _userName);

         //assert
         var exception = Assert.Throws<BeersApiException>(() => Action());
         Assert.Contains(nameof(User.Email), exception.InvalidData);
      }

      [Theory]
      [InlineData("email")]
      [InlineData("testemail.com")]
      public void CreateUser_EmailWrongFormat_ThrowsBeersApiException(string value)
      {
         //act
         void Action() => User.Create(_uId, value, Role.BeersApiAdmin, _userName);

         //assert
         var exception = Assert.Throws<BeersApiException>(() => Action());
         Assert.Contains(nameof(User.Email), exception.InvalidData);
      }

      [Fact]
      public void CreateUser_EmptyGuidAsUId_ThrowsBeersApiException()
      {
         //act
         void Action() => User.Create(Guid.Empty, _email, Role.BeersApiReadOnly, _userName);

         //assert
         var exception = Assert.Throws<BeersApiException>(() => Action());
         Assert.Contains(nameof(User.UId), exception.InvalidData);
      }

      [Theory]
      [InlineData("")]
      [InlineData(" ")]
      [InlineData(null)]
      public void CreateUser_FirstNameNotValid_ThrowsBeersApiException(string value)
      {
         //act
         void Action() => User.Create(_uId, _email, Role.BeersApiAdmin, value);

         //assert
         var exception = Assert.Throws<BeersApiException>(() => Action());
         Assert.Contains(nameof(User.FirstName), exception.InvalidData);
      }

      [Fact]
      public void CreateUser_FirstNameLengthGreaterThanMaxLength_ThrowsBeersApiException()
      {
         //act
         void Action() => User.Create(_uId, _email, Role.BeersApiAdmin, new string('a', USERNAME_MAX_LENGTH + 1));

         //assert
         var exception = Assert.Throws<BeersApiException>(() => Action());
         Assert.Contains(nameof(User.FirstName), exception.InvalidData);
      }
   }
}
