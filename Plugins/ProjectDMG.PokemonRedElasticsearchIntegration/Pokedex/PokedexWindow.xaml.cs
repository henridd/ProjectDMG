using System.Threading.Tasks;
using System.Windows;

namespace ProjectDMG.PokemonRedElasticsearchIntegration.Pokedex
{
    /// <summary>
    /// Interaction logic for PokedexWindow.xaml
    /// </summary>
    internal partial class PokedexWindow : Window
    {
        private PokedexHtmlGenerator _pokedexHtmlGenerator = new PokedexHtmlGenerator();
        private bool _isBrowserReady;

        internal PokedexWindow()
        {
            InitializeComponent();
            Loaded += PokedexWindow_Loaded;
        }

        internal void UpdatePokemon(PokemonInformation? pokemonInformation)
        {
            _pokedexHtmlGenerator.SetPokemon(pokemonInformation);

            if (_isBrowserReady)
            {
                Dispatcher.Invoke(() => pokedexBrowser.NavigateToString(_pokedexHtmlGenerator.Html));
            }
        }

        private async void PokedexWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await pokedexBrowser.EnsureCoreWebView2Async();
            _isBrowserReady = true;
            pokedexBrowser.NavigateToString(_pokedexHtmlGenerator.Html);
        }
    }
}
