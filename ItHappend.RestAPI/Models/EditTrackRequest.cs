using System;

namespace ItHappend.RestAPI.Models
{
    public class EditTrackRequest
    {
        public  string Name { get; set; }
        public  DateTime CreatedAt { get; set; }
        public  string[] AllowedCustomizations { get; set; }
    }
}