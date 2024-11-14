using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using ArtExpander.Core;
using System;

namespace ArtExpander.Patches
{
    [HarmonyPatch(typeof(CardUI))]
    public class GhostCardPatch
    {
        private static readonly string LOG_PREFIX = "[GhostCardPatch] ";
        
        private static void LogError(string message, Exception ex = null)
        {
            Plugin.Logger.LogError($"{LOG_PREFIX}{message}" + (ex != null ? $"\nException: {ex}" : ""));
        }

        private static void LogWarning(string message)
        {
            Plugin.Logger.LogWarning($"{LOG_PREFIX}{message}");
        }

        private static void LogInfo(string message)
        {
            Plugin.Logger.LogInfo($"{LOG_PREFIX}{message}");
        }

        [HarmonyPatch("SetGhostCardUI")]
        [HarmonyPostfix]
        static void Postfix(CardUI __instance, MonsterData data, bool isBlackGhost)
        {
            try
            {
                // Clean up existing animators first
                var ghostCard = AccessTools.Field(typeof(CardUI), "m_GhostCard").GetValue(__instance) as CardUI;
                if (ghostCard != null)
                {
                    var existingAnimators = ghostCard.GetComponents<GhostCardAnimatedRenderer>();
                    foreach(var animator in existingAnimators)
                    {
                        animator.StopAnimation();
                        UnityEngine.Object.Destroy(animator);
                    }
                }
                else 
                {
                    return;
                }

                CardData card_data = __instance.GetCardData();
                // Get required references
                Image mainImage = null, maskImage = null, glowImage = null;
        
                mainImage = AccessTools.Field(typeof(CardUI), "m_MonsterImage").GetValue(ghostCard) as Image;
                maskImage = AccessTools.Field(typeof(CardUI), "m_MonsterMaskImage").GetValue(ghostCard) as Image;
                glowImage = AccessTools.Field(typeof(CardUI), "m_MonsterGlowMask").GetValue(ghostCard) as Image;

                // Request animation
                bool loaded_frames = false;

                try
                {
                    loaded_frames = Plugin.animated_ghost_cache.RequestAnimationForCard(
                        monsterType: data.MonsterType,
                        borderType: card_data.borderType,
                        expansionType: ECardExpansionType.Ghost,
                        isBlackGhost: isBlackGhost,
                        isFoil: card_data.isFoil,
                        onFramesReady: (Sprite[] frames) => {
                            try
                            {
                                if (frames == null || frames.Length == 0)
                                {
                                    LogError("Received null or empty frames array");
                                    return;
                                }

                                if (ghostCard == null || ghostCard.gameObject == null)
                                {
                                    LogWarning("Ghost card was destroyed before animation could be applied");
                                    return;
                                }

                                var animator = ghostCard.gameObject.AddComponent<GhostCardAnimatedRenderer>();
                                animator.Initialize(mainImage, maskImage, glowImage, frames);
                            }
                            catch (Exception ex)
                            {
                                LogError("Error in animation callback", ex);
                            }
                        });
                }
                catch (Exception ex)
                {
                    LogError("Error requesting animation", ex);
                }
            }
            catch (Exception ex)
            {
                LogError("Unhandled error in SetGhostCardUI postfix", ex);
            }
        }
    }
}