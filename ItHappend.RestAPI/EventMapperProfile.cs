using System;
using AutoMapper;
using ItHappend.RestAPI.Models;
using ItHappened.Domain;

namespace ItHappend.RestAPI
{
    public class EventMapperProfile : Profile
    {
        public EventMapperProfile()
        {
            CreateMap<CustomizationsDto, CustomizationsModel>();
            CreateMap<CustomizationsModel, CustomizationsDto>();
            CreateMap<EventDto, GetEventsResponseItem>()
                .ForMember(
                    _ => _.Customizations,
                    opt =>
                        opt.MapFrom(s => s.CustomizationDto));

            CreateMap<EventDto, CreateEventResponse>()
                .ForMember(
                    _ => _.Customizations,
                    opt =>
                        opt.MapFrom(s => s.CustomizationDto));
            CreateMap<EventDto, EditEventResponse>()
                .ForMember(
                    _ => _.Customizations,
                    opt =>
                        opt.MapFrom(s => s.CustomizationDto));
        }
    }
}