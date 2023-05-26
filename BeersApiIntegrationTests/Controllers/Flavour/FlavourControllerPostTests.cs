using BeersApi.IntegrationTests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using BeersApi.IntegrationTests.Extensions;
using BeersApi.Models.Input.Flavours.Create;
using DataAccess;
using FluentAssertions;
using IdentityModel;
using Xunit;

namespace BeersApi.IntegrationTests.Controllers.Flavour
{
   public class FlavourControllerPostTests : IClassFixture<CustomWebApplicationFactory<Startup>>, IClassFixture<DbContextFactory>, IDisposable
   {
      private readonly CustomWebApplicationFactory<Startup> _customWebApplicationFactory;
      private readonly BeersApiContext _beersApiContext;
      private CreateFlavour _createFlavour = new CreateFlavour
      {
         Name = Name,
         Description = Description
      };
      private const string Name = "FlavourName";
      private const string Description = "FlavourDescription";
      private string _token;

      public FlavourControllerPostTests(CustomWebApplicationFactory<Startup> customWebApplicationFactory, DbContextFactory contextFactory)
      {
         _beersApiContext = contextFactory.Context;
         _customWebApplicationFactory = customWebApplicationFactory;

         var claims = new List<Claim>
         {
            new Claim(JwtClaimTypes.GivenName, "testUser"),
            new Claim(JwtClaimTypes.Role, "admin")
         };

         _token = _customWebApplicationFactory.Jwt.GenerateToken(claims);
      }

      private Task<HttpResponseMessage> Exec() => _customWebApplicationFactory.AddAuth(_token).Post("/flavours", _createFlavour);

      [Fact]
      public async Task CreateFlavour_NotAdmin_ReturnForbidden403()
      {
         //arrange
         var claims = new List<Claim>
         {
            new Claim(JwtClaimTypes.GivenName, "testUser"),
            new Claim(JwtClaimTypes.Role, "User")
         };

         _token = _customWebApplicationFactory.Jwt.GenerateToken(claims);

         //act
         var response = await Exec();

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
      }

      [Fact]
      public async Task CreateFlavour_NoTokenProvided_ReturnUnauthorized401()
      {
         //arrange
         _token = "";

         //act
         var response = await Exec();

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
      }

      [Fact]
      public async Task CreateFlavour_EmptyName_ReturnsBadRequest()
      {
         //arrange
         _createFlavour.Name = string.Empty;

         //act
         var response = await Exec().ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task CreateFlavour_NameNotProvided_ReturnsBadRequest()
      {
         //arrange
         _createFlavour = new CreateFlavour { Description = Description };

         //act
         var response = await Exec().ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task CreateFlavour_NameMoreThan50Characters()
      {
         //arrange
         _createFlavour.Name = new string('a', 51);

         //act
         var response = await Exec().ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task CreateFlavour_DescriptionNotProvided_ReturnsBadRequest()
      {
         //arrange
         _createFlavour = new CreateFlavour { Name = Name };

         //act
         var response = await Exec().ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task CreateFlavour_EmptyDescription_ReturnsBadRequest()
      {
         //arrange
         _createFlavour = new CreateFlavour { Name = Name, Description = string.Empty };

         //act
         var response = await Exec().ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task CreateFlavour_DescriptionLessThan3Characters_ReturnBadRequest()
      {
         //arrange
         _createFlavour.Description = "12";

         //act
         var response = await Exec().ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task CreateFlavour_DescriptionMoreThan3000Characters_ReturnsBadRequest()
      {
         //arrange
         _createFlavour.Name = new string('a', 3001);

         //act
         var response = await Exec().ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task CreateFlavour_FlavourWithSameNameAlreadyExists_ReturnsBadRequest()
      {
         //arrange
         var flavour = Domain.Entities.Flavour.Create("FlavourName", "Flavour Description");
         _beersApiContext.Add(flavour);
         await _beersApiContext.SaveChangesAsync();

         //act
         var response = await Exec().ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task CreateFlavour_ValidCreateFlavour_SaveFlavour()
      {
         //act
         await Exec();
         var flavourInDb = _beersApiContext.Flavours.FirstOrDefault(c => c.Name == _createFlavour.Name);

         //assert
         flavourInDb.Should().NotBeNull();
      }

      [Fact]
      public async Task CreateFlavour_ValidCreateFlavour_ReturnsCreatedFlavour()
      {
         //act
         var response = await Exec();
         var flavour = await response.BodyAs<Models.Output.Flavours.Flavour>();

         //assert
         flavour.UId.Should().NotBeEmpty();
         flavour.Name.Should().Be(_createFlavour.Name);
         flavour.Description.Should().Be(_createFlavour.Description);
      }

      public void Dispose()
      {
         _beersApiContext.Flavours.RemoveRange(_beersApiContext.Flavours);
         _beersApiContext.SaveChanges();
      }
   }
}
