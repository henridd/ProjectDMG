using System;

namespace ProjectDMG.Api
{
    public abstract class ProjectDMGPlugin : IDisposable
    {
        public abstract void Run();

        public abstract void Dispose();
    }
}
