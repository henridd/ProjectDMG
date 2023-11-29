using static ProjectDMG.Utils.BitOps;

namespace ProjectDMG {
    public class JOYPAD {

        private const int JOYPAD_INTERRUPT = 4;
        private const byte PAD_MASK = 0x10;
        private const byte BUTTON_MASK = 0x20;
        private byte pad = 0xF;
        private byte buttons = 0xF;

        internal void handleKeyDown(KeyEventArgs e) {
            byte b = GetKeyBit(e);
            if ((b & PAD_MASK) == PAD_MASK) {
                pad = (byte)(pad & ~(b & 0xF));
            } else if((b & BUTTON_MASK) == BUTTON_MASK) {
                buttons = (byte)(buttons & ~(b & 0xF));
            }
        }

        internal void handleKeyUp(KeyEventArgs e) {
            byte b = GetKeyBit(e);
            if ((b & PAD_MASK) == PAD_MASK) {
                pad = (byte)(pad | (b & 0xF));
            } else if ((b & BUTTON_MASK) == BUTTON_MASK) {
                buttons = (byte)(buttons | (b & 0xF));
            }
        }

        public void update(MMU mmu) {
            byte JOYP = mmu.JOYP;
            if(!isBit(4, JOYP)) {
                mmu.JOYP = (byte)((JOYP & 0xF0) | pad);
                if(pad != 0xF) mmu.requestInterrupt(JOYPAD_INTERRUPT);
            }
            if (!isBit(5, JOYP)) {
                mmu.JOYP = (byte)((JOYP & 0xF0) | buttons);
                if (buttons != 0xF) mmu.requestInterrupt(JOYPAD_INTERRUPT);
            }
            if ((JOYP & 0b00110000) == 0b00110000) mmu.JOYP = 0xFF;
        }

        private byte GetKeyBit(KeyEventArgs e) {
            switch (e.KeyCode) {
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
