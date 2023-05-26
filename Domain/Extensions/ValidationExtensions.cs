using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Domain.Extensions
{
   internal static class ValidationExtensions
   {

      /// <summary>
      ///  Check <paramref name="value"/> and throw an <see cref="BeersApiException"/> if it satisfies <paramref name="isInvalid"/>
      /// </summary>
      /// <typeparam name="T">Type of the value being checked</typeparam>
      /// <param name="value">Value to be tested</param>
      /// <param name="isInvalid">Function used to test if <paramref name="value"/> is not valid</param>
      /// <param name="message">Error message if exception is thrown</param>
      /// <param name="targetProperty">Name of the property <paramref name="value"/> would be assigned to</param>
      /// <exception cref="BeersApiException">Thrown when <paramref name="value"/> satisfies <paramref name="isInvalid"/></exception>
      private static void ThrowIfInvalid<T>(this T value, Func<T, bool> isInvalid, string message,
         string targetProperty)
      {
         if (isInvalid(value))
            throw BeersApiException.Create(BeersApiException.InvalidDataCode, message, new[] { targetProperty });
      }

      /// <summary>
      /// Throw an <see cref="BeersApiException"/> if <paramref name="value"/> is one of: <c>null</c>, <c>""</c> or a string with just spaces
      /// </summary>
      /// <param name="value"></param>
      /// <param name="targetProperty"></param>
      /// <exception cref="BeersApiException"> thrown when <paramref name="value"/> is not a valid string</exception>
      public static void ThrowIfNullOrWhiteSpace(this string value, string targetProperty)
      {
         value.ThrowIfInvalid(string.IsNullOrWhiteSpace, $"'{targetProperty}' must have a value", targetProperty);
      }

      /// <summary>
      /// Validate a string against <c>null</c>, <c>""</c>, or a string with just white spaces; if validation fails details are added
      /// to <paramref name="errors"/>
      /// </summary>
      /// <param name="value">value to be tested</param>
      /// <param name="targetProperty">Name of the property <paramref name="value"/> will be assigned to</param>
      /// <param name="errors">Errors are added to the list if the validation fails</param>
      public static void CheckMandatory(this string value, string targetProperty,
         IList<(string Name, string Msg)> errors)
      {
         if (string.IsNullOrWhiteSpace(value))
            errors.Add((targetProperty, $"{targetProperty} must be set"));
      }

      /// <summary>
      /// Validate a double again 0
      /// </summary>
      /// <param name="value">value to be tested</param>
      /// <param name="targetProperty">Name of the property <paramref name="value"/> will be assigned to</param>
      /// <param name="errors">Errors are added to the list if the validation fails</param>
      public static void CheckMandatory(this double value, string targetProperty,
         IList<(string Name, string Msg)> errors)
      {
         if (value == default)
            errors.Add((targetProperty, $"{targetProperty} cannot be 0"));
      }

      /// <summary>
      /// Verify a <paramref name="value"/> is set; error details are added to <paramref name="errors"/> if instance is <c>null</c>
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="value">value to be tested</param>
      /// <param name="targetProperty">Name of the property <paramref name="value"/> will be assigned to</param>
      /// <param name="errors">List errors is added to if validation fails</param>
      public static void CheckMandatory<T>(this T value, string targetProperty, IList<(string Name, string Msg)> errors) where T : class
      {
         if (value is null)
            errors.Add((targetProperty, $"{targetProperty} must be set"));
      }

      /// <summary>
      /// Convert a possibly empty list of messages into a <see cref="ValidationResult"/>
      /// </summary>
      /// <param name="errors">A list of messages </param>
      /// <returns>A <see cref="ValidationResult"/>; <see cref="ValidationResult.Success"/> is returned when <paramref name="errors"/> is empty</returns>
      public static ValidationResult ToValidationResult(this IList<(string Name, string Msg)> errors)
      {
         return errors.Count > 0
            ? new ValidationResult(string.Join(Environment.NewLine, errors.Select(e => e.Msg)),
               errors.Select(e => e.Name).Distinct())
            : ValidationResult.Success;
      }

      /// <summary>
      /// Check if string is in email format
      /// </summary>
      /// <param name="email">string to be tested</param>
      /// <returns>True if <paramref name="email"/> is a good email format; False if <paramref name="email"/> is not a good email format</returns>
      public static bool IsValidEmail(this string email)
      {
         var addr = new System.Net.Mail.MailAddress(email);
         try
         {
            return addr.Address == email;
         }
         catch
         {
            return false;
         }
      }

      /// <summary>
      /// Throw an <see cref="BeersApiException"/> if <paramref name="value"/> is 0
      /// </summary>
      /// <param name="value">value to check</param>
      /// <param name="targetProperty"></param>
      /// <exception cref="BeersApiException"> thrown when <paramref name="value"/> is 0</exception>
      public static void ThrowIfZero(this double value, string targetProperty)
      {
         value.ThrowIfInvalid(x => x == default, $"{targetProperty} cannot be 0.", targetProperty);
      }

      /// <summary>
      /// Thrown an <see cref="BeersApiException"/> if <paramref name="value"/> is <c>null</c>
      /// </summary>
      /// <typeparam name="T">Type of the value being checked</typeparam>
      /// <param name="value">value to be tested</param>
      /// <param name="targetProperty">name of the property <paramref name="value"/> would be assigned to</param>
      /// <exception cref="BeersApiException">Thrown when <paramref name="value"/> is <c>null</c></exception>
      public static void ThrowIfNull<T>(this T value, string targetProperty) where T : class
      {
         value.ThrowIfInvalid(x => x is null, $"{targetProperty} must be set", targetProperty);
      }

      /// <summary>
      /// Verify <param name="email"></param> has an email format
      /// </summary>
      /// <param name="email">email to check</param>
      /// <param name="targetProperty">Name of the property <paramref name="email"/> will be assigned to</param>
      /// <param name="errors">List errors is added to if validation fails</param>
      public static void CheckValidEmail(this string email, string targetProperty,
         IList<(string Name, string Msg)> errors)
      {
         //use regular expression to validate email in order to catch "test@email"
         if (string.IsNullOrWhiteSpace(email)) return;
         try
         {
            var addr = new System.Net.Mail.MailAddress(email);
         }
         catch
         {
            errors.Add((targetProperty, $"{targetProperty} is not a valid email format"));
         }
      }

      /// <summary>
      /// Verify <param name="value"></param> is not an empty guid
      /// </summary>
      /// <param name="value">value to be tested</param>
      /// <param name="targetProperty">Name of the property <paramref name="value"/> will be assigned to</param>
      /// <param name="errors">List errors is added to if validation fails</param>
      public static void CheckGuidNotEmpty(this Guid value, string targetProperty,
         IList<(string name, string Msg)> errors)
      {
         if (value == Guid.Empty)
            errors.Add((targetProperty, $"{targetProperty} cannot be an empty Guid"));
      }

      /// <summary>
      /// Throw an <see cref="BeersApiException"/> if <paramref name="value"/> is empty
      /// </summary>
      /// <param name="value"></param>
      /// <param name="targetProperty"></param>
      /// <exception cref="BeersApiException"> thrown when <paramref name="value"/> is not a valid Guid</exception>
      public static void ThrowIfEmpty(this Guid value, string targetProperty)
      {
         if (value == Guid.Empty)
            throw BeersApiException.Create(BeersApiException.InvalidDataCode, $"'{targetProperty}' must have a value");
      }

      /// <summary>
      /// Throw an <see cref="BeersApiException"/> if <paramref name="value"/> is lesser than 0
      /// </summary>
      /// <param name="value"></param>
      /// <param name="targetProperty"></param>
      /// <exception cref="BeersApiException"> thrown when <paramref name="value"/> is lesser than 0</exception>
      public static void ThrowIfZeroOrLesser(this int value, string targetProperty)
      {
         if (value <= 0)
            throw BeersApiException.Create(BeersApiException.InvalidDataCode, $"'{targetProperty}' must be lesser than O");
      }

      /// <summary>
      /// Validate a string against <c>O</c> or lesser value; if validation fails details are added
      /// to <paramref name="errors"/>
      /// </summary>
      /// <param name="value">value to be tested</param>
      /// <param name="targetProperty">Name of the property <paramref name="value"/> will be assigned to</param>
      /// <param name="errors">Errors are added to the list if the validation fails</param>
      public static void CheckMandatory(this int value, string targetProperty,
         IList<(string Name, string Msg)> errors)
      {
         if (value <= 0)
            errors.Add((targetProperty, $"{targetProperty} must be set"));
      }

      /// <summary>
      /// Verify <param name="value"></param> is in max length
      /// </summary>
      /// <param name="value">email to check</param>
      /// <param name="targetProperty">Name of the property <paramref name="value"/> will be assigned to</param>
      /// <param name="maxLength">max length allow for this property</param>
      /// <param name="errors">List errors is added to if validation fails</param>
      public static void CheckMaxLength(this string value, string targetProperty, int maxLength,
         IList<(string Name, string Msg)> errors)
      {
         if (value != null && value.Length > maxLength)
         {
            errors.Add((targetProperty, $"{targetProperty} is not a valid email format"));
         }
      }

   }
}
