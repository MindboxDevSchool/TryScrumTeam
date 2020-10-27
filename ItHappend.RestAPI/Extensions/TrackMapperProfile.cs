using System;
using System.Collections.Generic;
using AutoMapper;
using ItHappend.RestAPI.Models;
using ItHappened.Domain;

namespace ItHappend.RestAPI.Extensions
{
    public class CustomizationsTypeToStringConverter : ITypeConverter<CustomizationType, string>
    {
        public string Convert(CustomizationType source, string destination, ResolutionContext context)
        {
            return source.ToString();
        }
    }

    public class StringToCustomizationsTypeConverter : ITypeConverter<string, CustomizationType>
    {
        public CustomizationType Convert(string source, CustomizationType destination, ResolutionContext context)
        {
            // can throw ArgumentException
            return Enum.Parse<CustomizationType>(source);
        }
    }

    public class TrackMapperProfile : Profile
    {
        private static Dictionary<string, CustomizationType> _customizations =
            new Dictionary<string, CustomizationType>();

        public TrackMapperProfile()
        {
            CreateMap<CustomizationType, string>().ConvertUsing(new CustomizationsTypeToStringConverter());
            CreateMap<string, CustomizationType>().ConvertUsing(new StringToCustomizationsTypeConverter());
            CreateMap<TrackDto, TrackModel>()
                .ForMember(
                    _ => _.AllowedCustomizations,
                    opt =>
                        opt.MapFrom(s => s.AllowedCustomizations));
        }
    }
}