using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using ArtExpander.Core;  // for FileNameToMonsterTypeResolver

namespace ArtExpander.Core{
public class ArtCache {
    // last bool is for isfoil yes/no
    private Dictionary<string, Sprite> _imageCache = new Dictionary<string, Sprite>();
    private readonly Dictionary<(EMonsterType, ECardBorderType, ECardExpansionType, bool), string> _resolvedPathCache = new();
    private string _baseArtPath;
    // special border type constants, internally the game treats GhostWhite/GhostBLack as a EElementIndex
    // Using -1 to match ECardExpansionType.None pattern
    public const ECardBorderType NoneBorder = (ECardBorderType)(-1);
    public const ECardBorderType GhostWhiteBorder = (ECardBorderType)(-2);
    public const ECardBorderType GhostBlackBorder = (ECardBorderType)(-3);

    public class CardFolderResolutionResult {
        public ECardExpansionType ExpansionType = ECardExpansionType.None;
        public ECardBorderType BorderType = (ECardBorderType)(-1); // NoneBorder
        public bool IsFoil = false;
    }

    public static CardFolderResolutionResult ResolveCardFromPath(string filepath) {
        var result = new CardFolderResolutionResult();
        // Split path into components and examine each folder
        string[] folders = filepath.Split(Path.DirectorySeparatorChar);
        //Plugin.Logger.LogWarning($"ResolveCardFromPath is checking {filepath}");
        foreach(string folder in folders) {
            if(string.IsNullOrEmpty(folder)){
                continue;
            } 

            // Check for foil
            //Plugin.Logger.LogWarning($"{folder}");
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
    public void LogCacheContents()
    {
        Plugin.Logger.LogInfo("=== Art Cache Contents ===");

        // Group by expansion type first
        var groupedByExpansion = _resolvedPathCache
            .GroupBy(kvp => kvp.Key.Item3) // Item3 is ECardExpansionType
            .OrderBy(g => g.Key);

        foreach (var expansionGroup in groupedByExpansion)
        {
            Plugin.Logger.LogInfo($"\nExpansion: {expansionGroup.Key}");

            // Group by border type within each expansion
            var groupedByBorder = expansionGroup
                .GroupBy(kvp => kvp.Key.Item2) // Item2 is ECardBorderType
                .OrderBy(g => g.Key);

            foreach (var borderGroup in groupedByBorder)
            {
                string borderName = borderGroup.Key switch
                {
                    (ECardBorderType)(-1) => "None",
                    (ECardBorderType)(-2) => "GhostWhite",
                    (ECardBorderType)(-3) => "GhostBlack",
                    _ => borderGroup.Key.ToString()
                };
                
                Plugin.Logger.LogInfo($"\n  Border: {borderName}");

                foreach (var entry in borderGroup.OrderBy(kvp => kvp.Key.Item1)) // Item1 is EMonsterType
                {
                    string relativePath = entry.Value.Replace(_baseArtPath, "").TrimStart('\\', '/');
                    string foilStatus = entry.Key.Item4 ? "Foil" : "Normal"; // Add foil status
                    Plugin.Logger.LogInfo($"    {entry.Key.Item1} ({foilStatus}): {relativePath}");
                }
            }
        }

        Plugin.Logger.LogInfo("\n=== End Art Cache Contents ===");
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
            borderType = GhostWhiteBorder;
            return true;
        }
        
        // Handle _white suffix
        if (borderName.EndsWith("_white", StringComparison.OrdinalIgnoreCase)) {
            borderType = GhostWhiteBorder;
            return true;
        }
        
        // Try normal border type parse if no special cases match
        return Enum.TryParse<ECardBorderType>(borderName, true, out borderType);
    }
    public void Initialize(string basePath)
    {
        _baseArtPath = basePath;

        var artFolders = Directory.GetDirectories(_baseArtPath);
        foreach (var folder in artFolders)
        {
            // Get all subfolders including the root folder
            var allFolders = new List<string> { folder };
            allFolders.AddRange(Directory.GetDirectories(folder, "*", SearchOption.AllDirectories));
            
            foreach (var currentFolder in allFolders)
            {
                // Use SearchPattern array to find both .png and .PNG files
                var pngFiles = Directory.GetFiles(currentFolder, "*.png")
                    .Concat(Directory.GetFiles(currentFolder, "*.PNG"))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToArray();

                if (pngFiles.Length > 0)
                {
                    // Convert to relative path for ResolveCardFromPath
                    var relativeFolderPath = Path.GetRelativePath(_baseArtPath, currentFolder);
                    
                    // Get folder resolution result using relative path
                    var resolutionResult = ResolveCardFromPath(relativeFolderPath);

                    foreach (var pngFile in pngFiles)
                    {
                        // Try to resolve monster type from filename
                        if (FileNameToMonsterTypeResolver.TryResolveMonsterType(
                            Path.GetFileNameWithoutExtension(pngFile), 
                            out EMonsterType monsterType))
                        {
                            var cacheKey = (
                                monsterType,
                                resolutionResult.BorderType,
                                resolutionResult.ExpansionType,
                                resolutionResult.IsFoil
                            );
                            
                            if (!_resolvedPathCache.ContainsKey(cacheKey))
                            {
                                _resolvedPathCache[cacheKey] = pngFile;
                            }
                        }
                        else
                        {
                            Plugin.Logger.LogWarning($"Failed to parse Monster Name {pngFile}");
                        }
                    }
                }
            }
        }
    }
    public string ResolveArtPath(EMonsterType monsterType, ECardBorderType borderType, ECardExpansionType expansionType, bool isDestiny, bool isFoil = false)
    {
        ECardBorderType modifiedBorderType = borderType;
        // For Ghost expansion, use isDestiny to determine which ghost variant to use
        if (expansionType == ECardExpansionType.Ghost)
        {
            modifiedBorderType = isDestiny 
                ? GhostBlackBorder 
                : GhostWhiteBorder;
        }

        // special method to try specified foil type first then non-foil
        string TryLookup(EMonsterType mt, ECardBorderType bt, ECardExpansionType et, bool tryFoilFallback = true)
        {
            // Try with specified foil status
            if (_resolvedPathCache.TryGetValue((mt, bt, et, isFoil), out string path))
                return path;
                
            // Optionally try non-foil fallback
            if (tryFoilFallback && isFoil && _resolvedPathCache.TryGetValue((mt, bt, et, false), out path))
                return path;
                
            return null;
        }

        // Try most specific combination first, then least specific last
        string result = TryLookup(monsterType, modifiedBorderType, expansionType);
        if (result != null)
            return result;

        //no border specified, expansion specified. This is the Tetramon vs Destiny 'split'
        // use case, to merely double the # of cards in the game by splitting Tetramon?Destiny
        result = TryLookup(monsterType, NoneBorder, expansionType);
        if (result != null)
            return result;

        // no expansion specified, border specified
        result = TryLookup(monsterType, modifiedBorderType, ECardExpansionType.None);
        if (result != null)
            return result;

        // no border specified, no expansion specified
        result = TryLookup(monsterType, NoneBorder, ECardExpansionType.None);
        if (result != null)
            return result;
            
        return null;

    }
    public Sprite LoadSprite(string path) {
        if (string.IsNullOrEmpty(path))
            return null;

        // Check image cache first
        if (_imageCache.TryGetValue(path, out Sprite cachedSprite))
            return cachedSprite;

        try {
            // Load the texture from file
            byte[] fileData = File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(2, 2);
            if (texture.LoadImage(fileData)) {
                // Create sprite from the loaded texture
                Sprite sprite = Sprite.Create(texture, 
                    new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f));
                
                // Cache the sprite
                _imageCache[path] = sprite;
                return sprite;
            }
        }
        catch (Exception ex) {
            Plugin.Logger.LogError($"Failed to load image from {path}: {ex.Message}");
        }

        return null;
    }
    public void ClearImageCache() {
        foreach (var sprite in _imageCache.Values) {
            if (sprite != null) {
                UnityEngine.Object.Destroy(sprite);
            }
        }
        _imageCache.Clear();
    }
}

}