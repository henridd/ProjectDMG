using System.Drawing;

namespace ProjectDMG.DMG
{
    public interface IGUI
    {
        public void SetEmulatorImageSource(Bitmap source);

        public void Invalidate();
    }
}
