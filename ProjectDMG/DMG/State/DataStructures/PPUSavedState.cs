using ProtoBuf;
using System;

namespace ProjectDMG.DMG.State.DataStructures
{
    [ProtoContract]
    internal class PPUSavedState
    {
        [ProtoMember(1)]
        public int scanlineCounter { get; set; }
        [ProtoMember(2)]
        public Int32[] Bits { get; set; }
    }
}
