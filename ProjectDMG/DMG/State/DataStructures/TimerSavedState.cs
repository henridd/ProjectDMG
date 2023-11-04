using ProtoBuf;

namespace ProjectDMG.DMG.State.DataStructures
{
    [ProtoContract]
    internal class TimerSavedState
    {
        [ProtoMember(1)]
        public int divCounter{get;set;}
        [ProtoMember(2)]
        public int timerCounter { get; set; }
    }
}
