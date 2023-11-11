using System.Collections.Generic;

namespace ProjectDMG.Api.Models
{
    public readonly struct AddressRangeValue
    {
        public AddressRange AddressRange { get; }
        public IEnumerable<byte> Values { get; }

        public AddressRangeValue(AddressRange addressRange, IEnumerable<byte> values)
        {
            AddressRange = addressRange;
            Values = values;
        }
    }
}
