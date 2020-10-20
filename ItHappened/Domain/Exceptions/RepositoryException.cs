using System;

namespace ItHappened.Domain.Exceptions
{
    public class RepositoryException:Exception
    {
        private static string GetMessage(RepositoryExceptionType type, Guid entityId)
        {
            switch (type)
            {
                case RepositoryExceptionType.TrackNotFound:
                    return $"Track [{entityId}] not found in repository";
                case RepositoryExceptionType.EventNotFound:
                    return $"Event [{entityId}] not found in repository";
                case RepositoryExceptionType.UserNotFound:
                    return $"User [{entityId}] not found in repository";
                default: return "default";
            }
        }

        public RepositoryException(RepositoryExceptionType type, Guid entityId)
            : base(GetMessage(type,entityId))
        {
            
        }
    }
}