using System.Collections.Generic;
using System.Linq;

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
        public static string CreateString(this IEnumerable<CustomizationType> AllowedCustomizations)
        {
            var allowedCustomizations =
                string.Join(" ", AllowedCustomizations.Select(s => s.ToString()).ToArray());
            return allowedCustomizations;
        }
    }
}