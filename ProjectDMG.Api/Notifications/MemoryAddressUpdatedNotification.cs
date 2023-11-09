using System.Collections.Generic;

namespace ProjectDMG.Api.Notifications
{
    public readonly struct MemoryAddressUpdatedNotification
    {
        public Dictionary<AddressRange, MemoryAddressValueUpdate> AddressesValues { get; }

        public MemoryAddressUpdatedNotification(Dictionary<AddressRange, MemoryAddressValueUpdate> addressesValues)
        {
            AddressesValues = addressesValues;
        }
    }
}
