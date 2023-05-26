using DataAccess;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BeersApi.Infrastructure.Middlewares.CustomClaims
{
   public class CustomClaimsHandler
   {
      public const string BEERSAPI_ROLE_HEADER = "X-BEERSAPI-Role";
      private readonly RequestDelegate _next;
      private readonly Func<IBeersApiContext> _contextFactory;

      public CustomClaimsHandler(RequestDelegate next, Func<IBeersApiContext> contextFactory)
      {
         _next = next;
         _contextFactory = contextFactory;
      }

      /// <summary>
      /// Adds local authorization claims to user (if authenticated)
      /// </summary>
      /// <param name="httpContext"></param>
      /// <returns></returns>
      public async Task InvokeAsync(HttpContext httpContext)
      {
         if (httpContext.User.Identity != null && httpContext.User.Identity.IsAuthenticated)
         {
            var beersApiContext = _contextFactory();
            var emailClaim = httpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

            if (emailClaim == null)
               throw BeersApiException.Create(BeersApiException.InvalidDataCode, $"Claim type {ClaimTypes.Email} is not present in user claims");

            var user = await beersApiContext
               .Users
               .SingleOrDefaultAsync(x => x.Email == emailClaim.Value)
               .ConfigureAwait(false);

            if (user == null)
               throw BeersApiException.Create(BeersApiException.Forbidden,
                  $"Cannot find user with email: {emailClaim.Value}");

            httpContext.User
               .Identities
               .FirstOrDefault()
               ?.AddClaim(new Claim("BeersApiRole", user.Role.ToString()));

            httpContext.Response.Headers.Add(BEERSAPI_ROLE_HEADER, user.Role.ToString());

         }

         await _next(httpContext);
      }
   }
}
