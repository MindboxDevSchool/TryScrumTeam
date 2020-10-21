using System;

namespace ItHappened.Domain.Exceptions
{
    public class DomainException:Exception
    {
        private DomainExceptionType Type { get; }
        private static string GetMessage(DomainExceptionType type, Guid userId, Guid entityId)
        {
            switch (type)
            {
                case DomainExceptionType.EventAccessDenied:
                    return $"User [{userId}] does not have permission to access the event [{entityId}]";
                case DomainExceptionType.TrackAccessDenied:
                    return $"User [{userId}] does not have permission to access the track [{entityId}]";
                default:
                    return "Unspecified exception occured, follow the link for additional info https://goo.su/2mvQ ";
            }
        }

        public DomainException(DomainExceptionType type, Guid authorId, Guid entityId)
            : base(GetMessage(type,authorId,entityId))
        {
            Type = type;
        }
    }
}