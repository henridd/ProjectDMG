using ProjectDMG.Api;
using ProjectDMG.Api.Notifications;
using System;
using System.Diagnostics;

namespace ProjectDMG.PokemonRedElasticsearchIntegration
{
    public class Bootstrapper : ProjectDMGPlugin
    {
        public override void Run()
        {
            var memoryWatcher = MemoryWatcherProvider.GetInstance();

            memoryWatcher.AddSubscription(MemoryAddresses.TurnNumber, null).ItemAdded += TurnNumberChanged;
            memoryWatcher.AddSubscription(MemoryAddresses.Pokemon1HP, null).ItemAdded += OnPlayerHPChanged;
            memoryWatcher.AddSubscription(MemoryAddresses.EnemyHP, null).ItemAdded += OnEnemyHPChanged;

        }

        private void TurnNumberChanged(object? sender, ItemAddedEventArgs<MemoryAddressUpdatedNotification> e)
        {
            var turnAddressUpdate = e.Item.AddressesValues[MemoryAddresses.TurnNumber];
            if(turnAddressUpdate.NewValue == 0)
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
