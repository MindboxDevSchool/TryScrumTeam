﻿namespace ItHappened.Domain.CustomizationTypes
{
    public class GeotagCustomization
    {
        public GeotagCustomization(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Latitude { get; }

        public double Longitude { get; }
    }
}