using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using BeersApi.IntegrationTests.Extensions;
using BeersApi.IntegrationTests.Helpers;
using DataAccess;
using FluentAssertions;
using IdentityModel;
using Xunit;

namespace BeersApi.IntegrationTests.Controllers.Flavour
{
   public class FlavourControllerDeleteTests : IClassFixture<CustomWebApplicationFactory<Startup>>, IClassFixture<DbContextFactory>, IDisposable
   {
      private readonly CustomWebApplicationFactory<Startup> _customWebApplicationFactory;
      private readonly BeersApiContext _beersApiContext;
      private string _token;
      private int _id;
      private const string Name = "Name";
      private const string Description = "Description";

      public FlavourControllerDeleteTests(CustomWebApplicationFactory<Startup> customWebApplicationFactory, DbContextFactory contextFactory)
      {
         _customWebApplicationFactory = customWebApplicationFactory;
         _beersApiContext = contextFactory.Context;

         var flavour = Domain.Entities.Flavour.Create(Name, Description);
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

      [Fact]
      public async Task DeleteFlavour_UserNotAuthenticated_ReturnsUnauthorized401()
      {
         //arrange
         _token = string.Empty;
         //act
         var response = await Exec().ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
      }

      [Fact]
      public async Task DeleteFlavour_UserNotAdmin_ReturnsForbidden403()
      {
         //arrange
         var claims = new List<Claim>
         {
            new Claim(JwtClaimTypes.GivenName, "testUser"),
            new Claim(JwtClaimTypes.Role, "User")
         };
         var token = _customWebApplicationFactory.Jwt.GenerateToken(claims);

         //act
         var response = await _customWebApplicationFactory.AddAuth(token).Delete($"/flavours/{_id}");

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
      }

      [Fact]
      public async Task DeleteFlavour_WrongIdFormat_ReturnsBadRequest()
      {
         //act
         var response = await _customWebApplicationFactory.AddAuth(_token).Delete($"/flavours/{Guid.NewGuid()}").ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      }

      [Fact]
      public async Task DeleteFlavour_WrongId_ReturnsNotFound()
      {
         //arrange
         _id = 0;

         //act
         var response = await Exec().ConfigureAwait(false);

         //assert
         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
      }

      [Fact]
      public async Task DeleteFlavour_AdminUser_ReturnsDeletedCategory()
      {
         //act
         var response = await Exec().ConfigureAwait(false);
         var flavourModel = await response.BodyAs<Models.Output.Flavours.Flavour>();

         //assert
         flavourModel.Id.Should().Be(_id);
         flavourModel.Name.Should().Be(Name);
         flavourModel.Description.Should().Be(Description);
      }

      private Task<HttpResponseMessage> Exec() =>
         _customWebApplicationFactory.AddAuth(_token).Delete($"/flavours/{_id}");

      public void Dispose()
      {
         _beersApiContext.Flavours.RemoveRange(_beersApiContext.Flavours);
         _beersApiContext.SaveChanges();
      }
   }
}
