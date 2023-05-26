using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BeersApi.Authorization
{
   public class CanDoEverythingHandler : AuthorizationHandler<CanDoEverythingRequirement>
   {
      protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanDoEverythingRequirement requirement)
      {
         var userEmail = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
         if (userEmail == null)
         {
            context.Fail();
            return Task.CompletedTask;
         }

         if (string.IsNullOrWhiteSpace(userEmail.Value))
         {
            context.Fail();
            return Task.CompletedTask;
         }

         if (userEmail.Value != requirement.UserEmail)
         {
            context.Fail();
            return Task.CompletedTask;
         }

         context.Succeed(requirement);
         return Task.CompletedTask;
      }
   }
}
