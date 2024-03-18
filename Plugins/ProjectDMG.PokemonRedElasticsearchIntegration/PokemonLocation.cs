using Elastic.Clients.Elasticsearch;

namespace ProjectDMG.PokemonRedElasticsearchIntegration
{
    internal readonly struct PokemonLocation
    {
        public string MapName { get; }

        public GeoLocation? GeoLocation { get; }

        public PokemonLocation(string mapName, GeoLocation? geoLocation)
        {
            MapName = mapName;
            GeoLocation = geoLocation;
        }
    }
}
