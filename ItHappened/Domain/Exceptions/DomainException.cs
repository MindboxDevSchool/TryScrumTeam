using System;
using System.Collections.Generic;

namespace ItHappened.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public DomainExceptionType Type { get; }

        private static string GetMessage(DomainExceptionType type, Guid userId, Guid entityId)
        {
            switch (type)
            {
                case DomainExceptionType.TrackAccessDenied:
                    return $"User [{userId}] does not have permission to access the track [{entityId}]";
                default:
                    return "Unspecified exception occured, follow the link for additional info https://goo.su/2mvQ ";
            }
        }

        private static string GetMessage(DomainExceptionType type, string login)
        {
            switch (type)
            {
                case DomainExceptionType.IncorrectPassword:
                    return $"Incorrect password for user with login [{login}]";
                default:
                    return "Unspecified exception occured, follow the link for additional info https://goo.su/2mvQ ";
            }
        }

        private static string GetMessage(
            DomainExceptionType type,
            Guid trackId,
            IEnumerable<CustomizationType> customizationTypes)
        {
            switch (type)
            {
                case DomainExceptionType.NotAllowedCustomizations:
                    string customizations = "";
                    foreach (var c in customizationTypes)
                    {
                        customizations += c.ToString() + " ";
                    }

                    return $"Customizations [{customizations}] not allowed in track [{trackId}]";
                default:
                    return "Unspecified exception occured, follow the link for additional info https://goo.su/2mvQ ";
            }
        }

        public DomainException(DomainExceptionType type, Guid authorId, Guid entityId)
            : base(GetMessage(type, authorId, entityId))
        {
            Type = type;
        }

        public DomainException(DomainExceptionType type, string login)
            : base(GetMessage(type, login))
        {
            Type = type;
        }

        public DomainException(
            DomainExceptionType type,
            Guid trackId,
            IEnumerable<CustomizationType> customizationTypes)
            : base(GetMessage(type, trackId, customizationTypes))
        {
            Type = type;
        }
    }
}