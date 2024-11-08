
// File: Utils/FileNameResolver.cs
using System;

namespace ArtExpander.Core
{
    public static class FileNameToMonsterTypeResolver 
    {
        public static bool TryResolveMonsterType(string filename, out EMonsterType monsterType)
        {
            monsterType = EMonsterType.None;
            
            if (filename.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
            {
                filename = filename.Substring(0, filename.Length - 4);
            }

            string[] expansionPrefixes = {
                "Tetramon_", "Destiny_", "Ghost_", "Megabot_", 
                "FantasyRPG_", "CatJob_", "FoodieGO_"
            };
            
            foreach (var prefix in expansionPrefixes)
            {
                filename = filename.Replace(prefix, "");
            }

            switch (filename)
            {
                case "Mummy":
                    monsterType = EMonsterType.MummyMan;
                    return true;
                case "CrystalA":
                    monsterType = EMonsterType.EmeraldA;
                    return true;
                case "CrystalB":
                    monsterType = EMonsterType.EmeraldB;
                    return true;
                case "CrystalC":
                    monsterType = EMonsterType.EmeraldC;
                    return true;
            }

            return Enum.TryParse<EMonsterType>(filename, out monsterType);
        }
    }
}
