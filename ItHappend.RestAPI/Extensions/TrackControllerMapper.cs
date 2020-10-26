using System;
using System.Collections.Generic;
using System.Linq;
using ItHappend.RestAPI.Models;
using ItHappened.Domain;

namespace ItHappend.RestAPI.Extensions
{
    public static class TrackControllerMapper
    {
        public static IEnumerable<GetTracksResponse> Map(this IEnumerable<TrackDto> tracks)
        {
            return tracks.Select(track => new GetTracksResponse()
            {
                Id = track.Id,
                Name = track.Name,
                CreatedAt = track.CreatedAt,
                CreatorId = track.CreatorId,
                AllowedCustomizations = track.AllowedCustomizations.Map()
            });
        }

        public static CreateTrackResponse Map(this TrackDto track)
        {
            return new CreateTrackResponse()
            {
                Id = track.Id,
                Name = track.Name,
                CreatedAt = track.CreatedAt,
                CreatorId = track.CreatorId,
                AllowedCustomizations = track.AllowedCustomizations.Map()
            };
        }

        public static TrackDto Map(this EditTrackRequest trackRequest)
        {
            return new TrackDto(
                trackRequest.Id,
                trackRequest.Name,
                trackRequest.CreatedAt,
                trackRequest.CreatorId,
                trackRequest.AllowedCustomizations.Map()
            );
        }

        public static EditTrackResponse MapToResponse(this TrackDto track)
        {
            return new EditTrackResponse()
            {
                Id = track.Id,
                Name = track.Name,
                CreatedAt = track.CreatedAt,
                CreatorId = track.CreatorId,
                AllowedCustomizations = track.AllowedCustomizations.Map()
            };
        }

        public static string[] Map(this IEnumerable<CustomizationType> customizationTypes)
        {
            return customizationTypes.Select(customizationType =>
            {
                switch (customizationType)
                {
                    case CustomizationType.Comment: return "Comment";
                    case CustomizationType.Photo: return "Photo";
                    case CustomizationType.Geotag: return "Geotag";
                    case CustomizationType.Rating: return "Rating";
                    case CustomizationType.Scale: return "Scale";
                    default: return "Unrecognized type";
                }
            }).ToArray();
        }

        public static IEnumerable<CustomizationType> Map(this string[] customizationTypes)
        {
            return customizationTypes.Select(customizationType =>
            {
                switch (customizationType)
                {
                    case "Comment": return CustomizationType.Comment;
                    case "Photo": return CustomizationType.Photo;
                    case "Geotag": return CustomizationType.Geotag;
                    case "Scale": return CustomizationType.Scale;
                    default: return CustomizationType.Rating;
                }
            }).ToArray();
        }
    }
}