using ProtoBuf;

namespace ProjectDMG.Core.DMG.State.DataStructures.GamePak
{
    [ProtoContract]
    internal class MBC3SavedState : GamePakSavedState
    {
        [ProtoMember(2)]
        public byte[] ERAM { get; set; }
        [ProtoMember(3)]
        public bool ERAM_ENABLED { get; set; }
        [ProtoMember(4)]
        public int ROM_BANK { get; set; }
        [ProtoMember(5)]
        public int RAM_BANK { get; set; }
        [ProtoMember(6)]
        public byte RTC_S { get; set; }
        [ProtoMember(7)]
        public byte RTC_M { get; set; }
        [ProtoMember(8)]
        public byte RTC_H { get; set; }
        [ProtoMember(9)]
        public byte RTC_DL { get; set; }
        [ProtoMember(10)]
        public byte RTC_DH { get; set; }
        [ProtoMember(11)]
        public byte RTC_0 { get; set; }
        [ProtoMember(12)]
        public byte RTC_6 { get; set; }
        [ProtoMember(13)]
        public byte RTC_7 { get; set; }
    }
}
