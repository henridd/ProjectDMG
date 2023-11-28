namespace ProjectDMG.PokemonRedElasticsearchIntegration
{
    /// <summary>
    /// Obtained from https://datacrystal.romhacking.net/wiki/Pok%C3%A9mon_Red/Blue:RAM_map
    /// </summary>
    internal static class MemoryAddresses
    {
        public const string Money1 = "D347";
        public const string Money2 = "D348";
        public const string Money3 = "D349";

        public const string CurrentMap = "D35E";

        public const string Pokemon1HP = "D16D";

        // Battle
        public const string EnemyName1 = "CFDA";
        public const string EnemyName2 = "CFDB";
        public const string EnemyName3 = "CFDC";
        public const string EnemyName4 = "CFDD";
        public const string EnemyName5 = "CFDE";
        public const string EnemyName6 = "CFDF";
        public const string EnemyName7 = "CFE0";
        public const string EnemyName8 = "CFE1";
        public const string EnemyName9 = "CFE2";
        public const string EnemyName10 = "CFE3";        

        public const string EnemyCatchRate = "D007";
        public const string EnemyHP = "CFE7";
        public const string EnemyWildLevel = "CFF3";
        public const string EnemyPrimaryType = "CFEA";
        public const string EnemySecondaryType = "CFEB";


        public const string EnemyTrainerLevel = "CFE8";
        public const string BattleType = "D057";
        public const string TurnNumber = "CCD5";

        public const string PlayerBattleMove = "CCDC";
    }
}
