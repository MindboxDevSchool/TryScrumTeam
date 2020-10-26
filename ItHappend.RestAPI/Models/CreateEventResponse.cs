using System;

namespace ItHappend.RestAPI.Models
{
    public class CreateEventResponse
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public CustomizationsModel Customizations { get; set; }
    }
}