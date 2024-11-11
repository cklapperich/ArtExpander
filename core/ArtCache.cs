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

    public string ResolveArtPath(
        EMonsterType monsterType, 
        ECardBorderType borderType, 
        ECardExpansionType expansionType, 
        bool isBlackGhost,
        bool isFoil = false)
        {
            return CardAssetResolver.ResolvePathFromCardInfo(
                _resolvedPathCache, 
                monsterType, 
                borderType,
                expansionType, 
                isBlackGhost,
                isFoil);
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
                    var resolutionResult = CardAssetResolver.CardInfoFromPath(relativeFolderPath);

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