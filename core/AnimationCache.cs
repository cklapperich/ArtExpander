using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
//TODO: ADD CODE FOR UNITY ASYNC ASSET LOADING
//TODO: TEXTURE COMPRESSION? IF TEXTURES ARE MULTIPLES OF 4 ON THE DIMENSION BUT
// COULD INCREASE CPU USAGE!!
namespace ArtExpander.Core
{
    public class AnimationCache 
    {
        // Cache for animation frame paths, keyed by card properties
        private readonly Dictionary<(EMonsterType monsterType, ECardBorderType BorderType, ECardExpansionType ExpansionType, bool IsFoil), List<string>> _animationFilePaths 
            = new Dictionary<(EMonsterType monsterType, ECardBorderType BorderType, ECardExpansionType ExpansionType, bool IsFoil), List<string>>();

        // Cache for loaded sprite arrays, keyed by the folder path containing the animation frames
        private readonly Dictionary<string, Sprite[]> _loadedAnimations = new Dictionary<string, Sprite[]>();
        
            private AssetBundle _animationsBundle;
    public void Initialize(string rootPath)
    {
        // Validate input parameter
        if (string.IsNullOrWhiteSpace(rootPath))
        {
            throw new ArgumentNullException(nameof(rootPath), "Root path cannot be null or empty.");
        }

        // Verify root path exists
        if (!Directory.Exists(rootPath))
        {
            Plugin.Logger.LogError($"Root directory does not exist at {rootPath}");
            throw new DirectoryNotFoundException($"The specified root path does not exist: {rootPath}");
        }

        bool loadFromBundle = false;
        string bundlePath = rootPath + ".assets";
        
        try
        {
            if (File.Exists(bundlePath))
            {
                loadFromBundle = true;
                _animationsBundle = AssetBundle.LoadFromFile(bundlePath);
                
                if (_animationsBundle == null)
                {
                    Plugin.Logger.LogError($"Failed to load asset bundle at {bundlePath}");
                    throw new InvalidOperationException($"Asset bundle could not be loaded from {bundlePath}");
                }
            }
            else
            {
                Plugin.Logger.LogWarning($"Could not find asset bundle at {bundlePath}");
            }

            ScanAnimationFolder(rootPath, loadFromBundle);
        }
        catch (Exception ex) when (ex is not ArgumentNullException && ex is not DirectoryNotFoundException)
        {
            Plugin.Logger.LogError($"Error during initialization: {ex.Message}");
            _animationsBundle?.Unload(true);
            throw;
        }
    }

        private void ScanAnimationFolder(string targetFolder, bool isAssetBundle = true)
        {
            IEnumerable<string> allPngPaths;
            
            if (isAssetBundle)
            {
                allPngPaths = _animationsBundle.GetAllAssetNames()
                    .Where(name => name.EndsWith(".png", StringComparison.OrdinalIgnoreCase));
            }
            else 
            {
                allPngPaths = Directory.GetFiles(targetFolder, "*.png", SearchOption.AllDirectories);
            }

            _animationFilePaths.Clear();

            // Group files by their directory to handle multiple animations
            var framesByDirectory = allPngPaths.GroupBy(Path.GetDirectoryName);

            foreach (var directory in framesByDirectory)
            {
                //needed for version 3.2 compatibilty where people can name things _black or _white
                string monstertype_string = directory.Key.Replace("_white","").Replace("_black","");
                if (!FileNameToMonsterTypeResolver.TryResolveMonsterType(monstertype_string, out EMonsterType monsterType))
                {
                    Plugin.Logger.LogWarning($"Could not resolve monster type for directory: {directory.Key}");
                    continue;
                }

                // Use the first file to determine the card properties
                var firstFile = directory.First();
                var cardResolution = ArtCache.ResolveCardFromPath(firstFile);
                
                var cacheKey = (monsterType, cardResolution.BorderType, cardResolution.ExpansionType, cardResolution.IsFoil);

                // Store sorted frame paths
                var sortedFrames = directory.OrderBy(p => 
                {
                    var filename = Path.GetFileNameWithoutExtension(p);
                    return int.TryParse(filename, out int frameNum) ? frameNum : int.MaxValue;
                }).ToList();

                _animationFilePaths[cacheKey] = sortedFrames;

                Plugin.Logger.LogInfo($"Cached {sortedFrames.Count} frame paths for {directory.Key}");
            }

            Plugin.Logger.LogInfo($"Scanning complete. Found {_animationFilePaths.Count} animation sets");
        }

