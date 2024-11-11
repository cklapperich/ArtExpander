using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using ArtExpander.Core;

namespace ArtExpander.Patches
{
    [HarmonyPatch(typeof(CardUI))]
    public class GhostCardPatch
    {
        [HarmonyPatch("SetGhostCardUI")]
        [HarmonyPostfix]
        static void Postfix(CardUI __instance, MonsterData data, bool isBlackGhost)
        {
            // Plugin.Logger.LogInfo($"=== SetGhostCardUI Called ===\n" +
            //     $"Monster: {data.MonsterType}\n" +
            //     $"Is Black Ghost: {isBlackGhost}\n" +
            //     $"Card GameObject Active: {__instance.gameObject.activeInHierarchy}");

            // Clean up first
            var ghostCard = AccessTools.Field(typeof(CardUI), "m_GhostCard").GetValue(__instance) as CardUI;
            if (ghostCard != null)
            {
                var existingAnimators = ghostCard.GetComponents<GhostCardAnimatedRenderer>();
                //Plugin.Logger.LogInfo($"Found {existingAnimators.Length} existing animators");
                
                foreach(var animator in existingAnimators)
                {
                    animator.StopAnimation();
                    Object.Destroy(animator);
                }
            }

            // Then only add new animation if needed
            if (Plugin.animated_ghost_cache.TryGetAnimation(data.MonsterType, isBlackGhost, out var frames))
            {
                if (ghostCard == null)
                {
                    //Plugin.Logger.LogWarning("Ghost card is null, cannot add animator");
                    return;
                }

                var mainImage = AccessTools.Field(typeof(CardUI), "m_MonsterImage").GetValue(ghostCard) as Image;
                var maskImage = AccessTools.Field(typeof(CardUI), "m_MonsterMaskImage").GetValue(ghostCard) as Image;
                var glowImage = AccessTools.Field(typeof(CardUI), "m_MonsterGlowMask").GetValue(ghostCard) as Image;
                
                if (mainImage == null || maskImage == null)
                {
                    //Plugin.Logger.LogWarning("Required images are null, cannot add animator");
                    return;
                }

                // Plugin.Logger.LogWarning($"Setting up ghost card animation:\n" +
                //     $"Monster Type: {data.MonsterType}\n" +
                //     $"Ghost Card null?: {ghostCard == null}\n" +
                //     $"Animation Frames Found: {frames != null}\n" +
                //     $"Frame Count: {(frames != null ? frames.Length : 0)}");
                
                var animator = ghostCard.gameObject.AddComponent<GhostCardAnimatedRenderer>();
                animator.Initialize(mainImage, maskImage, glowImage, frames);
            }
        }
    }
}