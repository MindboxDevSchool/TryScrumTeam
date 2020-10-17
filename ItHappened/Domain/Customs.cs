using ItHappened.Domain.Customizations;

namespace ItHappened.Domain
{
    public class Customs
    {
        public Customs(CommentCustom comment, RatingCustom rating, ScaleCustom scale, PhotoCustom photo, GeotagCustom geotag)
        {
            Comment = comment;
            Rating = rating;
            Scale = scale;
            Photo = photo;
            Geotag = geotag;
        }

        public CommentCustom Comment { get; }

        public RatingCustom Rating { get; }

        public ScaleCustom Scale { get; }

        public PhotoCustom Photo { get; }

        public GeotagCustom Geotag { get; }
    }
}