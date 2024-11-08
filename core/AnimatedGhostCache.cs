using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Profiling;
using ArtExpander.Core;

namespace ArtExpander.Core
{
    public class AnimatedGhostCache 
    {
        private readonly Dictionary<(EMonsterType, bool), Sprite[]> _animatedGhostCards = new();
        private readonly Dictionary<string, PerformanceMetrics> _folderMetrics = new();

        public class PerformanceMetrics
        {
            public long LoadTimeMs { get; set; }
            public long MemoryUsageBytes { get; set; }
            public int FrameCount { get; set; }
            public string FolderName { get; set; }
        }

        public IReadOnlyDictionary<string, PerformanceMetrics> FolderMetrics => _folderMetrics;

        public bool TryGetAnimation(EMonsterType monsterType, bool isBlackGhost, out Sprite[] frames)
        {
            return _animatedGhostCards.TryGetValue((monsterType, isBlackGhost), out frames);
        }

        private long GetCurrentMemoryUsage()
        {
            return Profiler.GetTotalAllocatedMemoryLong();
        }

        public void LoadAnimatedFolder(string folderPath)
        {
            if (!Directory.Exists(folderPath)) return;

            var directories = Directory.GetDirectories(folderPath);
            
            foreach (var dir in directories)
            {
                var dirName = Path.GetFileName(dir);
                var stopwatch = Stopwatch.StartNew();
                var initialMemory = GetCurrentMemoryUsage();

                bool isBlackGhost = dirName.Contains("_black");
                string monsterName = dirName.Replace("_black", "").Replace("_white","");

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

                var frames = new List<Sprite>();
                foreach (var framePath in frameFiles)
                {
                    byte[] fileData = File.ReadAllBytes(framePath);
                    Texture2D texture = new Texture2D(2, 2);
                    if (texture.LoadImage(fileData))
                    {
                        var sprite = Sprite.Create(texture, 
                            new Rect(0, 0, texture.width, texture.height),
                            new Vector2(0.5f, 0.5f));
                        frames.Add(sprite);
                    }
                }

                if (frames.Count > 0)
                {
                    _animatedGhostCards[(monsterType, isBlackGhost)] = frames.ToArray();
                    
                    stopwatch.Stop();
                    var finalMemory = GetCurrentMemoryUsage();
                    var memoryUsed = finalMemory - initialMemory;

                    var metrics = new PerformanceMetrics
                    {
                        LoadTimeMs = stopwatch.ElapsedMilliseconds,
                        MemoryUsageBytes = memoryUsed,
                        FrameCount = frames.Count,
                        FolderName = dirName
                    };

                    _folderMetrics[dirName] = metrics;
                }
            }

            // Log summary of all folders
            //LogPerformanceSummary();
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
    }
}