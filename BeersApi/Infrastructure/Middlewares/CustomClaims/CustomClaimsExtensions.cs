using Microsoft.AspNetCore.Builder;
using System;

namespace BeersApi.Infrastructure.Middlewares.CustomClaims
{
   /// <summary>
   /// Adds local authorization claims to user (if authenticated)
   /// </summary>
   public static class CustomClaimsExtensions
   {
      public static void AddCustomClaims(this IApplicationBuilder builder)
      {
         builder.UseWhen(context => context.Request.Method.Equals("post", StringComparison.OrdinalIgnoreCase)
                                    || context.Request.Method.Equals("put", StringComparison.OrdinalIgnoreCase)
                                    || context.Request.Method.Equals("delete", StringComparison.OrdinalIgnoreCase),
            builder =>
            {
               builder.UseMiddleware<CustomClaimsHandler>();
            });

      }

   }
}
