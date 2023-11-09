using System;
using System.Collections.Generic;

namespace ProjectDMG.Api.Notifications
{
    public readonly struct MemoryAddressSubscription
    {
        public int Id { get; }
        public AddressRange SubscribedAddresses { get; }
        public IEnumerable<AddressRange> RelevantAddresses { get; }

        public MemoryAddressSubscription(int id, AddressRange subscribedAddresses, IEnumerable<AddressRange> relevantAddresses)
        {
            Id = id;
            SubscribedAddresses = subscribedAddresses;
            RelevantAddresses = relevantAddresses ?? Array.Empty<AddressRange>();
        }
    }
}
