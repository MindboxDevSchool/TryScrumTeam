using System;

namespace ItHappened.Domain.Exceptions
{
    public class DomainException:Exception
    {
        private static string GetMessage(DomainExceptionType type, Guid userId, Guid entityId)
        {
            switch (type)
            {
                case DomainExceptionType.EventAccessDenied:
                    return $"User [{userId}] does not have permission to access the event [{entityId}]";
                case DomainExceptionType.TrackAccessDenied:
                    return $"User [{userId}] does not have permission to access the track [{entityId}]";
                default: return "default";
            }
        }

        public DomainException(DomainExceptionType type, Guid authorId, Guid entityId)
            : base(GetMessage(type,authorId,entityId))
        {
            
        }
    }
}