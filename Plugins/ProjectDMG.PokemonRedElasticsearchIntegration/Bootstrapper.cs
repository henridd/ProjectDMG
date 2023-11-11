using ProjectDMG.Api;
using ProjectDMG.Api.Notifications;
using ProjectDMG.PokemonRedElasticsearchIntegration.Converters;
using System.Diagnostics;
using System.Linq;

namespace ProjectDMG.PokemonRedElasticsearchIntegration
{
    public class Bootstrapper : ProjectDMGPlugin
    {
        private AddressRange _enemyPokemonNameAddressRange;
        private AddressRange _moneyAddressRange;

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

            _moneyAddressRange = new AddressRange(new[]
            {
                MemoryAddresses.Money1,
                MemoryAddresses.Money2,
                MemoryAddresses.Money3
            });
            memoryWatcher.AddSubscription(_moneyAddressRange, null).ItemAdded += MoneyChanged;
        }

        private void MoneyChanged(object? sender, ItemAddedEventArgs<MemoryAddressUpdatedNotification> e)
        {
            var newHexValues = e.Item.AddressesValues[_moneyAddressRange].NewValue.Select(x => x.ToString("X"));
            var moneyInString = string.Join(string.Empty, newHexValues);

            Debug.WriteLine($"Your money has changed! New value: {int.Parse(moneyInString)}");
        }

        private void EnemyNameChanged(object? sender, ItemAddedEventArgs<MemoryAddressUpdatedNotification> e)
        {
            Debug.WriteLine($"Enemy found: {ByteToCharConverter.Convert(e.Item.AddressesValues[_enemyPokemonNameAddressRange].NewValue)}" +
                $" on map {ByteToLocationNameConverter.Convert(e.Item.AddressesValues[MemoryAddresses.CurrentMap].NewValue.First())}");
        }
    }
}
