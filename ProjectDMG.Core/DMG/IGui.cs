using System.Drawing;

namespace ProjectDMG.Core.DMG
{
    public interface IGui
    {
        void Invalidate();

        void SetEmulatorImageSource(Bitmap bitmap);
    }
}
