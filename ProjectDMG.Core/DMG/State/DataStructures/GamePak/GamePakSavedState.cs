using ProtoBuf;

namespace ProjectDMG.Core.DMG.State.DataStructures.GamePak
{
    [ProtoContract()]
    [ProtoInclude(100, typeof(MBC3SavedState))]
    internal abstract class GamePakSavedState
    {
    }
}
