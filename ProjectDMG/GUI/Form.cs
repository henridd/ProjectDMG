using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace ProjectDMG {
    public partial class Form : System.Windows.Forms.Form {

        ProjectDMG dmg;
        private string defaultRomPath = "G:\\Desenvolvimento\\CSharp\\ProjectDMG\\Roms\\PokemonRed.gb";

        public Form() {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e) {
            dmg = new ProjectDMG(this);
            if(Path.Exists(defaultRomPath))
            {
                dmg.POWER_ON(defaultRomPath);
            }
        }

        private void Key_Down(object sender, KeyEventArgs e) {
            if(e.Control)
            {
                if(e.Shift)
                {
                    HandleCtrlShiftCommand(e);
                    return;
                }

                HandleCtrlCommand(e);
                return;
            }

            if (dmg.power_switch) dmg.joypad.handleKeyDown(e);
        }

        private void HandleCtrlCommand(KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                    SaveState(e.KeyCode.ToString());
                    break;
            }
        }

        private void HandleCtrlShiftCommand(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                    LoadState(e.KeyCode.ToString());
                    break;
            }
        }

        private void LoadState(string fileName)
        {
            dmg.POWER_OFF();
            var state = dmg.LoadSavedState(fileName);

            while (dmg.IsRunning)
                Thread.Sleep(100);

            dmg.POWER_ON(defaultRomPath, state);
        }

        private void SaveState(string fileName)
        {
            dmg.GenerateSaveState(fileName);
        }

        private void Key_Up(object sender, KeyEventArgs e) {
            if (dmg.power_switch) dmg.joypad.handleKeyUp(e);
        }

        private void Drag_Drop(object sender, DragEventArgs e) {
            string[] cartNames = (string[])e.Data.GetData(DataFormats.FileDrop);
            dmg.POWER_ON(cartNames[0]);
        }

        private void Drag_Enter(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.All;
            dmg.POWER_OFF();
        }

    }
}
