using ProjectDMG.Api;

namespace ProjectDMG.PokemonRedElasticsearchIntegration
{
    public class Bootstrapper : ProjectDMGPlugin
    {
        public override void Run()
        {
            MemoryWatcherProvider.GetInstance().AddSubscription("D16D", null);
        }
    }
}
