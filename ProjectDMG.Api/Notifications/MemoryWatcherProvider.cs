namespace ProjectDMG.Api.Notifications
{
    public static class MemoryWatcherProvider
    {
        private static IMemoryWatcher _instance = new MemoryWatcher();

        public static IMemoryWatcher GetInstance()
        {
            if(_instance.IsDisposed)
            {
                _instance = new MemoryWatcher();
            }

            return _instance;
        }
    }
}
