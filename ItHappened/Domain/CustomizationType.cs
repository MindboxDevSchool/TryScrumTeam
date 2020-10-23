using System.Collections.Generic;

namespace ItHappened.Domain
{
    public enum CustomizationType
    {
        Photo,
        Rating,
        Scale,
        Geotag,
        Comment
    }

    public static class CustomizationTypeExtensions
    {
        public static string CreateString(this IEnumerable<CustomizationType> types)
        {
            string customizations = "";
            foreach (var c in types)
            {
                customizations += c.ToString() + " ";
            }
            return customizations;
        }
    }
}