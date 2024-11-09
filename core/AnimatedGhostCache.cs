using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Profiling;
using ArtExpander.Core;
using UnityEngine.Networking;
using System.Collections;

namespace ArtExpander.Core
{
public class AnimatedGhostCache
{
    public IReadOnlyDictionary<string, PerformanceMetrics> FolderMetrics => _folderMetrics;

    public class PerformanceMetrics
    {
        public long LoadTimeMs { get; set; }
        public long MemoryUsageBytes { get; set; }
        public int FrameCount { get; set; }
        public string FolderName { get; set; }
    }

    // Image cache - only populated when needed
    private readonly Dictionary<(EMonsterType, bool), Sprite[]> _animatedGhostCards = new();
    
    // File path cache - populated at startup
    private readonly Dictionary<(EMonsterType, bool), string[]> _animationFilePaths = new();
    
    // Loading state tracking
    private readonly HashSet<(EMonsterType, bool)> _currentlyLoading = new();
    private readonly object _loadLock = new object();
    
    private readonly Dictionary<string, PerformanceMetrics> _folderMetrics = new();


public IEnumerator LoadFolderAsync(EMonsterType monsterType, bool isBlackGhost)
{
    var key = (monsterType, isBlackGhost);
    Plugin.Logger.LogInfo($"Starting async load for monster: {monsterType} (isBlackGhost: {isBlackGhost})");

    // Check if we need to load
    lock (_loadLock)
    {
        if (_animatedGhostCards.ContainsKey(key))
        {
            Plugin.Logger.LogInfo($"Animation already loaded for {monsterType}. Skipping.");
            yield break;
        }
        
        if (_currentlyLoading.Contains(key))
        {
            Plugin.Logger.LogInfo($"Animation already loading for {monsterType}. Skipping.");
            yield break;
        }
        
        if (!_animationFilePaths.ContainsKey(key))
        {
            Plugin.Logger.LogWarning($"No animation paths found for {monsterType}. Cannot load.");
            yield break;
        }
        
        _currentlyLoading.Add(key);
        Plugin.Logger.LogInfo($"Added {monsterType} to loading queue");
    }

    var framePaths = _animationFilePaths[key];
    Plugin.Logger.LogInfo($"Beginning to load {framePaths.Length} frames for {monsterType}");
    
    var frames = new List<Sprite>();
    var stopwatch = Stopwatch.StartNew();
    var initialMemory = GetCurrentMemoryUsage();

    for (int i = 0; i < framePaths.Length; i++)
    {
        var framePath = framePaths[i];
        Plugin.Logger.LogDebug($"Loading frame {i + 1}/{framePaths.Length}: {Path.GetFileName(framePath)}");

        UnityWebRequest request = null;
        try
        {
            request = UnityWebRequest.Get(Path.GetFullPath(framePath));
            request.downloadHandler = new DownloadHandlerTexture();
        }
        catch (Exception ex)
        {
            Plugin.Logger.LogError($"Error creating web request for frame {i + 1} for {monsterType}: {ex.Message}");
            continue;
        }

        yield return request.SendWebRequest();

        try
        {
            if (request.result != UnityWebRequest.Result.Success)
            {
                Plugin.Logger.LogError($"Failed to load frame {i + 1} for {monsterType}: {request.error}");
                continue;
            }

            var texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Plugin.Logger.LogDebug($"Frame {i + 1} loaded: {texture.width}x{texture.height} pixels");
            
            var sprite = Sprite.Create(texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f));
            frames.Add(sprite);
        }
        catch (Exception ex)
        {
            Plugin.Logger.LogError($"Error processing frame {i + 1} for {monsterType}: {ex.Message}");
        }
        finally
        {
            if (request != null)
            {
                request.Dispose();
            }
        }
    }

    if (frames.Count > 0)
    {
        _animatedGhostCards[key] = frames.ToArray();
        
        // Update metrics
        stopwatch.Stop();
        var finalMemory = GetCurrentMemoryUsage();
        var dirName = Path.GetFileName(Path.GetDirectoryName(framePaths[0]));
        var memoryUsed = finalMemory - initialMemory;
        
        _folderMetrics[dirName] = new PerformanceMetrics
        {
            LoadTimeMs = stopwatch.ElapsedMilliseconds,
            MemoryUsageBytes = memoryUsed,
            FrameCount = frames.Count,
            FolderName = dirName
        };

        Plugin.Logger.LogInfo($"Successfully loaded {frames.Count} frames for {monsterType} in {stopwatch.ElapsedMilliseconds}ms " +
            $"(Memory used: {memoryUsed / 1024f / 1024f:F2}MB)");
    }
    else
    {
        Plugin.Logger.LogWarning($"No frames were successfully loaded for {monsterType}");
    }

