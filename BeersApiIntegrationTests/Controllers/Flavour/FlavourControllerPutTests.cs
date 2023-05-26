using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using BeersApi.IntegrationTests.Extensions;
using BeersApi.IntegrationTests.Helpers;
using BeersApi.Models.Input.Flavours.Update;
using DataAccess;
using FluentAssertions;
using IdentityModel;
using Xunit;

namespace BeersApi.IntegrationTests.Controllers.Flavour
{
   public class FlavourControllerPutTests : IClassFixture<CustomWebApplicationFactory<Startup>>, IClassFixture<DbContextFactory>, IDisposable
   {
      private readonly CustomWebApplicationFactory<Startup> _customWebApplicationFactory;
      private readonly BeersApiContext _beersApiContext;
      private UpdateFlavour _updateFlavour = new UpdateFlavour
      {
         Name = Name,
         Description = Description
      };
      private const string Name = "FlavourNameUpdated";
      private const string Description = "FlavourDescriptionUpdated";
      private int _id;
      private string _token;

      public FlavourControllerPutTests(CustomWebApplicationFactory<Startup> customWebApplicationFactory, DbContextFactory contextFactory)
      {
         _beersApiContext = contextFactory.Context;
         _customWebApplicationFactory = customWebApplicationFactory;
         var flavour = Domain.Entities.Flavour.Create("FlavourName", "Flavour Description");
         _beersApiContext.Add(flavour);
         _beersApiContext.SaveChanges();
         _id = flavour.Id;

         var claims = new List<Claim>
         {
            new Claim(JwtClaimTypes.GivenName, "testUser"),
            new Claim(JwtClaimTypes.Role, "admin")
         };

         _token = _customWebApplicationFactory.Jwt.GenerateToken(claims);
      }

      private Task<HttpResponseMessage> Exec(int id) =>
         _customWebApplicationFactory.AddAuth(_token).Put($"/flavours/{id}", _updateFlavour);

      [Fact]
      public async Task UpdateFlavour_NotAdmin_ReturnForbidden403()
      {
         //arrange
         var claims = new List<Claim>
         {
            new Claim(JwtClaimTypes.GivenName, "testUser"),
            new Claim(JwtClaimTypes.Role, "User")
         };

         _token = _customWebApplicationFactory.Jwt.GenerateToken(claims);

         //act
         var response = await Exec(_id).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
      }

      [Fact]
      public async Task UpdateFlavour_NoTokenProvided_ReturnUnauthorized401()
      {
         //arrange


         _token = "";

         //act
         var response = await Exec(_id).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
      }

      [Fact]
      public async Task UpdateFlavour_NameNotProvided_ReturnsBadRequest_I()
      {
         //arrange
         _updateFlavour = new UpdateFlavour { Description = Description };

         //act
         var response = await Exec(_id).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task UpdateFlavour_NameEmpty_ReturnsBadRequest()
      {
         //arrange
         _updateFlavour.Name = string.Empty;

         //act
         var response = await Exec(_id).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task UpdateFlavour_NameLengthMoreThan50Characters_ReturnsBadRequest()
      {
         //arrange
         _updateFlavour.Name = new string('a', 51);

         //act
         var response = await Exec(_id).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task UpdateFlavour_DescriptionNotProvided_ReturnsBadRequest()
      {
         //arrange
         _updateFlavour = new UpdateFlavour { Name = Name };

         //act
         var response = await Exec(_id).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task UpdateFlavour_DescriptionEmpty_ReturnsBadRequest()
      {
         //arrange
         _updateFlavour.Description = string.Empty;

         //act
         var response = await Exec(_id).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task UpdateFlavour_DescriptionLengthLessThan3Characters_ReturnsBadRequest()
      {
         //arrange
         _updateFlavour.Description = "12";

         //act
         var response = await Exec(_id).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task UpdateFlavour_DescriptionLengthMoreThan3000Characters_ReturnsBadRequest()
      {
         //arrange
         _updateFlavour.Description = new string('a', 3001);

         //act
         var response = await Exec(_id).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task UpdateFlavour_NameAlreadyExistForAnotherCategory_ReturnsBadRequest()
      {
         //arrange
         _beersApiContext.Add(Domain.Entities.Flavour.Create("Another Flavour Name", "Another Flavour Description"));
         await _beersApiContext.SaveChangesAsync();
         _updateFlavour.Name = "Another Flavour Name";

         //act
         var response = await Exec(_id).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task UpdateFlavour_NoExistingCategory_ReturnsNotFound()
      {
         //arrange
         _id = 100;

         //act
         var response = await Exec(_id).ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
      }

      [Fact]
      public async Task UpdateFlavour_ValidInput_ReturnsUpdatedCategory()
      {
         //act
         var response = await Exec(_id).ConfigureAwait(false);
         var categoryModel = await response.BodyAs<Models.Output.Categories.Category>();

         //assert
         categoryModel.Name.Should().Be(_updateFlavour.Name);
         categoryModel.Description.Should().Be(_updateFlavour.Description);
      }

      public void Dispose()
      {
         _beersApiContext.Flavours.RemoveRange(_beersApiContext.Flavours);
         _beersApiContext.SaveChanges();
      }
   }
}
