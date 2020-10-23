using System;

namespace ItHappened.Domain.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message) : base(message)
        {
        }
    }

    public class BadDataException : Exception
    {
        public BadDataException(string message) : base(message)
        {
        }   
    }}