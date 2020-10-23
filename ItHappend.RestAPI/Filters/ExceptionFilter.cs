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
            int statusCode = 500;
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
                        statusCode = 406;
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
                        statusCode = 406;
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
            
            
            

            context.Result = new ObjectResult(context.Exception.Message)
            {
                StatusCode = statusCode,
            };
        }
    }
}