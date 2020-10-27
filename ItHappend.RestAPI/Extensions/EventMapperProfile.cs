using AutoMapper;
using ItHappend.RestAPI.Models;
using ItHappened.Domain;

namespace ItHappend.RestAPI.Extensions
{
    public class EventMapperProfile : Profile
    {
        public EventMapperProfile()
        {
            CreateMap<CustomizationsDto, CustomizationsModel>();
            CreateMap<CustomizationsModel, CustomizationsDto>();
            CreateMap<EventDto, EventModel>()
                .ForMember(
                    _ => _.Customizations,
                    opt =>
                        opt.MapFrom(s => s.CustomizationDto));
        }
    }
}