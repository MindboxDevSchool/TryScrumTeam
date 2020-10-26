using System;

namespace ItHappend.RestAPI.Models
{
    public class EditTrackRequest
    {
        public  Guid Id { get; set; }
        public  string Name { get; set; }
        public  DateTime CreatedAt { get; set; }
        public  Guid CreatorId { get; set; }
        public  string[] AllowedCustomizations { get; set; }
    }
}