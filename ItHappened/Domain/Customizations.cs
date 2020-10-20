using ItHappened.Domain.CustomizationTypes;

namespace ItHappened.Domain
{
    public class Customizations
    {
        public Customizations(CommentCustomization comment, RatingCustomization rating, ScaleCustomization scale, PhotoCustomization photo, GeotagCustomization geotag)
        {
            Comment = comment;
            Rating = rating;
            Scale = scale;
            Photo = photo;
            Geotag = geotag;
        }

        public Customizations()
        {
            
        }

        public CommentCustomization Comment { get; }

        public RatingCustomization Rating { get; }

        public ScaleCustomization Scale { get; }

        public PhotoCustomization Photo { get; }

        public GeotagCustomization Geotag { get; }
    }
}