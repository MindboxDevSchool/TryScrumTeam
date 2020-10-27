using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ItHappend.RestAPI.Models
{
    public class CreateEventRequest : IValidatableObject
    {
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public CustomizationsModel Customizations { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CreatedAt == new DateTime())
            {
                yield return new ValidationResult("CreatedAt is invalid or not not found in request body", new[] { "CreatedAt" });
            }
        }
    }
}