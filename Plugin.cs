using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
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

        // Single configuration entry for animation toggle
        internal static ConfigEntry<bool> EnableAnimations;

        private void Awake()
        {   
            Logger = base.Logger;
            
            // Initialize configuration
            EnableAnimations = Config.Bind(
                "Animation Settings",
                "Enable Animations",
                true,
                "Enable or disable all card animations"
            );

            PluginPath = Path.GetDirectoryName(Info.Location);
            Logger.LogDebug($"Art Expander starting! Plugin path: {PluginPath}");
            
            string cardArtPath = Path.Combine(PluginPath, "cardart");
            string baseArtPath = PluginPath;
            
            bool useCardArtPath = Directory.Exists(cardArtPath);
            string finalArtPath = useCardArtPath ? cardArtPath : baseArtPath;
            Logger.LogDebug($"Using art path: {finalArtPath}");

            if (EnableAnimations.Value)
            {
                string animatedGhostPath = Path.Combine(finalArtPath, "Ghost", "animated");
                animated_ghost_cache.Initialize(animatedGhostPath);
            }
            else
            {
                Logger.LogInfo("Animations disabled in config - skipping animation loading");
            }

            art_cache.Initialize(finalArtPath);
            
            harmony.PatchAll();
            Logger.LogInfo("Patches applied!");
        }
    }
}