using System;
using System.IO;
using ProjectDMG.DMG.State.DataStructures;

namespace ProjectDMG.DMG.State
{
    internal class SaveStateManager
    {
        public SaveStateManager()
        {
            if (!Directory.Exists(GetDefaultPath()))
            {
                Directory.CreateDirectory(GetDefaultPath());
            }
        }

        internal void GenerateSaveState(MMU mmu, CPU cpu, PPU ppu,TIMER timer, ProjectDMGSavedState projectDMGSavedState, string fileName)
        {
            var filePath = GetPath(fileName);
            var mmuState = mmu.CreateSaveState();
            var cpuState = cpu.CreateSaveState();
            var ppuState = ppu.CreateSaveState();
            var gamePakState = mmu.CreateGamePakSaveState();
            var timerState = timer.CreateSaveState();
            var state = new SavedState()
            {
                CPUSavedState = cpuState,
                MMUSavedState = mmuState,
                PPUSavedState = ppuState,
                GamePakSavedState = gamePakState,
                TimerSavedState = timerState,
                ProjectDMGSavedState = projectDMGSavedState,
            };
            SaveStateSerializer.Serialize(filePath, state);
        }

        internal SavedState LoadSavedState(string fileName)
        {
            var filePath = GetPath(fileName);
            return SaveStateSerializer.Deserialize<SavedState>(filePath);
        }

        private string GetDefaultPath()
            => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SaveStates");

        private string GetPath(string fileName)
            => Path.Combine(GetDefaultPath(), fileName + ".st");
    }
}
