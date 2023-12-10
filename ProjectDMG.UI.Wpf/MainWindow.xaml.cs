using ProjectDMG.Core;
using ProjectDMG.Core.DMG;
using ProjectDMG.GUI.Wpf.Converters;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace ProjectDMG.GUI.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    internal partial class MainWindow : Window, IGui
    {
        private Emulator _dmg;
        private string _defaultRomPath = "G:\\Desenvolvimento\\CSharp\\ProjectDMG\\Roms\\PokemonRed.gb";

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            this.KeyDown += new KeyEventHandler(this.Key_Down);
            this.KeyUp += new KeyEventHandler(this.Key_Up);
        }

        public void Invalidate()
        {
            emulatorScreen.Invalidate();
        }

        public void SetEmulatorImageSource(Bitmap bitmap)
        {
            emulatorScreen.Image = bitmap;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _dmg = new Emulator(this);
            if (Path.Exists(_defaultRomPath))
            {
                _dmg.POWER_ON(_defaultRomPath);
            }
            this.Focus();
        }

        private void Key_Down(object sender, KeyEventArgs e)
        {
            var isControlPressed = (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
            var isShiftPressed = (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;

            if (isControlPressed)
            {
                if (isShiftPressed)
                {
                    HandleCtrlShiftCommand(e);
                    return;
                }

                HandleCtrlCommand(e);
                return;
            }

            if (_dmg.power_switch)
            {
                _dmg.HandleKeyDown(KeyToKeyBitConverter.GetKeyBit(e.Key));
            }
        }

        private void HandleCtrlCommand(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.D1:
                case Key.D2:
                case Key.D3:
                    SaveState(e.Key.ToString());
                    break;
            }
        }

        private void HandleCtrlShiftCommand(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.D1:
                case Key.D2:
                case Key.D3:
                    LoadState(e.Key.ToString());
                    break;
            }
        }

        private void LoadState(string fileName)
        {
            _dmg.LoadSavedState(fileName);
        }

        private void SaveState(string fileName)
        {
            _dmg.GenerateSaveState(fileName);
        }

        private void Key_Up(object sender, KeyEventArgs e)
        {
            if (_dmg.power_switch)
            {
                _dmg.HandleKeyUp(KeyToKeyBitConverter.GetKeyBit(e.Key));
            }
        }

        //private void Drag_Drop(object sender, DragEventArgs e)
        //{
        //    string[] cartNames = (string[])e.Data.GetData(DataFormats.FileDrop);
        //    _dmg.POWER_ON(cartNames[0]);
        //}

        //private void Drag_Enter(object sender, DragEventArgs e)
        //{
        //    if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.All;
        //    _dmg.POWER_OFF();
        //}
    }
}
