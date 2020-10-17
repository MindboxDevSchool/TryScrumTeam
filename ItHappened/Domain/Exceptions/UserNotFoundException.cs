using System;

namespace ItHappened.Domain.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(Guid userId)
            : base($"User with id : {userId.ToString()} is not found")
        {
            
        }
    }
}