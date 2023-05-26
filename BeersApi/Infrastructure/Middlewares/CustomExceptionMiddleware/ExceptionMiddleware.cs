using BeersApi.Models;
using Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;

namespace BeersApi.Infrastructure.Middlewares.CustomExceptionMiddleware
{
   public class ExceptionMiddleware
   {
      private readonly RequestDelegate _next;
      private readonly ILoggerFactory _loggerFactory;
      private readonly IWebHostEnvironment _env;

      public ExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, IWebHostEnvironment env)
      {
         _loggerFactory = loggerFactory;
         _next = next;
         _env = env;
      }

      public async Task InvokeAsync(HttpContext httpContext)
      {
         try
         {
            await _next(httpContext);
         }
         catch (BeersApiException ex)
         {
            _loggerFactory.CreateLogger("LoggerBeersApi").LogError($"Something went wrong: {ex}");
            await HandleExceptionAsync(httpContext, TranslateException(ex), ex.Message, ex.StackTrace, ex.InvalidData);
         }
         catch (Exception ex)
         {
            _loggerFactory.CreateLogger("LoggerBeersApi").LogError($"Something went wrong: {ex}");
            await HandleExceptionAsync(httpContext, HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace);
         }
      }

      private Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string message, string details, IEnumerable<string> invalidProperties = null)
      {
         context.Response.ContentType = "application/json";
         context.Response.StatusCode = (int)statusCode;

         return context.Response.WriteAsync(new ErrorDetails()
         {
            StatusCode = (int)statusCode,
            Message = message,
            InvalidProperties = invalidProperties,
            Details = _env.IsDevelopment() ? details : null
         }.ToString());
      }

      /// <summary>
      /// Map internal error code to https status code
      /// </summary>
      private static readonly ReadOnlyDictionary<int, HttpStatusCode> BeersApiCodeToStatusCode = new ReadOnlyDictionary<int, HttpStatusCode>(new Dictionary<int, HttpStatusCode>()
      {
         [BeersApiException.InvalidDataCode] = HttpStatusCode.BadRequest,
         [BeersApiException.NotFound] = HttpStatusCode.NotFound,
         [BeersApiException.Forbidden] = HttpStatusCode.Forbidden,
         [BeersApiException.Unauthorized] = HttpStatusCode.Unauthorized
      });

      /// <summary>
      /// Get a proper <see cref="HttpStatusCode"/> based on the details
      /// </summary>
      /// <param name="e">An exception</param>
      /// <returns>A status Code</returns>
      private HttpStatusCode TranslateException(BeersApiException e)
      {
         if (e.Detail.HasValue && BeersApiCodeToStatusCode.TryGetValue(e.Detail.Value, out var code))
            return code;
         return HttpStatusCode.InternalServerError;
      }
   }
}
