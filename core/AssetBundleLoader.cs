using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;


namespace ArtExpander.Core
{
    public class AssetBundleLoader : IDisposable
    {
        private AssetBundle _bundle;
        private readonly string _bundlePath;
        private readonly string _rootPath;
        private bool _loadFromBundle;
        private string[] _allAssetNames;

       private static (bool isValid, string compression) CheckAssetBundle(string filePath)
        {
            string ReadNullTerminatedString(BinaryReader reader)
            {
                var bytes = new List<byte>();
                byte b;
                while ((b = reader.ReadByte()) != 0)
                {
                    bytes.Add(b);
                }
                return Encoding.UTF8.GetString(bytes.ToArray());
            }
            const string UNITY_FS_MAGIC = "UnityFS";
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            using (BinaryReader reader = new BinaryReader(fs, Encoding.UTF8))
            {
                var actualFileSize = new FileInfo(filePath).Length;

                // Read signature (8 bytes)
                byte[] signatureBytes = reader.ReadBytes(8);
                string signature = Encoding.ASCII.GetString(signatureBytes).TrimEnd('\0');
                if (!signature.StartsWith(UNITY_FS_MAGIC))
                {
                    return (false, "Unknown");
                }

                // Read format version (should be just 1 byte, skip the rest)
                byte formatVersion = reader.ReadByte();
                reader.ReadBytes(3); // Skip padding

                // Read Unity version string (null-terminated)
                var unityVersion = ReadNullTerminatedString(reader);

                // Read generator version string (null-terminated)
                var generatorVersion = ReadNullTerminatedString(reader);

                // Read file size and block info
                // Reading bytes manually to control byte order
                byte[] fileSizeBytes = reader.ReadBytes(8);
                byte[] compressedSizeBytes = reader.ReadBytes(4);
                byte[] uncompressedSizeBytes = reader.ReadBytes(4);
                byte[] flagsBytes = reader.ReadBytes(4);

                // Convert values maintaining proper byte order
                long fileSize = BitConverter.ToInt64(fileSizeBytes, 0);
                uint compressedBlocksInfoSize = BitConverter.ToUInt32(compressedSizeBytes, 0);
                uint uncompressedBlocksInfoSize = BitConverter.ToUInt32(uncompressedSizeBytes, 0);
                uint flags = BitConverter.ToUInt32(flagsBytes, 0);

                // Extract compression type from flags (bits 16-17)
                int compressionType = (int)((flags >> 16) & 0x3);
                string compressionName = compressionType switch
                {
                    0 => "None",
                    1 => "LZMA",
                    2 => "LZ4",
                    3 => "LZ4HC",
                    _ => "Unknown"
                };
                return (true,compressionName);
            }
        }

        public AssetBundleLoader(string rootPath, string bundleSuffix)
        {
            _rootPath = rootPath;
            _bundlePath = rootPath + bundleSuffix;
            _loadFromBundle = File.Exists(_bundlePath);
            if (_loadFromBundle)
            {
                try
                {
                   bool correct_format = false;
                   string compression_type = "Unknown";
                   (correct_format,compression_type) = CheckAssetBundle(_bundlePath);
                   
                   if (!correct_format){
                       Plugin.Logger.LogError($"!!! AssetBundle not in a readable unity 2021.3.38f format. Please rebuild bundle. Filepath: {_bundlePath}");
                       _loadFromBundle=false;
                       return;
                   }
                   
                    if (compression_type != "None" && compression_type != "LZ4"){
                        Plugin.Logger.LogError($"!!! ASSETBUNDLE NOT IN LZ4 format. compression_type is {compression_type} Please package in LZ4 or UNCOMPRESSED for better performance.");
                    }


                    _bundle = AssetBundle.LoadFromFile(_bundlePath);
                    _allAssetNames = _bundle.GetAllAssetNames();

                }
                catch (Exception ex)
                {
                    Plugin.Logger.LogError($"Failed to check or load bundle {_bundlePath}: {ex.Message}");
                    _loadFromBundle = false;
                }
            }
            else
            {
                Plugin.Logger.LogWarning($"Failed to load from bundle. File {_bundlePath} not found.");
                _loadFromBundle = false;
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