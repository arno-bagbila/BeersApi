using Domain.Entities;
using System;
using Xunit;

namespace Domain.Tests.UnitTests
{
   public class CommentUnitTests
   {
      #region Data

      private const string BODY = "I am a comment";
      private const string USER_FIRST_NAME = "userFirstName";
      private Beer _beer;
      private Category _category;
      private Color _color;
      private Country _country;
      private readonly Guid USER_UNIQUE_ID = Guid.NewGuid();
      private const int BODY_MAX_LENGTH = 3000;
      private const int USERFIRSTNAME_MAX_LENGTH = 256;

      #endregion

      public CommentUnitTests()
      {
         _category = Category.Create("category name", "category description");
         _color = Color.Create("red");
         _country = Country.Create("Country name", "co");
         _beer = Beer.Create("Beer name", "Beer description", "logoUrl", 3.7, 2.5, _category, _color, _country);
      }

      [Theory]
      [InlineData("")]
      [InlineData(" ")]
      [InlineData(null)]
      public void CreateComment_BodyEmpty_ThrowsBeersApiException(string value)
      {
         //act
         void Action() => Comment.Create(value, USER_FIRST_NAME, _beer, USER_UNIQUE_ID);

         //assert
         var exception = Assert.Throws<BeersApiException>(() => Action());
         Assert.Contains(nameof(Comment.Body), exception.InvalidData);
      }

      [Fact]
      public void CreateComment_BodyLengthGreaterThanMax_ThrowsBeersApiException()
      {
         //act
         void Action() =>
            Comment.Create(new string('a', BODY_MAX_LENGTH + 1), USER_FIRST_NAME, _beer, USER_UNIQUE_ID);

         //assert
         var exception = Assert.Throws<BeersApiException>(() => Action());
         Assert.Contains(nameof(Comment.Body), exception.InvalidData);
      }

      [Theory]
      [InlineData("")]
      [InlineData(" ")]
      [InlineData(null)]
      public void CreateComment_UserFirstNameEmpty_ThrowsBeersApiException(string value)
      {
         //act
         void Action() => Comment.Create(value, USER_FIRST_NAME, _beer, USER_UNIQUE_ID);

         //assert
         var exception = Assert.Throws<BeersApiException>(() => Action());
         Assert.Contains(nameof(Comment.Body), exception.InvalidData);
      }

      [Fact]
      public void CreateComment_UserFirstNameLengthGreaterThanMax_ThrowsException()
      {
         //act
         void Action() => Comment.Create(BODY, new string('a', USERFIRSTNAME_MAX_LENGTH + 1), _beer, USER_UNIQUE_ID);

         //assert
         var exception = Assert.Throws<BeersApiException>(() => Action());
         Assert.Contains(nameof(Comment.UserFirstName), exception.InvalidData);
      }

      [Theory]
      [InlineData(null)]
      public void CreateComment_BeerNull_ThrowsBeersApiException(Beer value)
      {
         //act
         void Action() => Comment.Create(BODY, USER_FIRST_NAME, value, USER_UNIQUE_ID);

         //assert
         var exception = Assert.Throws<BeersApiException>(() => Action());
         Assert.Contains(nameof(Comment.Beer), exception.InvalidData);
      }
   }
}
