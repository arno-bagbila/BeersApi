using BeersApi.Models.Input.Users.Create;
using BeersApi.NUnit.FeatureTests.Extensions;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BeersApi.NUnit.FeatureTests.Users.UserTests
{

   [TestFixture]
   public class CreateUsers : NUnitFeatureTestBase
   {
      private CreateUser _createUser;
      private Guid _uid = Guid.NewGuid();
      private string _email = "email@test.com";
      private string _roleName = "BeersApiAdmin";
      private string _firstName = "userName";

      [SetUp]
      public void BeforeEachTest()
      {
         _createUser = new CreateUser { UId = _uid, Email = _email, RoleName = _roleName, FirstName = _firstName };
      }

      #region Tests

      [Test]
      public async Task CreateUser_InvalidRole_ReturnsBadRequest400()
      {
         //arrange
         _createUser.RoleName = "RoleName";

         //act
         var response = await Exec().ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Test]
      public async Task CreateUser_ExistingEmail_ReturnsBadRequest400()
      {
         //arrange
         var userUId = Guid.NewGuid();
         var createUser = new CreateUser
         {
            Email = "existingemail@test.com",
            RoleName = "BeersApiAdmin",
            UId = userUId,
            FirstName = "username"
         };
         await Client.PostAsJsonAsync("users", createUser).ConfigureAwait(false);

         _createUser.Email = "existingemail@test.com";

         //act
         var response = await Exec().ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Test]
      public async Task CreateUser_ExistingUniqueId_ReturnsBadRequest400()
      {
         //arrange
         var userUId = Guid.NewGuid();
         var createUser = new CreateUser
         {
            Email = "newexistingemail@test.com",
            RoleName = "BeersApiAdmin",
            UId = userUId,
            FirstName = "username"
         };
         await Client.PostAsJsonAsync("users", createUser).ConfigureAwait(false);

         _createUser.UId = userUId;

         //act
         var response = await Exec().ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Test]
      public async Task CreateUser_ValidInput_ReturnsCreateUser201()
      {
         //act
         var response = await Exec().ConfigureAwait(false);
         var user = await response.BodyAs<Models.Output.Users.User>();

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.Created);
         user.Email.Should().Be(_email);
         user.RoleName.Should().Be(_roleName);
         user.UId.Should().Be(_uid);
         user.Id.Should().NotBe(0);
         user.FirstName.Should().Be(_firstName);
      }

      #endregion

      private async Task<HttpResponseMessage> Exec()
      {
         return await Client.PostAsJsonAsync("users", _createUser).ConfigureAwait(false);
      }

   }
}
