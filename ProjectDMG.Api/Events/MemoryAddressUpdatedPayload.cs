using System.Collections.Generic;

namespace ProjectDMG.Api.Events
{
    public class MemoryAddressUpdatedPayload
    {
        public Dictionary<ushort, byte> Values { get; set; }

        public MemoryAddressUpdatedPayload(Dictionary<ushort, byte> values)
        {
            Values = values;
        }
    }
}
