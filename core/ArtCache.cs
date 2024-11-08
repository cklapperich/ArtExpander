using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using ArtExpander.Core;  // for FileNameToMonsterTypeResolver

namespace ArtExpander.Core{

    // public class AnimatedGhostCache 
    // {
    //     private readonly Dictionary<(EMonsterType, bool), Sprite[]> _animatedGhostCards = new();

    //     public bool TryGetAnimation(EMonsterType monsterType, bool isBlackGhost, out Sprite[] frames)
    //     {
    //         return _animatedGhostCards.TryGetValue((monsterType, isBlackGhost), out frames);
    //     }

    //     public void LoadAnimatedFolder(string folderPath)
    //     {
    //         if (!Directory.Exists(folderPath)) return;

    //         // Look for folders named like "PiggyA_frames" or "PiggyA_black_frames"
    //         var directories = Directory.GetDirectories(folderPath);
            
    //         foreach (var dir in directories)
    //         {
    //             var dirName = Path.GetFileName(dir).ToLowerInvariant();
    //             if (!dirName.EndsWith("_frames")) continue;

    //             bool isBlackGhost = dirName.Contains("_black_");
    //             string monsterName = dirName.Replace("_black_frames", "").Replace("_frames", "");

    //             if (!FileNameToMonsterTypeResolver.TryResolveMonsterType(monsterName, out EMonsterType monsterType))
    //             {
    //                 Plugin.Logger.LogWarning($"Could not resolve monster type for animation folder: {dirName}");
    //                 continue;
    //             }

    //             // Load all PNGs in numerical order
    //             var frameFiles = Directory.GetFiles(dir, "*.png")
    //                 .OrderBy(f => int.Parse(Path.GetFileNameWithoutExtension(f)))
    //                 .ToArray();

    //             if (frameFiles.Length == 0)
    //             {
    //                 Plugin.Logger.LogWarning($"No frames found in animation folder: {dirName}");
    //                 continue;
    //             }

    //             var frames = new List<Sprite>();
    //             foreach (var framePath in frameFiles)
    //             {
    //                 byte[] fileData = File.ReadAllBytes(framePath);
    //                 Texture2D texture = new Texture2D(2, 2);
    //                 if (texture.LoadImage(fileData))
    //                 {
    //                     var sprite = Sprite.Create(texture, 
    //                         new Rect(0, 0, texture.width, texture.height),
    //                         new Vector2(0.5f, 0.5f));
    //                     frames.Add(sprite);
    //                 }
    //             }

    //             if (frames.Count > 0)
    //             {
    //                 _animatedGhostCards[(monsterType, isBlackGhost)] = frames.ToArray();
    //                 Plugin.Logger.LogInfo($"Loaded {frames.Count} frames for {monsterType} (Black: {isBlackGhost})");
    //             }
    //         }
    //     }

    //     public void ClearCache()
    //     {
    //         foreach (var frames in _animatedGhostCards.Values)
    //         {
    //             foreach (var sprite in frames)
    //             {
    //                 if (sprite != null)
    //                 {
    //                     UnityEngine.Object.Destroy(sprite);
    //                 }
    //             }
    //         }
    //         _animatedGhostCards.Clear();
    //     }
    // }
    
public class ArtCache {
    private readonly Dictionary<(EMonsterType, ECardBorderType, ECardExpansionType), string> _resolvedPathCache = new();
    private string _baseArtPath;
    private readonly Dictionary<string, Sprite> _imageCache = new();
    // special border type constants, internally the game treats GhostWhite/GhostBLack as a EElementIndex
    // Using -1 to match ECardExpansionType.None pattern
    private const ECardBorderType NoneBorder = (ECardBorderType)(-1);
    private const ECardBorderType GhostWhiteBorder = (ECardBorderType)(-2);
    private const ECardBorderType GhostBlackBorder = (ECardBorderType)(-3);

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

