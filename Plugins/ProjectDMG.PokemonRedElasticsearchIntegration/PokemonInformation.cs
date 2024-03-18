using Elastic.Clients.Elasticsearch;

namespace ProjectDMG.PokemonRedElasticsearchIntegration
{
internal readonly struct PokemonInformation
{
    public int Number { get; }
    public string FoundAt { get; }
    public GeoLocation? GeoLocation { get; }
    public string PokemonName { get; }
    public int Level { get; }
    public int CatchRate { get; }
    public string PrimaryType { get; }
    public string? SecondaryType { get; }
    public bool IsWildPokemon { get; }
    public bool IsTrainerBattle { get; }

    public PokemonInformation(int number, PokemonLocation location, string pokemonName, int level, int catchRate, string primaryType, string? secondaryType, bool isWildPokemon, bool isTrainerBattle)
        {
        FoundAt = location.MapName;
        GeoLocation = location.GeoLocation;
        PokemonName = pokemonName;
        Level = level;
        CatchRate = catchRate;
        PrimaryType = primaryType;
        SecondaryType = secondaryType;
        IsWildPokemon = isWildPokemon;
        IsTrainerBattle = isTrainerBattle;
        Number = number;
    }
}
}
