using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace ItHappend.RestAPI.Filters
{
    public class LoggingFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var message = "Executing request: ";
            message += context.HttpContext.Request.Method + "  ";
            message += context.HttpContext.Request.Path.ToString();
            Log.Logger.Information(message);
            
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}