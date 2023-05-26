using Microsoft.AspNetCore.Authorization;

namespace BeersApi.Authorization
{
   public class CanDoEverythingRequirement : IAuthorizationRequirement
   {

      public string UserEmail { get; }

      public CanDoEverythingRequirement(string userEmail)
      {
         UserEmail = userEmail;
      }
   }
}
