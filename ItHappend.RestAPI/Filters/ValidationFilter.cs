using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ItHappend.RestAPI.Filters
{
    public class ValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(
                    context.ModelState.SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage).ToArray());
            }

            base.OnActionExecuting(context);
        }
    }
}