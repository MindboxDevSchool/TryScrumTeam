using System;

namespace ItHappened.Domain
{
    public class EventDto
    {
        public EventDto(Event @event)
        {
            Id = @event.Id;
            CreatedAt = @event.CreatedAt;
            TrackId = @event.TrackId;
            CustomizationDto = @event.Customization.GetDto();
        }

        public EventDto(Guid id, DateTime createdAt, Guid trackId, CustomizationsDto customizationDto)
        {
            Id = id;
            CreatedAt = createdAt;
            TrackId = trackId;
            CustomizationDto = customizationDto;
        }

        public readonly Guid Id;
        public readonly DateTime CreatedAt;
        public readonly Guid TrackId;
        public readonly CustomizationsDto CustomizationDto;
    }
}