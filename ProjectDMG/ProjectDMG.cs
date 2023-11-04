using ProjectDMG.Api;
using ProjectDMG.DMG.State;
using ProjectDMG.DMG.State.DataStructures;
using ProjectDMG.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectDMG
{
    public class ProjectDMG
    {

        Form window;

        public ProjectDMG(Form window)
        {
            this.window = window;
        }

        private CPU cpu;
        private MMU mmu;
        private PPU ppu;
        private TIMER timer;
        public JOYPAD joypad;
        private SaveStateManager saveStateManager;

        public bool power_switch;
        private int cpuCycles;
        private int cyclesThisUpdate;
        private object saveLock = new object();

        public bool IsRunning { get; private set; }

        internal void POWER_ON(string cartName)
            => POWER_ON(cartName, null);

        internal void POWER_ON(string cartName, SavedState state)
        {
            mmu = new MMU(MemoryWatcherProvider.GetInstance(), state?.MMUSavedState);
            cpu = new CPU(mmu, state?.CPUSavedState);
            ppu = new PPU(window, state?.PPUSavedState);
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

            ppu.Dispose();

            IsRunning = false;
        }

        private void handleInterrupts()
        {
            byte IE = mmu.IE;
            byte IF = mmu.IF;
            for (int i = 0; i < 5; i++)
            {
                if ((((IE & IF) >> i) & 0x1) == 1)
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

        internal void GenerateSaveState(string fileName)
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

        internal SavedState LoadSavedState(string fileName)
        {
            return saveStateManager.LoadSavedState(fileName);
        }
    }
}
