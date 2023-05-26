using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BeersApi.ActionFilters
{
   public class ValidateEntityId : ActionFilterAttribute
   {
      public override void OnActionExecuting(ActionExecutingContext context)
      {
         if (context.ActionArguments.ContainsKey("id"))
         {
            int id = (int)context.ActionArguments["id"];
         }
         else
         {
            context.Result = new BadRequestObjectResult("Bad id parameter");
            return;
         }
      }
   }
}
