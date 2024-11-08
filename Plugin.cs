// File: Plugin.cs
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using System.IO;
using ArtExpander.Core;

namespace ArtExpander
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    [BepInProcess("Card Shop Simulator.exe")]
    [BepInDependency("shaklin.TextureReplacer", BepInDependency.DependencyFlags.SoftDependency)]
    public class Plugin : BaseUnityPlugin
    {

        internal static AnimatedGhostCache animated_ghost_cache = new AnimatedGhostCache();

        internal static new ManualLogSource Logger;
        private readonly Harmony harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        internal static string PluginPath;
        internal static ArtCache art_cache = new ArtCache();

        private void Awake()
        {   
            Logger = base.Logger;
            PluginPath = Path.GetDirectoryName(Info.Location);
            Logger.LogInfo($"Art Expander starting! Plugin path: {PluginPath}");
            
            string cardArtPath = Path.Combine(PluginPath, "cardart");
            string baseArtPath = PluginPath;
            
            bool useCardArtPath = Directory.Exists(cardArtPath);
            string finalArtPath = useCardArtPath ? cardArtPath : baseArtPath;
            Logger.LogInfo($"Using art path: {finalArtPath}");

            string animatedGhostPath = Path.Combine(finalArtPath, "Ghost", "animated");
            animated_ghost_cache.LoadAnimatedFolder(animatedGhostPath);

            art_cache.Initialize(finalArtPath);
            
            harmony.PatchAll();
            Logger.LogInfo("Patches applied!");
        }
    }
}