    lock (_loadLock)
    {
        _currentlyLoading.Remove(key);
        Plugin.Logger.LogInfo($"Removed {monsterType} from loading queue");
    }
}

    public bool TryGetAnimation(EMonsterType monsterType, bool isBlackGhost, out Sprite[] frames)
    {
        var key = (monsterType, isBlackGhost);
        if (_animatedGhostCards.TryGetValue(key, out frames))
        {
            return true;
        }

        // If we have paths but haven't loaded yet, start loading
        if (_animationFilePaths.ContainsKey(key))
        {
            LoadFolderAsync(monsterType, isBlackGhost);
        }

        frames = null;
        return false;
    }
    
    public void ScanAnimationFolders(string rootPath)
    {
        Plugin.Logger.LogInfo($"Starting animation folder scan at path: {rootPath}");
        
        if (!Directory.Exists(rootPath))
        {
            Plugin.Logger.LogError($"Root path does not exist: {rootPath}");
            return;
        }

        var directories = Directory.GetDirectories(rootPath);
        Plugin.Logger.LogInfo($"Found {directories.Length} potential animation directories");

        foreach (var dir in directories)
        {
            var dirName = Path.GetFileName(dir);
            Plugin.Logger.LogInfo($"Processing directory: {dirName}");
            
            bool isBlackGhost = dirName.Contains("_black");
            string monsterName = dirName.Replace("_black", "").Replace("_white","");
            
            Plugin.Logger.LogInfo($"Parsed monster name: {monsterName} (isBlackGhost: {isBlackGhost})");

            if (!FileNameToMonsterTypeResolver.TryResolveMonsterType(monsterName, out EMonsterType monsterType))
            {
                Plugin.Logger.LogWarning($"Could not resolve monster type for animation folder: {dirName}");
                continue;
            }

            Plugin.Logger.LogInfo($"Resolved monster type: {monsterType}");

            var frameFiles = Directory.GetFiles(dir, "*.png")
                .OrderBy(f => int.Parse(Path.GetFileNameWithoutExtension(f)))
                .ToArray();

            if (frameFiles.Length == 0)
            {
                Plugin.Logger.LogWarning($"No frames found in animation folder: {dirName}");
                continue;
            }

            Plugin.Logger.LogInfo($"Found {frameFiles.Length} frame files in {dirName}");
            Plugin.Logger.LogDebug($"Frame files: {string.Join(", ", frameFiles.Select(Path.GetFileName))}");

            _animationFilePaths[(monsterType, isBlackGhost)] = frameFiles;
            Plugin.Logger.LogInfo($"Successfully cached paths for {monsterName} (isBlackGhost: {isBlackGhost})");
        }

        Plugin.Logger.LogInfo($"Scan complete. Total animation sets cached: {_animationFilePaths.Count}");
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
            _folderMetrics.Clear();
        }

    private long GetCurrentMemoryUsage()
    {
        return Profiler.GetTotalAllocatedMemoryLong();
    }

    private void LogPerformanceSummary()
    {
        if (_folderMetrics.Count == 0) return;

        var totalLoadTime = _folderMetrics.Values.Sum(m => m.LoadTimeMs);
        var totalMemory = _folderMetrics.Values.Sum(m => m.MemoryUsageBytes);
        var totalFrames = _folderMetrics.Values.Sum(m => m.FrameCount);

        Plugin.Logger.LogInfo($"=== Animation Loading Performance Summary ===\n" +
            $"Total Folders: {_folderMetrics.Count}\n" +
            $"Total Load Time: {totalLoadTime}ms\n" +
            $"Total Memory Usage: {totalMemory / 1024f / 1024f:F2}MB\n" +
            $"Total Frames: {totalFrames}\n" +
            $"Average Load Time per Folder: {totalLoadTime / _folderMetrics.Count:F2}ms\n" +
            $"Average Memory per Frame: {(totalMemory / (float)totalFrames) / 1024f:F2}KB");
    }

    }
}

