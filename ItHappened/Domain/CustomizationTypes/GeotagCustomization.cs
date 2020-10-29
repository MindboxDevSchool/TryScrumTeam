namespace ItHappened.Domain.CustomizationTypes
{
    public class GeotagCustomization
    {
        public GeotagCustomization(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Latitude { get; set;}

        public double Longitude { get; set;}
    }
}