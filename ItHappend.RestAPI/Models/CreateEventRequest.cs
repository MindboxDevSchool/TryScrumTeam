using System;

namespace ItHappend.RestAPI.Models
{
    public class CreateEventRequest
    {
        public DateTime CreatedAt { get; set; }
        public CustomizationsModel Customizations { get; set; }
    }
}