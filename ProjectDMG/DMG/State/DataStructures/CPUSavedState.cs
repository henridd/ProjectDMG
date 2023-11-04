using ProtoBuf;

namespace ProjectDMG.DMG.State.DataStructures
{
    [ProtoContract]
    public class CPUSavedState
    {
        [ProtoMember(1)]
        public byte A { get; set; }
        [ProtoMember(2)]
        public byte B { get; set; }
        [ProtoMember(3)]
        public byte C { get; set; }
        [ProtoMember(4)]
        public byte D { get; set; }
        [ProtoMember(5)]
        public byte E { get; set; }
        [ProtoMember(6)]
        public byte F { get; set; }
        [ProtoMember(7)]
        public byte H { get; set; }
        [ProtoMember(8)]
        public byte L { get; set; }
        [ProtoMember(9)]
        public ushort PC { get; set; }
        [ProtoMember(10)]
        public ushort SP { get; set; }
        [ProtoMember(11)]
        public bool IME { get; set; }
        [ProtoMember(12)]
        public bool IMEEnabler { get; set; }
        [ProtoMember(13)]
        public bool HALTED { get; set; }
        [ProtoMember(14)]
        public bool HALT_BUG { get; set; }
        [ProtoMember(15)]
        public int cycles { get; set; }
    }
}
