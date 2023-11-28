using System;

namespace ProjectDMG.PokemonRedElasticsearchIntegration.Converters
{
    /// <summary>
    /// Snatched from https://bulbapedia.bulbagarden.net/wiki/Pok%C3%A9mon_data_structure_(Generation_I)#Type
    /// </summary>
    internal static class ByteToPokemonTypeConverter
    {
        internal static string? Convert(byte? b)
        {
            if (b == null)
            {
                return null;
            }

            return b switch
            {
                0 => "Normal",
                1 => "Fighting",
                2 => "Flying",
                3 => "Poison",
                4 => "Ground",
                5 => "Rock",
                7 => "Bug",
                8 => "Ghost",
                20 => "Fire",
                21 => "Water",
                22 => "Grass",
                23 => "Electric",
                24 => "Psychic",
                25 => "Ice",
                26 => "Dragon",
                _ => throw new ArgumentOutOfRangeException(nameof(b)),
            };
        }
    }
}
