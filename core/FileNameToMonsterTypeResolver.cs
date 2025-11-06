using System;
using System.IO;

namespace ArtExpander.Core
{
public static class FileNameToMonsterTypeResolver
{
    public static bool TryResolveMonsterType(string filename, out EMonsterType monsterType)
    {
        monsterType = EMonsterType.None;
        
        // If filename is just a number, return false immediately
        if (int.TryParse(filename, out _))
        {
            return false;
        }

        // Strip any file extension using Path.GetFileNameWithoutExtension
        filename = Path.GetFileNameWithoutExtension(filename);

        // Convert to lowercase before the switch comparison
        string lowercaseFilename = filename.ToLowerInvariant();
        
        filename = filename.Replace("_white","").Replace("_black","");

        // MAX=122 is for the max tetramon/destiny, dev added this in 0.61.4 (?) and so this broke 'max'.Ugh. Max is also megabot 1023
        if (filename == "MAX")
        {
            monsterType = EMonsterType.MAX;
            return true;
        }
        else if (filename == "Max")
        {
            monsterType = EMonsterType.Max;
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