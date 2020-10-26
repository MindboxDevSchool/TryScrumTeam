using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace ItHappend.RestAPI.Filters
{
    public class LoggingFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var message = "Executed request: ";
            message += context.HttpContext.Request.Method + "  ";
            message += context.HttpContext.Request.Path.ToString();
            Log.Logger.Information(message);
            Log.Logger.Information(context.HttpContext.Request.Body.ToString());
        }
    }
}