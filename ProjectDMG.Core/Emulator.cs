using ProjectDMG.Api.Notifications;
using ProjectDMG.Core.DMG;
using ProjectDMG.Core.DMG.State;
using ProjectDMG.Core.DMG.State.DataStructures;
using ProjectDMG.Core.Utils;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectDMG.Core
{
    public class Emulator
    {
        private IGui gui;
        private string currentCartPath;

        private CPU cpu;
        private MMU mmu;
        private PPU ppu;
        private TIMER timer;
        public JOYPAD joypad;
        private SaveStateManager saveStateManager;
        private IMemoryWatcher memoryWatcher;

        public bool power_switch;
        private int cpuCycles;
        private int cyclesThisUpdate;
        private object saveLock = new object();

        public bool IsRunning { get; private set; }

        public Emulator(IGui gui)
        {
            this.gui = gui;
        }

        public void POWER_ON(string cartName)
            => POWER_ON(cartName, null);

        internal void POWER_ON(string cartName, SavedState state)
        {
            currentCartPath = cartName;
            memoryWatcher = MemoryWatcherProvider.GetInstance();
            mmu = new MMU(memoryWatcher, state?.MMUSavedState);
            cpu = new CPU(mmu, state?.CPUSavedState);
            ppu = new PPU(gui, state?.PPUSavedState);
            timer = new TIMER(state?.TimerSavedState);
            joypad = new JOYPAD();
            saveStateManager = new SaveStateManager();

            mmu.loadGamePak(cartName, state?.GamePakSavedState);

            PluginLoader.Load();

            power_switch = true;

            if (state != null)
            {
                cyclesThisUpdate = state.ProjectDMGSavedState.cyclesThisUpdate;
            }

            Task t = Task.Factory.StartNew(EXECUTE, TaskCreationOptions.LongRunning);
        }

        public void POWER_OFF()
        {
            power_switch = false;
        }

        int fpsCounter;

        public void EXECUTE()
        {
            // Main Loop Work in progress
            long start = nanoTime();

            var timerCounter = new Stopwatch();
            timerCounter.Start();
            IsRunning = true;

            while (power_switch)
            {
                lock (saveLock)
                {
                    if (timerCounter.ElapsedMilliseconds > 1000)
                    {
                        //window.Text = "ProjectDMG | FPS: " + fpsCounter;
                        timerCounter.Restart();
                        fpsCounter = 0;
                    }

                    while (cyclesThisUpdate < Constants.CYCLES_PER_UPDATE)
                    {
                        cpuCycles = cpu.Exe();
                        cyclesThisUpdate += cpuCycles;

                        timer.update(cpuCycles, mmu);
                        ppu.update(cpuCycles, mmu);
                        joypad.update(mmu);
                        handleInterrupts();
                    }
                    fpsCounter++;
                    cyclesThisUpdate -= Constants.CYCLES_PER_UPDATE;
                }
            }

            memoryWatcher.Dispose();
            ppu.Dispose();

            IsRunning = false;
        }

        public void HandleKeyDown(byte keyBit)
            => joypad.handleKeyDown(keyBit);

        public void HandleKeyUp(byte keyBit)
            => joypad.handleKeyUp(keyBit);

        private void handleInterrupts()
        {
            byte IE = mmu.IE;
            byte IF = mmu.IF;
            for (int i = 0; i < 5; i++)
            {
                if (((IE & IF) >> i & 0x1) == 1)
                {
                    cpu.ExecuteInterrupt(i);
                }
            }

            cpu.UpdateIME();
        }

        private static long nanoTime()
        {
            long nano = 10000L * Stopwatch.GetTimestamp();
            nano /= TimeSpan.TicksPerMillisecond;
            nano *= 100L;
            return nano;
        }

        public void GenerateSaveState(string fileName)
        {
            lock (saveLock)
            {
                var dmgSaveState = new ProjectDMGSavedState()
                {
                    cyclesThisUpdate = cyclesThisUpdate,
                };

                saveStateManager.GenerateSaveState(mmu, cpu, ppu, timer, dmgSaveState, fileName);
            }
        }

        public void LoadSavedState(string fileName)
        {
            POWER_OFF();
            var state = saveStateManager.LoadSavedState(fileName);

            while (IsRunning)
                Thread.Sleep(100);

            POWER_ON(currentCartPath, state);
        }
    }
}
