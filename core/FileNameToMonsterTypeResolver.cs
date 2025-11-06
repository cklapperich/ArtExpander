using System;
using System.IO;

namespace ArtExpander.Core
{
/// <summary>
/// Resolves a filename or folder name to an EMonsterType enum value.
/// Supports both numeric IDs and named monsters (e.g., "1" -> PiggyA, "PiggyA" -> PiggyA).
/// </summary>
public static class FileNameToMonsterTypeResolver
{
    /// <summary>
    /// Attempts to resolve a filename or folder name to a monster type.
    ///
    /// Resolution order:
    /// 1. Numeric parsing (e.g., "1" -> (EMonsterType)1 = PiggyA)
    ///    - This MUST come first to support numeric-only filenames/folders
    ///    - Supports modded monsters from Enhanced Prefab Loader (e.g., "123" -> (EMonsterType)123)
    ///    - C# allows casting any integer to an enum, even if not explicitly defined
    /// 2. Special cases (MAX vs Max)
    /// 3. Lowercase enum name parsing (e.g., "mummy" -> MummyMan)
    /// 4. Case-insensitive enum parsing (e.g., "PiggyA" -> PiggyA)
    /// </summary>
    public static bool TryResolveMonsterType(string filename, out EMonsterType monsterType)
    {
        monsterType = EMonsterType.None;

        // Try to parse as numeric ID first (e.g., "1" -> PiggyA, "123" -> (EMonsterType)123)
        // IMPORTANT: This MUST be first, before any string processing, to support numeric filenames/folders
        // This supports both vanilla monsters (1-122) and modded monsters from Enhanced Prefab Loader (123+)
        if (int.TryParse(filename, out int numericId))
        {
            monsterType = (EMonsterType)numericId;
            return true;
        }

        // Strip any file extension using Path.GetFileNameWithoutExtension
        filename = Path.GetFileNameWithoutExtension(filename);

        // Convert to lowercase before the switch comparison
        string lowercaseFilename = filename.ToLowerInvariant();
        
        filename = filename.Replace("_white","").Replace("_black","");

        // MAX=122 is for the max tetramon/destiny, dev added this in 0.61.4 (?) and so this broke 'Max'.Ugh. "Max" is also megabot 1023
        if (filename=="MAX"){
            monsterType = EMonsterType.MAX;
            return true;
        }
        else if (filename=="Max"){
            monsterType=EMonsterType.Max;
            return true;
        }

        switch (lowercaseFilename)
        {
            case "foilmask":
                monsterType = (EMonsterType)(-999);
                return true;
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

        // Only try to parse if the filename isn't empty after prefix removal
        return !string.IsNullOrWhiteSpace(filename) && 
               Enum.TryParse<EMonsterType>(filename, true, out monsterType);
    }
}
}