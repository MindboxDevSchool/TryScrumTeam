using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain.CustomizationTypes;

namespace ItHappened.Domain
{
    public class Customizations
    {
        public Customizations()
        {
        }

        public Customizations(CustomizationsDto customizationsDto, IEnumerable<CustomizationType> allowedCustoms)
        {
            if (allowedCustoms.Contains(CustomizationType.Comment))
            {
                if (customizationsDto.Comment != null)
                    Comment = new CommentCustomization(customizationsDto.Comment);
            }

            if (allowedCustoms.Contains(CustomizationType.Rating))
            {
                if (customizationsDto.Rating != null)
                    Rating = new RatingCustomization((int) customizationsDto.Rating);
            }

            if (allowedCustoms.Contains(CustomizationType.Photo))
            {
                if (customizationsDto.PhotoUrl != null)
                    Photo = new PhotoCustomization(customizationsDto.PhotoUrl);
            }

            if (allowedCustoms.Contains(CustomizationType.Scale))
            {
                if (customizationsDto.Scale != null)
                    Scale = new ScaleCustomization((double) customizationsDto.Scale);
            }

            if (allowedCustoms.Contains(CustomizationType.Geotag))
            {
                if (customizationsDto.GeotagLatitude != null && customizationsDto.GeotagLongitude != null)
                    Geotag = new GeotagCustomization(
                        (double) customizationsDto.GeotagLatitude,
                        (double) customizationsDto.GeotagLongitude);
            }
        }

        public CommentCustomization Comment { get; set; }

        public RatingCustomization Rating { get; set;}

        public ScaleCustomization Scale { get; set;}

        public PhotoCustomization Photo { get; set;}

        public GeotagCustomization Geotag { get; set; }

        public CustomizationsDto GetDto()
        {
            var customizationDto = new CustomizationsDto();
            if (Comment != null)
                customizationDto.Comment = Comment.Value;
            if (Rating != null)
                customizationDto.Rating = Rating.Value;
            if (Scale != null)
                customizationDto.Scale = Scale.Value;
            if (Photo != null)
                customizationDto.PhotoUrl = Photo.Value;
            if (Geotag != null)
            {
                customizationDto.GeotagLongitude = Geotag.Longitude;
                customizationDto.GeotagLatitude = Geotag.Latitude;
            }

            return customizationDto;
        }

        public static IEnumerable<CustomizationType> GetCustomizationTypes(CustomizationsDto customizationsDto)
        {
            var types = new List<CustomizationType>();
            if (customizationsDto.Comment != null)
                types.Add(CustomizationType.Comment);
            if (customizationsDto.Rating != null)
                types.Add(CustomizationType.Rating);
            if (customizationsDto.Scale != null)
                types.Add(CustomizationType.Scale);
            if (customizationsDto.PhotoUrl != null)
                types.Add(CustomizationType.Photo);
            if (customizationsDto.GeotagLatitude != null && customizationsDto.GeotagLongitude != null)
                types.Add(CustomizationType.Geotag);
            return types;
        }
    }
}