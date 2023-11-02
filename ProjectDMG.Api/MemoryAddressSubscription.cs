using System.Collections.Generic;

namespace ProjectDMG.Api
{
    public readonly struct MemoryAddressSubscription
    {
        public int Id { get; }
        public ushort SubscribedAddress { get; }
        public IEnumerable<ushort> RelevantAddresses { get; }

        public MemoryAddressSubscription(int id, ushort subscribedAddress, IEnumerable<ushort> relevantAddresses)
        {
            Id = id;
            SubscribedAddress = subscribedAddress;
            RelevantAddresses = relevantAddresses ?? new ushort[0];
        }
    }
}
