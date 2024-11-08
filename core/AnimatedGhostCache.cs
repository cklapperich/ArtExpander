using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using ArtExpander.Core;  // for FileNameToMonsterTypeResolver

namespace ArtExpander.Core{
    public class AnimatedGhostCache 
    {
        private readonly Dictionary<(EMonsterType, bool), Sprite[]> _animatedGhostCards = new();

        public bool TryGetAnimation(EMonsterType monsterType, bool isBlackGhost, out Sprite[] frames)
        {
            Plugin.Logger.LogWarning($"Attempting to retrieve animated ghost for {monsterType},{isBlackGhost}");
            return _animatedGhostCards.TryGetValue((monsterType, isBlackGhost), out frames);
        }

        public void LoadAnimatedFolder(string folderPath)
        {
            if (!Directory.Exists(folderPath)) return;

            // Look for folders named like "PiggyA_frames" or "PiggyA_black_frames"
            var directories = Directory.GetDirectories(folderPath);
            
            foreach (var dir in directories)
            {
                var dirName = Path.GetFileName(dir);
                bool isBlackGhost = dirName.Contains("_black");
                string monsterName = dirName.Replace("_black", "").Replace("_white","");

                if (!FileNameToMonsterTypeResolver.TryResolveMonsterType(monsterName, out EMonsterType monsterType))
                {
                    Plugin.Logger.LogWarning($"Could not resolve monster type for animation folder: {dirName}");
                    continue;
                }

                // Load all PNGs in numerical order
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
                    Plugin.Logger.LogInfo($"Loaded {frames.Count} frames for {monsterType} (Black: {isBlackGhost})");
                }
            }
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
    }
}