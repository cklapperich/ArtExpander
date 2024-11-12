using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ArtExpander.Core
{
    public class AssetBundleLoader : IDisposable
    {
        private AssetBundle _bundle;
        private readonly string _bundlePath;
        private readonly string _rootPath;
        private readonly bool _loadFromBundle;
        private string[] _allAssetNames;

        public AssetBundleLoader(string rootPath, string bundleSuffix)
        {
            _rootPath = rootPath;
            _bundlePath = rootPath + bundleSuffix;
            _loadFromBundle = File.Exists(_bundlePath);
            if (_loadFromBundle)
            {
                _bundle = AssetBundle.LoadFromFile(_bundlePath);
                // For asset bundles, get all asset names and find first PNG
                _allAssetNames = _bundle.GetAllAssetNames();
                Plugin.Logger.LogWarning($"Loaded from bundle {_bundlePath}");
            }
            else{
                Plugin.Logger.LogWarning($"Failed to load from bundle. File {_bundlePath} not found.");
            }
        }

        public string[] GetAllAssetNames()
        {
            return _allAssetNames;
        }

        public bool IsUsingBundle => _loadFromBundle && _bundle != null;

        public Sprite LoadSprite(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                Plugin.Logger.LogWarning($"Path is null or empty  {path}");
                return null;
            }

            try
            {
                if (_loadFromBundle)
                {
                    var sprite = _bundle.LoadAsset<Sprite>(path);
                    if (sprite == null)
                    {
                        Plugin.Logger.LogWarning($"Failed to load sprite from bundle path {path}");
                    }
                    return sprite;
                }
                else
                {
                    // For file loading, combine with root path
                    string fullPath = Path.Combine(_rootPath, path);
                    
                    if (!File.Exists(fullPath))
                    {
                        Plugin.Logger.LogWarning($"File does not exist at path: {fullPath}");
                        return null;
                    }

                    byte[] fileData = File.ReadAllBytes(fullPath);
                    var texture = new Texture2D(2, 2, TextureFormat.RGBA32, mipChain: true);
                    texture.LoadImage(fileData);
                    return Sprite.Create(texture, 
                        new Rect(0, 0, texture.width, texture.height),
                        new Vector2(0.5f, 0.5f));
                }
            }
            catch (Exception ex)
            {
                Plugin.Logger.LogError($"Error loading sprite from {path}: {ex.Message}");
                return null;
            }
        }

        public void Dispose()
        {
            if (_bundle != null)
            {
                _bundle.Unload(true);
                _bundle = null;
            }
        }
    }
}