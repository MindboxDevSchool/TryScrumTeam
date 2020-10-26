using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ItHappened.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using Serilog.Core;

namespace ItHappend.RestAPI.Filters
{
    public class GlobalExceptionAttribute : Attribute, IExceptionFilter
    {
        
        public void OnException(ExceptionContext context)
        {
            var statusCode = 400;
            statusCode = context.Exception switch
            {
                DomainException domainException => domainException.Type switch
                {
                    DomainExceptionType.TrackAccessDenied => 403,
                    DomainExceptionType.IncorrectPassword => 401,
                    _ => statusCode
                },
                RepositoryException repositoryException => repositoryException.Type switch
                {
                    RepositoryExceptionType.LoginAlreadyExists => 400,
                    RepositoryExceptionType.EventNotFound => 404,
                    RepositoryExceptionType.TrackNotFound => 404,
                    RepositoryExceptionType.UserNotFound => 404,
                    RepositoryExceptionType.UserNotFoundByLogin => 404,
                    _ => statusCode
                },
                _ => statusCode
            };

            Log.Logger.Information("Exception occured: " + statusCode+ " " + context.Exception.Message);

            context.Result = new ObjectResult(context.Exception.Message)
            {
                StatusCode = statusCode,
            };
        }
    }
}