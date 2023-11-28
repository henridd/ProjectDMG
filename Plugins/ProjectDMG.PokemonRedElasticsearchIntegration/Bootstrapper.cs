using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using ProjectDMG.Api;
using ProjectDMG.Api.Models;
using ProjectDMG.Api.Notification.Structs;
using ProjectDMG.Api.Notifications;
using ProjectDMG.PokemonRedElasticsearchIntegration.Converters;
using ProjectDMG.PokemonRedElasticsearchIntegration.Elasticsearch;
using System;
using System.Linq;

namespace ProjectDMG.PokemonRedElasticsearchIntegration
{
    public class Bootstrapper : ProjectDMGPlugin
    {
        private AddressRange _enemyPokemonNameAddressRange;
        private ElasticsearchClient _elasticsearchClient;
        private PokedexIndexer _pokedexIndexer;
        private IMemoryWatcher _memoryWatcher;

        public override void Run()
        {
            Initialize();

            AddBattleStartedSubscription();
        }

        private void AddBattleStartedSubscription()
        {
            _enemyPokemonNameAddressRange = new AddressRange(new[]
                        {
                MemoryAddresses.EnemyName1,
                MemoryAddresses.EnemyName2,
                MemoryAddresses.EnemyName3,
                MemoryAddresses.EnemyName4,
                MemoryAddresses.EnemyName5,
                MemoryAddresses.EnemyName6,
                MemoryAddresses.EnemyName7,
                MemoryAddresses.EnemyName8,
                MemoryAddresses.EnemyName9,
                MemoryAddresses.EnemyName10,
            });

            var relevantAddresses = new AddressRange[]
            {
                MemoryAddresses.CurrentMap,
                MemoryAddresses.EnemyCatchRate,
                MemoryAddresses.EnemyWildLevel,
                MemoryAddresses.EnemyPrimaryType,
                MemoryAddresses.EnemySecondaryType,
                MemoryAddresses.BattleType
            };

            _memoryWatcher.AddSubscription(_enemyPokemonNameAddressRange, relevantAddresses).ItemAdded += PokemonNameChanged;
        }

        private void Initialize()
        {
            _memoryWatcher = MemoryWatcherProvider.GetInstance();
            _elasticsearchClient = CreateElasticsearchClient();
            _pokedexIndexer = new PokedexIndexer(_elasticsearchClient);
        }

        private ElasticsearchClient CreateElasticsearchClient()
        {
            var settings = new ElasticsearchClientSettings(new Uri("https://localhost:9200"))
                .ServerCertificateValidationCallback((_, _, _, _) => true) // Beware: very risky! 
                .Authentication(new BasicAuthentication("elastic", "elastic"));
            return new ElasticsearchClient(settings);
        }

        private void PokemonNameChanged(object? sender, ItemAddedEventArgs<MemoryAddressUpdatedNotification> e)
        {
            var pokemonInformation = CreatePokemonInformation(e.Item);

            if (string.IsNullOrWhiteSpace(pokemonInformation.PokemonName))
            {
                return;
            }

            _pokedexIndexer.Index(pokemonInformation);
        }

        private PokemonInformation CreatePokemonInformation(MemoryAddressUpdatedNotification notification)
        {
            var primaryType = GetPokemonType(notification, MemoryAddresses.EnemyPrimaryType)!;
            var secondaryType = GetPokemonType(notification, MemoryAddresses.EnemySecondaryType)!;

            if (secondaryType == primaryType)
            {
                secondaryType = null;
            }

            var battleType = notification.AddressesValues[MemoryAddresses.BattleType].NewValue.First();

            return new PokemonInformation(ByteToLocationNameConverter.Convert(notification.AddressesValues[MemoryAddresses.CurrentMap].NewValue.First()),
                ByteToCharConverter.Convert(notification.AddressesValues[_enemyPokemonNameAddressRange].NewValue),
                notification.AddressesValues[MemoryAddresses.EnemyWildLevel].NewValue.First(),
                notification.AddressesValues[MemoryAddresses.EnemyCatchRate].NewValue.First(),
                primaryType,
                secondaryType,
                battleType);
        }

        private string? GetPokemonType(MemoryAddressUpdatedNotification notification, AddressRange addressRange)
        {
            return ByteToPokemonTypeConverter.Convert(notification.AddressesValues[addressRange].NewValue.First());
        }
    }
}
