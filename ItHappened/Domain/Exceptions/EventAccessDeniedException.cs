using System;

namespace ItHappened.Domain.Exceptions
{
    public class EventAccessDeniedException:Exception
    {
        public EventAccessDeniedException(Guid authorId, Guid eventId)
            : base($"User [{authorId}] does not have permission to access the event [{eventId}]")
        {
            
        }
    }
}