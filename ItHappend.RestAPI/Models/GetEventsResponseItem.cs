using System;
using ItHappened.Domain;

namespace ItHappend.RestAPI.Models
{
    public class GetEventsResponseItem
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public CustomizationsModel Customizations { get; set; }
    }
}