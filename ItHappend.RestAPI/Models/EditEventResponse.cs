using System;

namespace ItHappend.RestAPI.Models
{
    public class EditEventResponse
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public CustomizationsModel Customizations { get; set; }
    }
}