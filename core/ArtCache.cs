using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using ArtExpander.Core;

namespace ArtExpander.Core {
    public class ArtCache : IDisposable {
        public const EMonsterType FOILMASK = (EMonsterType)(-999);
        private Dictionary<string, Sprite> _imageCache = new Dictionary<string, Sprite>();
        private readonly Dictionary<(EMonsterType, ECardBorderType, ECardExpansionType, bool), string> _resolvedPathCache = new();
        private string _baseArtPath;
        private AssetBundleLoader _bundleLoader;

        public string ResolveArtPath(
            EMonsterType monsterType,
            ECardBorderType borderType,
            ECardExpansionType expansionType,
            bool isBlackGhost,
            bool isFoil = false)
        {
            // First try to find the specific monster
            string result = CardAssetResolver.ResolvePathFromCardInfo(
                _resolvedPathCache,
                monsterType,
                borderType,
                expansionType,
                isBlackGhost,
                isFoil);

            // If not found and this is a foil request, try foilmask fallback
            if (result == null && isFoil)
            {
                result = CardAssetResolver.ResolvePathFromCardInfo(
                    _resolvedPathCache,
                    FOILMASK,
                    borderType,
                    expansionType,
                    isBlackGhost,
                    isFoil);
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
                        string foilStatus = entry.Key.Item4 ? "Foil" : "Normal";
                        Plugin.Logger.LogInfo($"    {entry.Key.Item1} ({foilStatus}): {relativePath}");
                    }
                }
            }

            Plugin.Logger.LogInfo("\n=== End Art Cache Contents ===");
        }

        public void Initialize(string basePath)
        {
            _baseArtPath = basePath;

            try
            {
                // Initialize the bundle loader with the .assets suffix
                _bundleLoader = new AssetBundleLoader(basePath, ".assets");
                
                if (_bundleLoader.IsUsingBundle)
                {
                    Plugin.Logger.LogWarning($"Loaded cardart.assets at {basePath}.assets");
                    ScanArtPaths(_bundleLoader.GetAllAssetNames());
                }
                else
                {
                    // Verify base path exists
                    if (!Directory.Exists(basePath))
                    {
                        Plugin.Logger.LogError($"cardart directory does not exist at {basePath}. failed to initialize ArtCache.");
                        return;
                    }

                    var allFiles = new List<string>();
                    var artFolders = Directory.GetDirectories(_baseArtPath);
                    
                    foreach (var folder in artFolders)
                    {
                        // Get all subfolders including the root folder
                        var allFolders = new List<string> { folder };
                        allFolders.AddRange(Directory.GetDirectories(folder, "*", SearchOption.AllDirectories));
                        
                        foreach (var currentFolder in allFolders)
                        {
                            // Skip if the folder path contains 'animated'
                            if (currentFolder.Contains("animated", StringComparison.OrdinalIgnoreCase))
                            {
                                Plugin.Logger.LogWarning($"Skipping animated folder: {currentFolder}");
                                continue;
                            }

                            // Get all PNG files
                            allFiles.AddRange(Directory.GetFiles(currentFolder, "*.png")
                                .Concat(Directory.GetFiles(currentFolder, "*.PNG"))
                                .Distinct(StringComparer.OrdinalIgnoreCase));
                        }
                    }
                    
                    ScanArtPaths(allFiles);
                }
            }
            catch (Exception ex) when (ex is not ArgumentNullException && ex is not DirectoryNotFoundException)
            {
                Plugin.Logger.LogError($"Error during ArtCache initialization: {ex.Message}");
                _bundleLoader?.Dispose();
            }
            //LogCacheContents();
        }

        private void ScanArtPaths(IEnumerable<string> paths)
        {
            _resolvedPathCache.Clear();

            // Filter for PNG files if not already filtered
            var pngPaths = paths.Where(path => 
                path.EndsWith(".png", StringComparison.OrdinalIgnoreCase) && 
                !path.Contains("animated", StringComparison.OrdinalIgnoreCase));

            foreach (var pngFile in pngPaths)
            {
                try
                {
                    // Get card resolution result
                    var resolutionResult = CardAssetResolver.CardInfoFromPath(pngFile);

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
                catch (Exception ex)
                {
                    Plugin.Logger.LogWarning($"Failed to process file {pngFile}: {ex.Message}");
                }
            }

            Plugin.Logger.LogInfo($"Scanning complete. Found {_resolvedPathCache.Count} art assets");
        }

        public Sprite LoadSprite(string path)
        {
            if (string.IsNullOrEmpty(path))
                return null;

            // Check image cache first
            if (_imageCache.TryGetValue(path, out Sprite cachedSprite))
                return cachedSprite;

            try
            {
                Sprite sprite;
                sprite = _bundleLoader.LoadSprite(path);

                if (sprite != null)
                {
                    // Cache the sprite
                    _imageCache[path] = sprite;
                }
                return sprite;
            }
            catch (Exception ex)
            {
                Plugin.Logger.LogError($"Failed to load image from {path}: {ex.Message}");
                return null;
            }
        }

        public void ClearImageCache()
        {
            foreach (var sprite in _imageCache.Values)
            {
                if (sprite != null)
                {
                    UnityEngine.Object.Destroy(sprite);
                }
            }
            _imageCache.Clear();
        }

        public void Dispose()
        {
            ClearImageCache();
            _bundleLoader?.Dispose();
        }
    }
}