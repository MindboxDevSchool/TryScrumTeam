using System;

namespace ItHappened.Domain.Exceptions
{
    public class TrackAccessDeniedException: Exception
    {
        public TrackAccessDeniedException(Guid authorId, Guid trackId)
        : base($"User [{authorId}] does not have permission to access the track [{trackId}]")
        {
            
        }

        public TrackAccessDeniedException(string message): base(message)
        {
            
        }
    }
}