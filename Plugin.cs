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
        internal static AnimationCache animated_ghost_cache;
        internal static new ManualLogSource Logger;
        private readonly Harmony harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        internal static string PluginPath;
        internal static ArtCache art_cache_bundle = new ArtCache();
        internal static ArtCache art_cache_directory = new ArtCache();
        internal static ArtCache foilmask_cache = new ArtCache();
        
        // Configuration entries for animations
        internal static ConfigEntry<bool> EnableAnimations;
        internal static ConfigEntry<int> AnimationFPS;

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

            AnimationFPS = Config.Bind(
                "Animation Settings",
                "Animation FPS",
                10,
                new ConfigDescription(
                    "Frames per second for card animations (requires restart)",
                    new AcceptableValueRange<int>(1, 30)
                )
            );

            PluginPath = Path.GetDirectoryName(Info.Location);
            Logger.LogDebug($"Art Expander starting! Plugin path: {PluginPath}");
            
            string cardArtPath = Path.Combine(PluginPath, "cardart");
            if (EnableAnimations.Value)
            {
                //needs a reference to a monobehavior so it can launch coroutines, coroutines are so it can load frames asynchronously when ready
                animated_ghost_cache = new AnimationCache(this); 
                string animatedGhostPath = Path.Combine(PluginPath, "animated");
                animated_ghost_cache.Initialize(animatedGhostPath);
            }
            else
            {
                Logger.LogInfo("Animations disabled in config - skipping animation loading");
            }

            // Initialize bundle cache (loads from cardart.assets if it exists)
            string cardArtBundlePath = cardArtPath + ".assets";
            if (File.Exists(cardArtBundlePath))
            {
                art_cache_bundle.Initialize(cardArtBundlePath);
            }
            else
            {
                Logger.LogInfo("No cardart.assets bundle found, skipping bundle cache");
            }

            // Initialize directory cache (loads from cardart/ folder)
            if (Directory.Exists(cardArtPath))
            {
                art_cache_directory.Initialize(cardArtPath);
            }
            else
            {
                Logger.LogInfo("No cardart directory found, skipping directory cache");
            }

            // Try foilmask.assets bundle first, fall back to foilmask/ directory
            string foilmaskBundlePath = Path.Combine(PluginPath, "foilmask.assets");
            if (File.Exists(foilmaskBundlePath))
            {
                foilmask_cache.Initialize(foilmaskBundlePath);
            }
            else
            {
                string foilmaskPath = Path.Combine(PluginPath, "foilmask");
                foilmask_cache.Initialize(foilmaskPath);
            }
            harmony.PatchAll();
            Logger.LogInfo("Patches applied!");
        }
    }
}