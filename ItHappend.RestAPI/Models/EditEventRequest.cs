using System;

namespace ItHappend.RestAPI.Models
{
    public class EditEventRequest
    {
        public DateTime CreatedAt { get; set; }
        public CustomizationsModel Customizations { get; set; }
    }
}