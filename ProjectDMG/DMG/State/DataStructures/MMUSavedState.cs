using ProtoBuf;

namespace ProjectDMG.DMG.State.DataStructures
{
    [ProtoContract]
    internal class MMUSavedState
    {
        [ProtoMember(1)]
        public byte[] VRAM { get; set; }
        [ProtoMember(2)]
        public byte[] WRAM0 { get; set; }
        [ProtoMember(3)]
        public byte[] WRAM1 { get; set; }
        [ProtoMember(4)]
        public byte[] OAM { get; set; }
        [ProtoMember(5)]
        public byte[] IO { get; set; }
        [ProtoMember(6)]
        public byte[] HRAM { get; set; }

        // Used by the serializer
        public MMUSavedState()
        {

        }

        public MMUSavedState(byte[] vram, byte[] wram0, byte[] wram1, byte[] oam, byte[] io, byte[] hram)
        {
            VRAM = vram;
            WRAM0 = wram0;
            WRAM1 = wram1;
            OAM = oam;
            IO = io;
            HRAM = hram;
        }
    }
}
