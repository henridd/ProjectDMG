using ProjectDMG.PokemonRedElasticsearchIntegration;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace ProjectDMG.PokemonRedElasticsearchIntegration.Pokedex
{
    /// <summary>
    /// HTML and CSS copied from https://dev.to/oryam/css-pokedex-3iln
    /// </summary>
    internal class PokedexHtmlGenerator
    {
        private PokemonInformation? _currentPokemon;
        public string Html { get; private set; }

        public PokedexHtmlGenerator()
        {
            GenerateHtml();
        }

        public void SetPokemon(PokemonInformation? pokemon)
        {
            _currentPokemon = pokemon;
            GenerateHtml();
        }

        public void GenerateHtml()
        {
            var style = ReadFile("Style");
            var body = ReadFile("Body");
            var head = ReadFile("Head");

            body = ReplaceBodyContents(body);
            style = ReplaceStyleContents(style);

            Html = @$"<html><head>{head}</head><style>{style}</style><body>{body}</body></html>";
        }

        private string ReplaceStyleContents(string style)
        {
            var pokemonId = 0;

            if (_currentPokemon.HasValue)
            {
                pokemonId = PokemonNameToIdConverter.Convert(_currentPokemon.Value.PokemonName);
            }

            return style.Replace("{pokemonid}", pokemonId.ToString("D3"));
        }

        private string ReplaceBodyContents(string body)
        {
            string pokemonName, primaryType, secondaryType; 
            pokemonName = primaryType = secondaryType = string.Empty;

            if (_currentPokemon.HasValue)
            {
                pokemonName = _currentPokemon.Value.PokemonName;
                primaryType = _currentPokemon.Value.PrimaryType;
                secondaryType = _currentPokemon.Value.SecondaryType ?? string.Empty;
            }

            return body.Replace("{pokemonname}", pokemonName)
                .Replace("{primarytype}", primaryType)
                .Replace("{secondarytype}", secondaryType);
        }

        private string ReadFile(string fileName)
        {
            var resourceName = "ProjectDMG.PokemonRedElasticsearchIntegration.Pokedex.Html." + fileName + ".html";

            using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            using StreamReader reader = new StreamReader(stream);

            var resourceContent = reader.ReadToEnd();

            return resourceContent;
        }
    }
}
