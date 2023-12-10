using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using ProjectDMG.Api;
using ProjectDMG.Api.Models;
using ProjectDMG.Api.Notification.Structs;
using ProjectDMG.Api.Notifications;
using ProjectDMG.PokemonRedElasticsearchIntegration.Converters;
using ProjectDMG.PokemonRedElasticsearchIntegration.Elasticsearch;
using ProjectDMG.PokemonRedElasticsearchIntegration.Pokedex;
using System;
using System.Linq;
using System.Threading;

namespace ProjectDMG.PokemonRedElasticsearchIntegration
{
    public class Bootstrapper : ProjectDMGPlugin
    {
        private AddressRange _enemyPokemonNameAddressRange;
        private ElasticsearchClient _elasticsearchClient;
        private PokedexIndexer _pokedexIndexer;
        private PokedexWindow _pokedexWindow;
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
            InitializeHtmlPokedex();
        }

        private void InitializeHtmlPokedex()
        {
            _pokedexWindow = new PokedexWindow();
            _pokedexWindow.Show();
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

            _pokedexWindow.UpdatePokemon(pokemonInformation);

            if (pokemonInformation == null)
            {
                return;
            }

            _pokedexIndexer.Index(pokemonInformation.Value);
        }

        private PokemonInformation? CreatePokemonInformation(MemoryAddressUpdatedNotification notification)
        {
            var pokemonName = ByteToCharConverter.Convert(notification.AddressesValues[_enemyPokemonNameAddressRange].NewValue).Trim();

            if (string.IsNullOrWhiteSpace(pokemonName))
            {
                return null;
            }

            var primaryType = GetPokemonType(notification, MemoryAddresses.EnemyPrimaryType)!;
            var secondaryType = GetPokemonType(notification, MemoryAddresses.EnemySecondaryType)!;

            if (secondaryType == primaryType)
            {
                secondaryType = null;
            }

            var battleType = notification.AddressesValues[MemoryAddresses.BattleType].NewValue.First();

            return new PokemonInformation(ByteToLocationNameConverter.Convert(notification.AddressesValues[MemoryAddresses.CurrentMap].NewValue.First()),
                pokemonName,
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

        public override void Dispose()
        {
            _pokedexWindow?.Close();
        }
    }
}
