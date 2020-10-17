using System;
using ItHappened.Domain.Customizations;

namespace ItHappened.Domain
{
    public class Customs
    {
        public Customs(string comment, RatingCustom rating, int scale, byte[] photo, GeotagCustom geotag)
        {
            Comment = comment ?? throw new ArgumentNullException(nameof(comment));
            Rating = rating ?? throw new ArgumentNullException(nameof(rating));
            Scale = scale;
            Photo = photo ?? throw new ArgumentNullException(nameof(photo));
            Geotag = geotag ?? throw new ArgumentNullException(nameof(geotag));
        }

        public string Comment { get; }

        public RatingCustom Rating { get; }

        public int Scale { get; }

        public byte[] Photo { get; }

        public GeotagCustom Geotag { get; }
    }
}