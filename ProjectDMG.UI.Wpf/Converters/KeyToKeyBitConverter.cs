using System.Windows.Input;

namespace ProjectDMG.GUI.Wpf.Converters
{
    internal static class KeyToKeyBitConverter
    {
        internal static byte GetKeyBit(Key key)
        {
            switch (key)
            {
                case Key.D:
                case Key.Right:
                    return 0x11;

                case Key.A:
                case Key.Left:
                    return 0x12;

                case Key.W:
                case Key.Up:
                    return 0x14;

                case Key.S:
                case Key.Down:
                    return 0x18;

                case Key.J:
                case Key.Z:
                    return 0x21;

                case Key.K:
                case Key.X:
                    return 0x22;

                case Key.Space:
                case Key.C:
                    return 0x24;

                case Key.Enter:
                case Key.V:
                    return 0x28;
            }
            return 0;
        }
    }
}
