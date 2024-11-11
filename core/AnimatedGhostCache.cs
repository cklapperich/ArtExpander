using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace ArtExpander.Core
{
public class AnimatedGhostCache 
{
    private readonly Dictionary<(EMonsterType, bool), Sprite[]> _animatedGhostCards = new();
    private readonly Dictionary<(EMonsterType, bool), string[]> _animationFilePaths = new();
    private AssetBundle _ghostAnimationsBundle;
    
    public void Initialize(string rootPath)
    {
        // First try to load asset bundle
        string bundlePath = rootPath + ".assets";
        if (File.Exists(bundlePath))
        {
            try 
            {
                _ghostAnimationsBundle = AssetBundle.LoadFromFile(bundlePath);
                LoadFromAssetBundle();
                Plugin.Logger.LogInfo("Loaded animations from asset bundle");
                return;
            }
            catch (Exception ex)
            {
                Plugin.Logger.LogError($"Failed to load asset bundle: {ex.Message}");
                // Fall through to loose file loading
            }
        }
        else{
            Plugin.Logger.LogWarning($"Could not find asset bundle at {bundlePath}");
        }

        // Fallback to loose PNG files
        ScanAnimationFolders(rootPath);
    }
    private void LoadFromAssetBundle()
    {
        // Get all PNG files from bundle
        var allPngPaths = _ghostAnimationsBundle.GetAllAssetNames()
            .Where(name => name.EndsWith(".png", StringComparison.OrdinalIgnoreCase));

        // Group by monster folders
        var groupedByMonster = new Dictionary<string, List<string>>();
        
        foreach (var path in allPngPaths)
        {
            // Expected path format: "animated/MonsterName_color/frame.png"
            var parts = path.Split('/');
            if (parts.Length != 3) 
            {
                Plugin.Logger.LogWarning($"Unexpected path structure in bundle: {path}");
                continue;
            }

            var monsterFolder = parts[1]; // e.g. "PiggyA_black"
            if (!groupedByMonster.ContainsKey(monsterFolder))
            {
                groupedByMonster[monsterFolder] = new List<string>();
            }
            groupedByMonster[monsterFolder].Add(path);
        }

        // Process each monster's animations
        foreach (var (folder, paths) in groupedByMonster)
        {
            bool isBlackGhost = folder.Contains("_black");
            string monsterName = folder.Replace("_black", "").Replace("_white", "");

            if (!FileNameToMonsterTypeResolver.TryResolveMonsterType(monsterName, out EMonsterType monsterType))
            {
                Plugin.Logger.LogWarning($"Could not resolve monster type for bundle folder: {folder}");
                continue;
            }

            // Sort frames by number
            var orderedPaths = paths.OrderBy(p => 
            {
                var filename = Path.GetFileNameWithoutExtension(p);
                if (int.TryParse(filename, out int frameNum))
                    return frameNum;
                return int.MaxValue;
            }).ToArray();

            if (orderedPaths.Length == 0)
            {
                Plugin.Logger.LogWarning($"No frames found in bundle folder: {folder}");
                continue;
            }

            // Cache the paths for lazy loading
            _animationFilePaths[(monsterType, isBlackGhost)] = orderedPaths;
            //Plugin.Logger.LogInfo($"Cached {orderedPaths.Length} frame paths for {monsterName} (Black: {isBlackGhost})");
        }

        Plugin.Logger.LogInfo($"Bundle scanning complete. Found {_animationFilePaths.Count} animation sets");
    }

    public bool TryGetAnimation(EMonsterType monsterType, bool isBlackGhost, out Sprite[] frames)
    {
        frames = null;
        var key = (monsterType, isBlackGhost);

        // Return cached frames if already loaded
        if (_animatedGhostCards.TryGetValue(key, out frames))
        {
            return true;
        }

        // Load frames if we have paths but haven't loaded them yet
        if (_animationFilePaths.TryGetValue(key, out var paths))
        {
            var loadedFrames = new List<Sprite>();

            foreach (var path in paths)
            {
                try
                {
                    Sprite sprite;
                    if (_ghostAnimationsBundle != null)
                    {
                        // Load from asset bundle
                        sprite = _ghostAnimationsBundle.LoadAsset<Sprite>(path);
                    }
                    else
                    {
                        // Load from loose file
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
                _animatedGhostCards[key] = frames;
                return true;
            }
        }

        return false;
    }

    public void Dispose()
    {
        if (_ghostAnimationsBundle != null)
        {
            _ghostAnimationsBundle.Unload(true);
            _ghostAnimationsBundle = null;
        }
        
        ClearCache();
    }
    public void ClearCache()
    {
        foreach (var frames in _animatedGhostCards.Values)
        {
            foreach (var sprite in frames)
            {
                if (sprite != null)
                {
                    UnityEngine.Object.Destroy(sprite);
                }
            }
        }
        _animatedGhostCards.Clear();
    }

    public void ScanAnimationFolders(string rootPath)
    {
        if (!Directory.Exists(rootPath))
        {
            Plugin.Logger.LogError($"Animation root path does not exist: {rootPath}");
            return;
        }

        var directories = Directory.GetDirectories(rootPath);
        
        foreach (var dir in directories)
        {
            var dirName = Path.GetFileName(dir);
            bool isBlackGhost = dirName.Contains("_black");
            string monsterName = dirName.Replace("_black", "").Replace("_white", "");

            if (!FileNameToMonsterTypeResolver.TryResolveMonsterType(monsterName, out EMonsterType monsterType))
            {
                Plugin.Logger.LogWarning($"Could not resolve monster type for animation folder: {dirName}");
                continue;
            }

            var frameFiles = Directory.GetFiles(dir, "*.png")
                .OrderBy(f => int.Parse(Path.GetFileNameWithoutExtension(f)))
                .ToArray();

            if (frameFiles.Length == 0)
            {
                Plugin.Logger.LogWarning($"No frames found in animation folder: {dirName}");
                continue;
            }

            _animationFilePaths[(monsterType, isBlackGhost)] = frameFiles;
            Plugin.Logger.LogInfo($"Cached {frameFiles.Length} frame paths for {monsterName} (Black: {isBlackGhost})");
        }

        Plugin.Logger.LogInfo($"Animation scanning complete. Found {_animationFilePaths.Count} animation sets");
    }
}
}