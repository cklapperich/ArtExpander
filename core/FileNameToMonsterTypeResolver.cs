using System;
using System.IO;

namespace ArtExpander.Core
{
    public static class FileNameToMonsterTypeResolver 
    {
        public static bool TryResolveMonsterType(string filename, out EMonsterType monsterType)
        {
            monsterType = EMonsterType.None;
            
            // Strip any file extension using Path.GetFileNameWithoutExtension
            filename = Path.GetFileNameWithoutExtension(filename);

            string[] expansionPrefixes = {
                "Tetramon_", "Destiny_", "Ghost_", "Megabot_", 
                "FantasyRPG_", "CatJob_", "FoodieGO_"
            };
            
            foreach (var prefix in expansionPrefixes)
            {
                filename = filename.Replace(prefix, "", StringComparison.OrdinalIgnoreCase);
            }

            // Convert to lowercase before the switch comparison
            string lowercaseFilename = filename.ToLowerInvariant();
            
            switch (lowercaseFilename)
            {
                case "mummy":
                    monsterType = EMonsterType.MummyMan;
                    return true;
                case "crystala":
                    monsterType = EMonsterType.EmeraldA;
                    return true;
                case "crystalb":
                    monsterType = EMonsterType.EmeraldB;
                    return true;
                case "crystalc":
                    monsterType = EMonsterType.EmeraldC;
                    return true;
            }
            
            return Enum.TryParse<EMonsterType>(filename, true, out monsterType);
        }
    }
}