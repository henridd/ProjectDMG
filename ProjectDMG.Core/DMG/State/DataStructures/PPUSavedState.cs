using ProtoBuf;
using System;

namespace ProjectDMG.Core.DMG.State.DataStructures
{
    [ProtoContract]
    internal class PPUSavedState
    {
        [ProtoMember(1)]
        public int scanlineCounter { get; set; }
        [ProtoMember(2)]
        public int[] Bits { get; set; }
    }
}
