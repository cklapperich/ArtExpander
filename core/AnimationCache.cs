using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using ArtExpander.Core;

namespace ArtExpander.Core
{
    public class AnimationCache
    {
        private readonly Dictionary<(EMonsterType monsterType, ECardBorderType BorderType, ECardExpansionType ExpansionType, bool IsFoil), List<string>> _animationFilePaths
            = new Dictionary<(EMonsterType monsterType, ECardBorderType BorderType, ECardExpansionType ExpansionType, bool IsFoil), List<string>>();
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
            string bundlePath = rootPath + ".assets";

            _bundleLoader = new AssetBundleLoader(bundlePath);
            if (_bundleLoader.IsUsingBundle)
            {
                ScanAnimationPaths(_bundleLoader.GetAllAssetNames());
            }
            else
            {
                if (!Directory.Exists(rootPath))
                {
                    Plugin.Logger.LogInfo($"Directory does not exist at {rootPath}. failed to initialize AnimationCache");
                    return;
                }
                ScanAnimationPaths(Directory.GetFiles(rootPath, "*.png", SearchOption.AllDirectories));
            }
        }

        private void ScanAnimationPaths(IEnumerable<string> paths)
        {
            var pngPaths = paths.Where(path => path.EndsWith(".png", StringComparison.OrdinalIgnoreCase));

            // Group files by their directory to handle multiple animations
            // Asset bundles use forward slashes, need to handle both path formats
            var framesByDirectory = pngPaths.GroupBy(path => {
                string dir = Path.GetDirectoryName(path);
                // Asset bundles may return null or empty for GetDirectoryName, extract manually
                if (string.IsNullOrEmpty(dir))
                {
                    int lastSlash = Math.Max(path.LastIndexOf('/'), path.LastIndexOf('\\'));
                    if (lastSlash > 0)
                        dir = path.Substring(0, lastSlash);
                }
                return dir;
            });

            // Process each animation directory (one directory = one animation set)
            // Example directory.Key: "animated/default/Tetramon/PiggyA" or "animated/default/Tetramon/1"
            foreach (var directory in framesByDirectory)
            {
                // Extract just the folder name from the full path (e.g., "animated/Tetramon/1" -> "1")
                // This is the monster identifier, which can be either:
                // - A numeric ID: "1" -> resolves to PiggyA via FileNameToMonsterTypeResolver
                // - A monster name: "PiggyA" -> resolves to PiggyA via FileNameToMonsterTypeResolver
                // IMPORTANT: We must extract ONLY the folder name, not the full path, because
                // TryResolveMonsterType expects a simple identifier, not a path with slashes
                string folderName = Path.GetFileName(directory.Key);
                if (!FileNameToMonsterTypeResolver.TryResolveMonsterType(folderName, out EMonsterType monsterType))
                {
                    Plugin.Logger.LogWarning($"Could not resolve monster type for directory: {directory.Key} (folder: {folderName})");
                    continue;
                }

                // Use the first file's full path to determine card properties (border, expansion, foil)
                // Example: "animated/default/Tetramon/1/1.png"
                // CardInfoFromPath will extract: Expansion=Tetramon, Border=None, Foil=false
                // (Note: CardInfoFromPath skips numeric folders like "1" to avoid misinterpreting them)
                var firstFile = directory.First();
                var cardResolution = CardAssetResolver.CardInfoFromPath(firstFile);

                var cacheKey = (monsterType, cardResolution.BorderType, cardResolution.ExpansionType, cardResolution.IsFoil);

                // Sort frames numerically by filename (1.png, 2.png, 3.png, etc.)
                var sortedFrames = directory.OrderBy(p =>
                {
                    var filename = Path.GetFileNameWithoutExtension(p);
                    return int.TryParse(filename, out int frameNum) ? frameNum : int.MaxValue;
                }).ToList();

                // Always set/overwrite - allows directory files to override bundle entries
                _animationFilePaths[cacheKey] = sortedFrames;
            }

            Plugin.Logger.LogInfo($"Scanning complete. Found {_animationFilePaths.Count} animation sets");
        }

     public bool RequestAnimationForCard(EMonsterType monsterType, ECardBorderType borderType,
        ECardExpansionType expansionType, bool isBlackGhost, bool isFoil, Action<Sprite[]> onFramesReady)
    {
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

        if (_loadedAnimations.TryGetValue(directoryPath, out var loadedFrames))
        {
            onFramesReady(loadedFrames);
            return true;
        }

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