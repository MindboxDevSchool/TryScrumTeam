using System.Reflection;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace ItHappend.RestAPI.Filters
{
    public class LoggingFilter: IActionFilter 
    {
        public void OnActionExecuting(ActionExecutingContext context)
            {
                var message = "Started: ";
                message += context.HttpContext.Request.Method + "  ";
                message += context.HttpContext.Request.Path.ToString();
                Log.Logger.Information(message);
            }

            public void OnActionExecuted(ActionExecutedContext context)
            {
                var message = "Finished: ";
                message += context.HttpContext.Request.Method + "  ";
                message += context.HttpContext.Request.Path.ToString();
                Log.Logger.Information(message);
            }
        
    }
}