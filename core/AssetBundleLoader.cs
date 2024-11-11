using System;
using System.IO;
using UnityEngine;
using ArtExpander.Core;  // for Plugin.Logger

namespace ArtExpander.core{
    public class AssetBundleLoader : IDisposable
{
    private AssetBundle _bundle;
    private readonly string _bundlePath;
    private readonly bool _loadFromBundle;

    public AssetBundleLoader(string rootPath, string bundleSuffix)
    {
        _bundlePath = rootPath + bundleSuffix;
        _loadFromBundle = File.Exists(_bundlePath);
        
        if (_loadFromBundle)
        {
            _bundle = AssetBundle.LoadFromFile(_bundlePath);
            if (_bundle == null)
            {
                Plugin.Logger.LogError($"Failed to load asset bundle at {_bundlePath}");
                throw new InvalidOperationException($"Asset bundle could not be loaded from {_bundlePath}");
            }
        }
    }

    public Sprite LoadSprite(string path)
    {
        if (_loadFromBundle)
        {
            return _bundle?.LoadAsset<Sprite>(path);
        }
        
        // File loading logic
        byte[] fileData = File.ReadAllBytes(path);
        var texture = new Texture2D(2, 2, TextureFormat.RGBA32, mipChain: true);
        texture.LoadImage(fileData);
        return Sprite.Create(texture, 
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f));
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