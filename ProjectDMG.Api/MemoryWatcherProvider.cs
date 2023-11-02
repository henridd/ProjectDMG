namespace ProjectDMG.Api
{
    public static class MemoryWatcherProvider
    {
        private static IMemoryWatcher _instance = new MemoryWatcher();

        public static IMemoryWatcher GetInstance()
        {
            return _instance;
        }
    }
}
