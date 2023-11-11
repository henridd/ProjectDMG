using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectDMG.Api.Models
{
    public readonly struct AddressRange : IEquatable<AddressRange>
    {
        public IEnumerable<ushort> MemoryAddresses { get; }

        public AddressRange(string hexAddress)
            : this(new ushort[] { Convert.ToUInt16(hexAddress, 16) })
        {

        }

        public AddressRange(ushort address)
            : this(new ushort[] { address })
        {

        }

        public AddressRange(IEnumerable<string> hexMemoryAddresses)
            : this(hexMemoryAddresses.Select(x => Convert.ToUInt16(x, 16)))
        {

        }

        public AddressRange(IEnumerable<ushort> memoryAddresses)
        {
            MemoryAddresses = memoryAddresses;
        }

        public static implicit operator AddressRange(ushort address) => new AddressRange(address);
        public static implicit operator AddressRange(string hexAddress) => new AddressRange(hexAddress);

        public static bool operator ==(AddressRange left, AddressRange right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(AddressRange left, AddressRange right)
        {
            return !(left == right);
        }

        public override bool Equals(object? obj)
        {
            return obj is AddressRange range && Equals(range);
        }

        public bool Equals(AddressRange other)
        {
            return MemoryAddresses.SequenceEqual(other.MemoryAddresses);
        }

        public override int GetHashCode()
        {
            int hash = 19;
            foreach (var address in MemoryAddresses)
            {
                hash = hash * 31 + address.GetHashCode();
            }
            return hash;
        }
    }
}
