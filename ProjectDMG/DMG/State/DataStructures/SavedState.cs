using ProjectDMG.DMG.State.DataStructures.GamePak;
using ProtoBuf;

namespace ProjectDMG.DMG.State.DataStructures
{
    [ProtoContract]
    internal class SavedState
    {
        [ProtoMember(1)]
        internal MMUSavedState MMUSavedState { get; set; }
        [ProtoMember(2)]
        internal CPUSavedState CPUSavedState { get; set; }
        [ProtoMember(3)]
        internal PPUSavedState PPUSavedState { get; set; }
        [ProtoMember(4)]
        internal GamePakSavedState GamePakSavedState { get; set; }
        [ProtoMember(5)]
        public TimerSavedState TimerSavedState { get; set; }
        [ProtoMember(6)]
        public ProjectDMGSavedState ProjectDMGSavedState { get; set; }

        public SavedState()
        {
            
        }
    }
}
