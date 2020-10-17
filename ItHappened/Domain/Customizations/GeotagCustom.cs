namespace ItHappened.Domain.Customizations
{
    public class GeotagCustom
    {
        public GeotagCustom(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Latitude { get; }

        public double Longitude { get; }
    }
}