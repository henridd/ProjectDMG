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

        public PokemonInformation(string foundAt, string pokemonName, int level, int catchRate, string primaryType, string? secondaryType, int battleType)
        {
            FoundAt = foundAt;
            PokemonName = pokemonName;
            Level = level;
            CatchRate = catchRate;
            PrimaryType = primaryType;
            SecondaryType = secondaryType;

            switch (battleType)
            {
                case 1:
                    IsWildPokemon = true;
                    break;
                case 2:
                    IsTrainerBattle = true;
                    break;
            }
        }
    }
}
