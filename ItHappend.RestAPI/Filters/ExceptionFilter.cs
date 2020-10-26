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
            int statusCode = 400;
            if(context.Exception is DomainException)
            {
                DomainException exception;
                exception = context.Exception as DomainException;
                switch (exception.Type)
                {
                    case DomainExceptionType.TrackAccessDenied:
                        statusCode = 403;
                        break;
                    case DomainExceptionType.IncorrectPassword:
                        statusCode = 401;
                        break;
                }
            }
            if(context.Exception is RepositoryException)
            {
                RepositoryException exception;
                exception = context.Exception as RepositoryException;
                switch (exception.Type)
                {
                    case RepositoryExceptionType.LoginAlreadyExists :
                        statusCode = 400;
                        break;
                    case RepositoryExceptionType.EventNotFound :
                        statusCode = 404;
                        break;
                    case RepositoryExceptionType.TrackNotFound :
                        statusCode = 404;
                        break;
                    case RepositoryExceptionType.UserNotFound :
                        statusCode = 404;
                        break;
                    case RepositoryExceptionType.UserNotFoundByLogin :
                        statusCode = 404;
                        break;
                }
            }
            
            Log.Logger.Warning("Exception occured: " + statusCode+ " " + context.Exception.Message);

            context.Result = new ObjectResult(context.Exception.Message)
            {
                StatusCode = statusCode,
            };
        }
    }
}