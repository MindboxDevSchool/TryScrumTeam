using System;

namespace ItHappened.Domain.Exceptions
{
    public class RepositoryException:Exception
    {
        public RepositoryExceptionType Type { get; }

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
                default: 
                    return "Unspecified exception occured, follow the link for additional info https://goo.su/2mvQ ";
            }
        }

        private static string GetMessage(RepositoryExceptionType type, string login)
        {
            switch (type)
            {
                case RepositoryExceptionType.LoginAlreadyExists:
                    return $"User with login [{login}] already exists";
                case RepositoryExceptionType.UserNotFoundByLogin:
                    return $"User with login [{login}] not found in repository";
                default:
                    return "Unspecified exception occured, follow the link for additional info https://goo.su/2mvQ ";
            }
        }

        public RepositoryException(RepositoryExceptionType type, Guid entityId)
            : base(GetMessage(type,entityId))
        {
            Type = type;
        }

        public RepositoryException(RepositoryExceptionType type, string login)
            : base(GetMessage(type, login))
        {
            Type = type;
        }
    }
}