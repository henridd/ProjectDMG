using System.Diagnostics;
using System.Linq;

namespace ProjectDMG.PokemonRedElasticsearchIntegration
{
    /// <summary>
    /// Based on https://bulbapedia.bulbagarden.net/wiki/Character_encoding_(Generation_I).
    /// Note that the table is based on hexadecimal, and we're getting decimal from the emulator.
    /// </summary>
    internal static class ByteToCharConverter
    {
        private const int _firstIndex = 127;
        private const int _lastIndex = 185;
        private static string _alphabet = " ABCDEFGHIJKLMNOPQRSTUVWXYZ():;[]abcdefghijklmnopqrstuvwxyz";

        public static string Convert(byte[] input)
        {
            return string.Join(string.Empty, input.Select(Convert));
        }

        public static char Convert(byte input)
        {
            try
            {
                if (input < _firstIndex || input > _lastIndex)
                    return ' ';

                // 128 results to A, 129 to B...
                return _alphabet[input - _firstIndex];
            }
            catch
            {
                Debug.Write($"Error converting {input}");
                return ' ';
            }
        }
    }
}
