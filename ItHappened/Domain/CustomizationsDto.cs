namespace ItHappened.Domain
{
    public class CustomizationsDto
    {
        public string Comment { get; set; }

        public double? GeotagLatitude { get; set; }

        public double? GeotagLongitude { get; set; }

        public string PhotoUrl { get; set; }

        public int? Rating { get; set; }

        public double? Scale { get; set; }
    }
}