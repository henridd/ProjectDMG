using System;

namespace ProjectDMG.PokemonRedElasticsearchIntegration
{
    internal readonly struct PokemonInformation
    {
        public string FoundAt { get; }
        public string PokemonName { get; }
        public int Level { get; }
        public int CatchRate { get; }
        public string PrimaryType { get; }
        public string? SecondaryType { get; }
        public bool IsWildPokemon { get; }
        public bool IsTrainerBattle { get; }

        public PokemonInformation(string foundAt, string pokemonName, int level, int catchRate, string primaryType, string? secondaryType, bool isWildPokemon, bool isTrainerBattle)
        {
            FoundAt = foundAt;
            PokemonName = pokemonName;
            Level = level;
            CatchRate = catchRate;
            PrimaryType = primaryType;
            SecondaryType = secondaryType;
            IsWildPokemon = isWildPokemon;
            IsTrainerBattle = isTrainerBattle;
        }
    }
}
