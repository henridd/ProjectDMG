using ProjectDMG.Api;
using ProjectDMG.Api.Notifications;
using System.Diagnostics;
using System.Linq;

namespace ProjectDMG.PokemonRedElasticsearchIntegration
{
    public class Bootstrapper : ProjectDMGPlugin
    {
        private AddressRange _enemyPokemonNameAddressRange;

        public override void Run()
        {
            var memoryWatcher = MemoryWatcherProvider.GetInstance();

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
            memoryWatcher.AddSubscription(_enemyPokemonNameAddressRange, new AddressRange[1] { MemoryAddresses.CurrentMap }).ItemAdded += EnemyNameChanged;

            var moneyRange = new AddressRange(new[]
            {
                MemoryAddresses.Money1,
                MemoryAddresses.Money2,
                MemoryAddresses.Money3
            });
            memoryWatcher.AddSubscription(moneyRange, null).ItemAdded += MoneyChanged;

            memoryWatcher.AddSubscription(MemoryAddresses.TurnNumber, null).ItemAdded += TurnNumberChanged;
            memoryWatcher.AddSubscription(MemoryAddresses.Pokemon1HP, null).ItemAdded += OnPlayerHPChanged;
            memoryWatcher.AddSubscription(MemoryAddresses.EnemyHP, null).ItemAdded += OnEnemyHPChanged;

        }

        private void MoneyChanged(object? sender, ItemAddedEventArgs<MemoryAddressUpdatedNotification> e)
        {
            Debug.WriteLine("Money name changed");
        }

        private void EnemyNameChanged(object? sender, ItemAddedEventArgs<MemoryAddressUpdatedNotification> e)
        {
            Debug.WriteLine($"Enemy name changed: {ByteToCharConverter.Convert(e.Item.AddressesValues[_enemyPokemonNameAddressRange].NewValue)}" +
                $" on map {ByteToLocationNameConverter.Convert(e.Item.AddressesValues[MemoryAddresses.CurrentMap].NewValue.First())}");
        }

        private void TurnNumberChanged(object? sender, ItemAddedEventArgs<MemoryAddressUpdatedNotification> e)
        {
            var turnAddressUpdate = e.Item.AddressesValues[MemoryAddresses.TurnNumber];
            if (turnAddressUpdate.NewValue.First() == 0)
            {
                Debug.WriteLine("Battle started!");
                return;
            }

            Debug.WriteLine($"Starting turn {turnAddressUpdate.NewValue}");
        }

        private void OnEnemyHPChanged(object? sender, ItemAddedEventArgs<MemoryAddressUpdatedNotification> e)
        {
            Debug.WriteLine($"EnemyHP changed. {e.Item.AddressesValues[MemoryAddresses.EnemyHP].PreviousValue} --> {e.Item.AddressesValues[MemoryAddresses.EnemyHP].NewValue}");
        }

        private void OnPlayerHPChanged(object? sender, ItemAddedEventArgs<MemoryAddressUpdatedNotification> e)
        {
            Debug.WriteLine($"Pokemon1HP changed. {e.Item.AddressesValues[MemoryAddresses.Pokemon1HP].PreviousValue} --> {e.Item.AddressesValues[MemoryAddresses.Pokemon1HP].NewValue}");
        }
    }
}
