using System.Collections.Generic;

namespace ProjectDMG.Api.Notifications
{
    public readonly struct MemoryAddressUpdatedNotification
    {
        public Dictionary<string, MemoryAddressValueUpdate> AddressesValues { get; }

        public MemoryAddressUpdatedNotification(Dictionary<string, MemoryAddressValueUpdate> addressesValues)
        {
            AddressesValues = addressesValues;
        }
    }
}
