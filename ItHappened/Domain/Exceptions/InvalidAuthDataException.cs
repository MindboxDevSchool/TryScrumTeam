using System;

namespace ItHappened.Domain.Exceptions
{
    public class InvalidAuthDataException : Exception
    {
        public InvalidAuthDataException(AuthData data)
            : base($"Invalid token for user: {data.Id.ToString()}")
        {
            
        }
    }
}