            // List all monster types for this expansion/border combination
            foreach (var entry in borderGroup.OrderBy(kvp => kvp.Key.Item1)) // Item1 is EMonsterType
            {
                string relativePath = entry.Value.Replace(_baseArtPath, "").TrimStart('\\', '/');
                Plugin.Logger.LogInfo($"    {entry.Key.Item1}: {relativePath}");
            }
        }
    }

    Plugin.Logger.LogInfo("\n=== End Art Cache Contents ===");
    
    // Log some statistics
    int totalEntries = _resolvedPathCache.Count;
    int uniqueMonsterTypes = _resolvedPathCache.Select(kvp => kvp.Key.Item1).Distinct().Count();
    int uniqueExpansions = _resolvedPathCache.Select(kvp => kvp.Key.Item3).Distinct().Count();
    int uniqueBorders = _resolvedPathCache.Select(kvp => kvp.Key.Item2).Distinct().Count();
    
    Plugin.Logger.LogInfo($"\nCache Statistics:");
    Plugin.Logger.LogInfo($"Total Entries: {totalEntries}");
    Plugin.Logger.LogInfo($"Unique Monster Types: {uniqueMonsterTypes}");
    Plugin.Logger.LogInfo($"Unique Expansions: {uniqueExpansions}");
    Plugin.Logger.LogInfo($"Unique Border Types: {uniqueBorders}");
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
        
        // Try normal border type parse
        return Enum.TryParse<ECardBorderType>(borderName, true, out borderType);
    }
 public void Initialize(string basePath) {
    _baseArtPath = basePath;

    var artFolders = Directory.GetDirectories(_baseArtPath);        
    foreach (string expansionFolder in artFolders) {
        string expansionFolderName = Path.GetFileName(expansionFolder);
        
        // Determine expansion type from folder name
        ECardExpansionType expansionType;
        if (expansionFolderName == "all_expansions") {
            expansionType = ECardExpansionType.None;
        }
        else if (!Enum.TryParse<ECardExpansionType>(expansionFolderName, true, out expansionType)) {
            continue; // Skip folders we don't recognize
        }

        // Process root-level PNG files (these apply to all border types)
        foreach (string file in Directory.GetFiles(expansionFolder, "*.png")) {
            string filename = Path.GetFileName(file);
            // Replace direct enum parse with FileNameResolver
            if (FileNameToMonsterTypeResolver.TryResolveMonsterType(filename, out EMonsterType monsterType)) {
                _resolvedPathCache[(monsterType, NoneBorder, expansionType)] = file;
            }
        }

        // Process border-specific folders within each expansion folder
        foreach (string borderFolder in Directory.GetDirectories(expansionFolder)) {
            string borderName = Path.GetFileName(borderFolder);                
            if (TryParseBorderFolder(borderName, out ECardBorderType borderType)) {
                // Look for PNGs in this border folder
                foreach (string file in Directory.GetFiles(borderFolder, "*.png")) {
                    string filename = Path.GetFileName(file);
                    // Replace direct enum parse with FileNameResolver
                    if (FileNameToMonsterTypeResolver.TryResolveMonsterType(filename, out EMonsterType monsterType)) {
                        //Plugin.Logger.LogWarning($"{monsterType} {borderType} {expansionType}");
                        _resolvedPathCache[(monsterType, borderType, expansionType)] = file;
                    }
                }
            } else {
                Plugin.Logger.LogWarning($"Skipping unrecognized border folder: {borderName}");
            }
        }
    }

    // Debug logging
    //LogCacheContents();
}
         public string ResolveArtPath(EMonsterType monsterType, ECardBorderType borderType, ECardExpansionType expansionType, bool isDestiny) {
            // Try most specific combination first
            if (_resolvedPathCache.TryGetValue((monsterType, borderType, expansionType), out string path))
                return path;

            // For Ghost expansion, use isDestiny to determine which ghost variant to use
            if (expansionType == ECardExpansionType.Ghost) {
                // Determine which ghost border type to use based on isDestiny
                ECardBorderType ghostBorder = isDestiny 
                    ? GhostBlackBorder 
                    : GhostWhiteBorder;

                //Plugin.Logger.LogInfo($"Looking up ghost art for {monsterType} isDestiny={isDestiny} ({ghostBorder})");
                
                // Try to get path with the specific ghost border
                if (_resolvedPathCache.TryGetValue((monsterType, ghostBorder, expansionType), out path))
                    return path;
                
                // If no specific ghost variant found, try generic ghost art
                if (_resolvedPathCache.TryGetValue((monsterType, NoneBorder, expansionType), out path))
                    return path;
            }

            // Continue with regular fallback checks
            if (_resolvedPathCache.TryGetValue((monsterType, NoneBorder, expansionType), out path))
                return path;
            
            if (_resolvedPathCache.TryGetValue((monsterType, borderType, ECardExpansionType.None), out path))
                return path;
            
            if (_resolvedPathCache.TryGetValue((monsterType, NoneBorder, ECardExpansionType.None), out path))
                return path;

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