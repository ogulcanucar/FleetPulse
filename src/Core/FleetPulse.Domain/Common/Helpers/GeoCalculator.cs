using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetPulse.Domain.Common.Helpers
{
    public static class GeoCalculator
    {
        private const double EarthRadiusKm = 6371.0;

        /// <summary>
        /// İki coğrafi koordinat arasındaki mesafeyi metre cinsinden hesaplar (Haversine).
        /// </summary>
        public static double CalculateDistanceInMeters(double lat1, double lon1, double lat2, double lon2)
        {
            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return EarthRadiusKm * c * 1000; // Metreye çevir
        }

        private static double ToRadians(double deg) => deg * (Math.PI / 180.0);
    }
}
