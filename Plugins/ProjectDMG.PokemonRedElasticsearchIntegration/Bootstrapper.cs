using ProjectDMG.Api;
using ProjectDMG.Api.Notifications;
using System.Diagnostics;

namespace ProjectDMG.PokemonRedElasticsearchIntegration
{
    public class Bootstrapper : ProjectDMGPlugin
    {
        private const string Pokemon1HP = "D16D";

        public override void Run()
        {
            MemoryWatcherProvider.GetInstance().AddSubscription(Pokemon1HP, null).ItemAdded += OnMoneyUpdated;
        }

        private void OnMoneyUpdated(object? sender, ItemAddedEventArgs<MemoryAddressUpdatedNotification> e)
        {
            Debug.WriteLine($"Pokemon1HP changed. {e.Item.AddressesValues[Pokemon1HP].PreviousValue} --> {e.Item.AddressesValues[Pokemon1HP].NewValue}");
        }
    }
}
