using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ItHappened.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ItHappend.RestAPI.Filters
{
    public class GlobalExceptionAttribute : Attribute, IExceptionFilter
    {
        
        public void OnException(ExceptionContext context)
        {
            
            context.Result = new ObjectResult(context.Exception.Message)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
            };
        }
    }
}