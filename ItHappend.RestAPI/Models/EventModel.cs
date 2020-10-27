using System;

namespace ItHappend.RestAPI.Models
{
    public class EventModel
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public CustomizationsModel Customizations { get; set; }
    }
}