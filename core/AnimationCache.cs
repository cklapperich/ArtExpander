using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using ArtExpander.Core;
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
    private AssetBundleLoader _bundleLoader;
    private readonly MonoBehaviour _coroutineRunner;

    public void LogCacheContents()
    {
        if (_animationFilePaths == null || _animationFilePaths.Count == 0)
        {
            Plugin.Logger.LogInfo("Animation cache is empty.");
            return;
        }

        Plugin.Logger.LogInfo($"Animation Cache Contents ({_animationFilePaths.Count} total animations):");
        Plugin.Logger.LogInfo("----------------------------------------");

        // foreach (var cacheEntry in _animationFilePaths)
        // {
        //     var (monsterType, borderType, expansionType, isFoil) = cacheEntry.Key;
        //     var frames = cacheEntry.Value;

        //     Plugin.Logger.LogInfo($"Monster Type: {monsterType}");
        //     Plugin.Logger.LogInfo($"Border Type: {borderType}");
        //     Plugin.Logger.LogInfo($"Expansion Type: {expansionType}");
        //     Plugin.Logger.LogInfo($"Is Foil: {isFoil}");
        //     Plugin.Logger.LogInfo($"Frame Count: {frames.Count}");
            
        //     // Log the first and last frame paths as examples
        //     if (frames.Any())
        //     {
        //         Plugin.Logger.LogInfo($"First Frame: {frames.First()}");
        //         Plugin.Logger.LogInfo($"Last Frame: {frames.Last()}");
        //     }
            
        //     Plugin.Logger.LogInfo("----------------------------------------");
        // }

        // Log loaded animations state
        Plugin.Logger.LogInfo($"Currently loaded animations in memory: {_loadedAnimations.Count}");
        foreach (var loadedPath in _loadedAnimations.Keys)
        {
            Plugin.Logger.LogInfo($"Loaded animation directory: {loadedPath} ({_loadedAnimations[loadedPath].Length} frames)");
        }
    }

    // Add to constructor
    public AnimationCache(MonoBehaviour coroutineRunner)
    {
        _coroutineRunner = coroutineRunner;
    }

        public void Initialize(string rootPath)
        {
            try
            {
                // Initialize the bundle loader with the .assets suffix
                _bundleLoader = new AssetBundleLoader(rootPath, ".assets");
                
                if (_bundleLoader.IsUsingBundle)
                {
                    ScanAnimationPaths(_bundleLoader.GetAllAssetNames());
                }
                else
                {
                    // Verify root path exists
                    if (!Directory.Exists(rootPath))
                    {
                        Plugin.Logger.LogError($"Root directory does not exist at {rootPath}");
                        throw new DirectoryNotFoundException($"The specified root path does not exist: {rootPath}");
                    }
                    ScanAnimationPaths(Directory.GetFiles(rootPath, "*.png", SearchOption.AllDirectories));
                }
            }
            catch (Exception ex) when (ex is not ArgumentNullException && ex is not DirectoryNotFoundException)
            {
                Plugin.Logger.LogError($"Error during initialization: {ex.Message}");
                _bundleLoader?.Dispose();
                throw;
            }
            LogCacheContents();
        }

        private void ScanAnimationPaths(IEnumerable<string> paths)
        {
            _animationFilePaths.Clear();

            // Filter for PNG files if not already filtered
            var pngPaths = paths.Where(path => path.EndsWith(".png", StringComparison.OrdinalIgnoreCase));
            
            // Group files by their directory to handle multiple animations
            var framesByDirectory = pngPaths.GroupBy(Path.GetDirectoryName);
            //Plugin.Logger.LogWarning(framesByDirectory);
            foreach (var directory in framesByDirectory)
            {
                //Plugin.Logger.LogWarning("================================");
                // Needed for version 3.2 compatibility where people can name things _black or _white
                string monstertype_string = directory.Key.Replace("_white","").Replace("_black","");
                //Plugin.Logger.LogWarning(directory);
                //Plugin.Logger.LogWarning(monstertype_string);
                if (!FileNameToMonsterTypeResolver.TryResolveMonsterType(monstertype_string, out EMonsterType monsterType))
                {
                    Plugin.Logger.LogWarning($"Could not resolve monster type for directory: {directory.Key}");
                    continue;
                }
                //Plugin.Logger.LogWarning(monsterType);

                // Use the first file to determine the card properties
                var firstFile = directory.First();
                var cardResolution = CardAssetResolver.CardInfoFromPath(firstFile);
                
                var cacheKey = (monsterType, cardResolution.BorderType, cardResolution.ExpansionType, cardResolution.IsFoil);

                // Store sorted frame paths
                var sortedFrames = directory.OrderBy(p => 
                {
                    var filename = Path.GetFileNameWithoutExtension(p);
                    return int.TryParse(filename, out int frameNum) ? frameNum : int.MaxValue;
                }).ToList();

                _animationFilePaths[cacheKey] = sortedFrames;

                //Plugin.Logger.LogInfo($"Cached {sortedFrames.Count} frame paths for {directory.Key}");
            }

            Plugin.Logger.LogInfo($"Scanning complete. Found {_animationFilePaths.Count} animation sets");
        }

     public bool RequestAnimationForCard(EMonsterType monsterType, ECardBorderType borderType, 
        ECardExpansionType expansionType, bool isBlackGhost, bool isFoil, Action<Sprite[]> onFramesReady)
    {
        // Get the list of frame paths for this card variant
        List<string> framePaths = CardAssetResolver.ResolvePathFromCardInfo(
            _animationFilePaths, 
            monsterType, 
            borderType, 
            expansionType, 
            isBlackGhost, 
            isFoil);

        if (framePaths == null || !framePaths.Any())
        {
            return false;
        }

        string directoryPath = Path.GetDirectoryName(framePaths[0]);
        
        // Check if animation is already loaded
        if (_loadedAnimations.TryGetValue(directoryPath, out var loadedFrames))
        {
            onFramesReady(loadedFrames);
            return true;
        }

        // Start async loading if not cached
        _coroutineRunner.StartCoroutine(LoadFramesCoroutine(framePaths, directoryPath, onFramesReady));
        return true;
    }

    private System.Collections.IEnumerator LoadFramesCoroutine(List<string> framePaths, string directoryPath, Action<Sprite[]> onFramesReady)
    {
        if (!framePaths.Any()) yield break;
        var loadedFrames = new List<Sprite>();
        foreach (var path in framePaths)
        {
            Sprite sprite = null;
            try 
            {
                sprite = _bundleLoader.LoadSprite(path);
            }
            catch (Exception ex)
            {
                Plugin.Logger.LogError($"Failed to load frame from {path}: {ex.Message}");
                continue;
            }

            if (sprite != null)
            {
                loadedFrames.Add(sprite);
            }
            yield return null;
        }
        
        if (loadedFrames.Count > 0)
        {
            var frames = loadedFrames.ToArray();
            _loadedAnimations[directoryPath] = frames;
            onFramesReady(frames);
        }
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
                if ( sprite != null)
                {
                    UnityEngine.Object.Destroy(sprite);
                }
            }
        }
        _loadedAnimations.Clear();
    }
    }
}