    public List<string> ResolveArtPathToList(EMonsterType monsterType, ECardBorderType borderType, 
        ECardExpansionType expansionType, bool isFoil = false)
    {
        // Local function to try lookup with foil fallback
        List<string> TryLookup(EMonsterType mt, ECardBorderType bt, ECardExpansionType et, bool tryFoilFallback = true)
        {   
            // Create lookup key with the specified foil status
            var key = (mt, bt, et, isFoil);
            if (_animationFilePaths.TryGetValue(key, out var path))
            {
                return path;
            }
                    
            // Try non-foil fallback if requested
            if (tryFoilFallback && isFoil)
            {
                key = (mt, bt, et, false);
                if (_animationFilePaths.TryGetValue(key, out path))
                {
                    return path;
                }
            }
                    
            return null;
        }

        // Try lookups from most specific to least specific
        
        // 1. Try with all specified parameters
        List<string> result = TryLookup(monsterType, borderType, expansionType);
        if (result != null)
        {
            return result;
        }

        // 2. Try with no border but specified expansion (Tetramon vs Destiny split)
        result = TryLookup(monsterType, ArtCache.NoneBorder, expansionType);
        if (result != null)
        {
            return result;
        }

        // 3. Try with specified border but no expansion
        result = TryLookup(monsterType, borderType, ECardExpansionType.None);
        if (result != null)
        {
            return result;
        }

        // 4. Try with no border and no expansion (most generic case)
        result = TryLookup(monsterType, ArtCache.NoneBorder, ECardExpansionType.None);
        if (result != null)
        {
            return result;
        }
                
        return null;
    }

    public bool TryGetAnimation(EMonsterType monsterType, ECardBorderType borderType, 
    ECardExpansionType expansionType, bool isFoil, out Sprite[] frames)
    {
        frames = null;
        
        // Get the list of frame paths for this card variant
        List<string> framePaths = ResolveArtPathToList(monsterType, borderType, expansionType, isFoil);

        if (framePaths == null || !framePaths.Any())
        {
            return false;
        }

        // Use the directory path as the cache key for loaded sprites
        string directoryPath = Path.GetDirectoryName(framePaths[0]);
        
        // Check if we already have these sprites loaded
        if (_loadedAnimations.TryGetValue(directoryPath, out frames))
        {
            return true;
        }

        // Load all sprites for this animation
        var loadedFrames = new List<Sprite>();
        foreach (var path in framePaths)
        {
            try
            {
                Sprite sprite;
                if (_animationsBundle != null)
                {
                    sprite = _animationsBundle.LoadAsset<Sprite>(path);
                }
                else
                {
                    byte[] fileData = File.ReadAllBytes(path);
                    var texture = new Texture2D(2, 2, TextureFormat.RGBA32, mipChain: true);
                    texture.LoadImage(fileData);
                    sprite = Sprite.Create(texture, 
                        new Rect(0, 0, texture.width, texture.height),
                        new Vector2(0.5f, 0.5f));
                }

                if (sprite != null)
                {
                    loadedFrames.Add(sprite);
                }
            }
            catch (Exception ex)
            {
                Plugin.Logger.LogError($"Failed to load frame from {path}: {ex.Message}");
            }
        }

        if (loadedFrames.Count > 0)
        {
            frames = loadedFrames.ToArray();
            _loadedAnimations[directoryPath] = frames;
            return true;
        }

        return false;
    }

    public void Dispose()
    {
        if (_animationsBundle != null)
        {
            _animationsBundle.Unload(true);
            _animationsBundle = null;
        }
        
        ClearCache();
    }

    public void ClearCache()
    {
        foreach (var frames in _loadedAnimations.Values)
        {
            foreach (var sprite in frames)
            {
                if (sprite != null)
                {
                    UnityEngine.Object.Destroy(sprite);
                }
            }
        }
        _loadedAnimations.Clear();
    }
    }
}