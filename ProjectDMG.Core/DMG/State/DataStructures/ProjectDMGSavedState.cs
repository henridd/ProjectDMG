using ProtoBuf;

namespace ProjectDMG.Core.DMG.State.DataStructures
{
    [ProtoContract]
    internal class ProjectDMGSavedState
    {
        [ProtoMember(1)]
        public int cyclesThisUpdate { get; set; }

    }
}
