using Elastic.Clients.Elasticsearch;

namespace ProjectDMG.PokemonRedElasticsearchIntegration.Converters
{
    internal class ByteToLatLonGeoLocationConverter
    {
        internal static LatLonGeoLocation? Convert(byte value)
        {
            switch (value)
            {
                // Route 22
                case 33: return new LatLonGeoLocation() { Lat = -0.20, Lon = -78.72 };
                default: return null;
            }
        }
    }
}
