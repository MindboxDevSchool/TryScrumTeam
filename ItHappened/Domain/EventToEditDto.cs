using System;

namespace ItHappened.Domain
{
    public class EventToEditDto
    {
        public EventToEditDto(Event @event)
        {
            Id = @event.Id;
            CustomizationDto = @event.Customization.GetDto();
        }

        public EventToEditDto(Guid id, CustomizationsDto customizationDto)
        {
            Id = id;
            CustomizationDto = customizationDto;
        }

        public readonly Guid Id;
        public readonly CustomizationsDto CustomizationDto;
    }
}