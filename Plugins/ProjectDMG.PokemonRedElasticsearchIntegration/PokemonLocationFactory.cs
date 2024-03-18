using Elastic.Clients.Elasticsearch;
using ProjectDMG.PokemonRedElasticsearchIntegration.Converters;
using System;

namespace ProjectDMG.PokemonRedElasticsearchIntegration
{
    internal static class PokemonLocationFactory
    {
        internal static PokemonLocation Create(byte id)
        {
            var mapName = ByteToLocationNameConverter.Convert(id);
            var latLonGeoLocation = ByteToLatLonGeoLocationConverter.Convert(id);

            GeoLocation? geoLocation = null;
            if(latLonGeoLocation != null)
            {
                ScrambleLatLonGeoLocation(latLonGeoLocation);
                geoLocation = GeoLocation.LatitudeLongitude(latLonGeoLocation);
            }

            return new PokemonLocation(mapName, geoLocation);
        }

        private static void ScrambleLatLonGeoLocation(LatLonGeoLocation latLonGeoLocation)
        {
            const double latLimit = 2.5;
            const double lonLimit = 5.0;

            latLonGeoLocation.Lat = NextDouble(latLonGeoLocation.Lat - latLimit, latLonGeoLocation.Lat + latLimit);
            latLonGeoLocation.Lon = NextDouble(latLonGeoLocation.Lon - lonLimit, latLonGeoLocation.Lon + lonLimit);

            double NextDouble(double minValue, double maxValue)
            {
                return new Random().NextDouble() * (maxValue - minValue) + minValue;
            }
        }
    }
}
