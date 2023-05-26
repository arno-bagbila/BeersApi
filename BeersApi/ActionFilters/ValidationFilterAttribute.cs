using BeersApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace BeersApi.ActionFilters
{
   public class ValidationFilterAttribute : ActionFilterAttribute
   {
      public override void OnActionExecuting(ActionExecutingContext context)
      {
         var param = context.ActionArguments.SingleOrDefault(p => p.Value is IEntity);
         if (param.Value == null)
         {
            context.Result = new BadRequestObjectResult("Object is null");
            return;
         }

         if (!context.ModelState.IsValid)
         {
            context.Result = new BadRequestObjectResult(context.ModelState);
         }
      }
   }
}
