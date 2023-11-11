using System.Collections.Generic;
using ProjectDMG.Api.Models;

namespace ProjectDMG.Api.Notification.Structs
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
