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

        public CommentCustomization Comment { get; }

        public RatingCustomization Rating { get; }

        public ScaleCustomization Scale { get; }

        public PhotoCustomization Photo { get; }

        public GeotagCustomization Geotag { get; }

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
    }
}