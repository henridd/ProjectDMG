using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.IndexManagement;

namespace ProjectDMG.PokemonRedElasticsearchIntegration.Elasticsearch
{
    internal class PokedexIndexer
    {
        private readonly ElasticsearchClient _elasticsearchClient;
        private const string _indexName = "pokemons";

        public PokedexIndexer(ElasticsearchClient elasticsearchClient)
        {
            _elasticsearchClient = elasticsearchClient;

            CreateIndex();
        }

        private void CreateIndex()
        {
            if (_elasticsearchClient.Indices.Exists(_indexName).Exists)
            {
                return;
            }

            var createIndexRequest = new CreateIndexRequestDescriptor(_indexName)
                .Mappings(m =>
                    m.Properties<PokemonInformation>(prop =>
                        prop.Keyword(pkm => pkm.FoundAt)
                            .GeoPoint(pkm => pkm.GeoLocation)
                            .Keyword(pkm => pkm.PokemonName) // Not using text so it can be used as a label
                            .ByteNumber(pkm => pkm.Number)
                            .IntegerNumber(pkm => pkm.CatchRate)
                            .IntegerNumber(pkm => pkm.Level)
                            .Keyword(pkm => pkm.PrimaryType)
                            .Keyword(pkm => pkm.SecondaryType)
                            .Boolean(pkm => pkm.IsWildPokemon)
                            .Boolean(pkm => pkm.IsTrainerBattle)));

            _elasticsearchClient.Indices.Create(createIndexRequest);
        }

        internal void Index(PokemonInformation pokemonInformation)
        {
            _elasticsearchClient.Index(pokemonInformation, _indexName);
        }
    }
}
