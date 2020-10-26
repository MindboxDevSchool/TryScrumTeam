using System;
using System.Collections.Generic;

namespace ItHappened.Domain
{
    public class TrackToEditDto
    {
        public TrackToEditDto(Guid id, string name, IEnumerable<CustomizationType> allowedCustomizations)
        {
            Id = id;
            Name = name;
            AllowedCustomizations = allowedCustomizations;
        }

        public readonly Guid Id;
        public readonly string Name;
        public readonly IEnumerable<CustomizationType> AllowedCustomizations;
    }
}