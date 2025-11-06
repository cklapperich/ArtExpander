using System;
using System.Collections.Generic;
using ArtExpander.Core;  // for EMonsterType, ECardBorderType, ECardExpansionType
using System.IO;

namespace ArtExpander.Core {
    public class CardAssetResolver
{
   public const ECardBorderType NoneBorder = (ECardBorderType)(-1);
    public const ECardBorderType GhostWhiteBorder = (ECardBorderType)(-2);
    public const ECardBorderType GhostBlackBorder = (ECardBorderType)(-3);
    public class CardFolderResolutionResult {
        public ECardExpansionType ExpansionType = ECardExpansionType.None;
        public ECardBorderType BorderType = NoneBorder;
        public bool IsFoil = false;
    }

    /// <summary>
    /// Extracts card properties (border type, expansion type, foil status) from a file path.
    /// Examines each folder in the path to determine card properties.
    ///
    /// IMPORTANT: Numeric-only folders are skipped to prevent misinterpreting monster IDs
    /// as enum values. For example, in path "animated/Tetramon/1/1.png":
    /// - "Tetramon" correctly sets ExpansionType = Tetramon
    /// - "1" is SKIPPED (it's a monster ID, not a card property)
    /// Without this check, "1" would be parsed as ECardExpansionType.Destiny (enum value 1),
    /// causing a mismatch between cached and requested expansion types.
    /// </summary>
    public static CardFolderResolutionResult CardInfoFromPath(string filepath) {
        var result = new CardFolderResolutionResult();
        // Split path into components and examine each folder
        string[] folders = filepath.Split(new[] { '/', '\\' },
                                        StringSplitOptions.RemoveEmptyEntries);

        foreach(string folder in folders) {
            if(string.IsNullOrEmpty(folder)){
                continue;
            }

            // Skip numeric-only folders (these are monster IDs, not card properties)
            // Without this, "1" in "animated/Tetramon/1/1.png" would be parsed as
            // ECardExpansionType(1) = Destiny, overwriting the correct "Tetramon" expansion
            if(int.TryParse(folder, out _)) {
                continue;
            }

            // Check for foil
            if(folder.Contains("foil", StringComparison.OrdinalIgnoreCase)) {
                result.IsFoil = true;
                continue;
            }

            // Try expansion type
            if(Enum.TryParse<ECardExpansionType>(folder, true, out var expansionType)) {
                result.ExpansionType = expansionType;
                continue;
            }

            // Try border type using existing method
            if(TryParseBorderFolder(folder, out var borderType)) {
                result.BorderType = borderType;
                continue;
            }
        }
        return result;
    }
    private static bool TryParseBorderFolder(string borderName, out ECardBorderType borderType) 
    {   
        // Handle ghost variants first
        if (borderName.Equals("GhostWhite", StringComparison.OrdinalIgnoreCase)) {
            borderType = GhostWhiteBorder;
            return true;
        }
        if (borderName.Equals("GhostBlack", StringComparison.OrdinalIgnoreCase)) {
            borderType = GhostBlackBorder;
            return true;
        }

        // we have to handle these for backwards compatibility with V3.2 of my mod :)
        // Handle _black suffix
        if (borderName.EndsWith("_black", StringComparison.OrdinalIgnoreCase)) {
            borderType = GhostBlackBorder;
            return true;
        }
        
        // Handle _white suffix
        if (borderName.EndsWith("_white", StringComparison.OrdinalIgnoreCase)) {
            borderType = GhostWhiteBorder;
            return true;
        }
        
        // Try normal border type parse if no special cases match
        bool result=Enum.TryParse<ECardBorderType>(borderName, true, out borderType);
        return result;
    }


    // Common resolution method that works with either single paths or lists
    public static T ResolvePathFromCardInfo<T>(
        Dictionary<(EMonsterType, ECardBorderType, ECardExpansionType, bool), T> cache,
        EMonsterType monsterType, 
        ECardBorderType borderType,
        ECardExpansionType expansionType,
        bool isBlackGhost,
        bool isFoil) where T : class
    {
        ECardBorderType modifiedBorderType = borderType;
        // Keep the Ghost expansion handling from original ArtCache
        if (expansionType == ECardExpansionType.Ghost)
        {
            modifiedBorderType = isBlackGhost 
                ? GhostBlackBorder 
                : GhostWhiteBorder;
        }
        
        // Local lookup function matching the pattern both caches use
        T TryLookup(EMonsterType mt, ECardBorderType bt, ECardExpansionType et, bool tryFoilFallback = true)
        {
            var key = (mt, bt, et, isFoil);
            if (cache.TryGetValue(key, out var asset))
                return asset;
                    
            if (tryFoilFallback && isFoil)
            {
                key = (mt, bt, et, false);
                if (cache.TryGetValue(key, out asset))
                    return asset;
            }
                    
            return null;
        }

        // Try lookups from most specific to least specific
        T result;
        
        // 1. Try with all specified parameters
        result = TryLookup(monsterType, modifiedBorderType, expansionType);
        if (result != null) return result;

        // 2. Try with no border but specified expansion
        result = TryLookup(monsterType, NoneBorder, expansionType);
        if (result != null) return result;

        // 3. Try with specified border but no expansion
        result = TryLookup(monsterType, modifiedBorderType, ECardExpansionType.None);
        if (result != null) return result;

        // 4. Try with no border and no expansion
        result = TryLookup(monsterType, NoneBorder, ECardExpansionType.None);
        return result;
    }
}
}