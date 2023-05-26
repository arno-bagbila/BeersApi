using Microsoft.AspNetCore.Builder;

namespace BeersApi.Infrastructure.Middlewares.CustomExceptionMiddleware
{
   public static class ExceptionMiddlewareExtensions
   {
      public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder builder) => builder.UseMiddleware<ExceptionMiddleware>();
   }
}
