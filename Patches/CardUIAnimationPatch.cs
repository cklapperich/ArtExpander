using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using ArtExpander.Core;
using System;

namespace ArtExpander.Patches
{
    [HarmonyPatch(typeof(CardUI))]
    public class CardUIAnimationPatch
    {
        private static readonly string LOG_PREFIX = "[CardUIAnimationPatch] ";

        // Cache reflection field lookup to avoid repeated reflection calls
        private static readonly System.Reflection.FieldInfo m_CenterFrameImageField =
            AccessTools.Field(typeof(CardUI), "m_CenterFrameImage");

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

        /// <summary>
        /// Common logic for applying animations to any CardUI object.
        /// </summary>
        private static void TryApplyAnimation(
            CardUI targetCardUI,
            EMonsterType monsterType,
            ECardBorderType borderType,
            ECardExpansionType expansionType,
            bool isBlackVariant,
            bool isFoil)
        {
            if (!Plugin.EnableAnimations.Value)
                return;

            if (targetCardUI == null)
                return;

            // Check if animation cache is initialized
            if (Plugin.animated_ghost_cache == null)
                return;

            try
            {
                // Get required Image references using cached field
                Image mainImage = m_CenterFrameImageField.GetValue(targetCardUI) as Image;

                // Request animation from cache
                try
                {
                    bool loaded_frames = Plugin.animated_ghost_cache.RequestAnimationForCard(
                        monsterType: monsterType,
                        borderType: borderType,
                        expansionType: expansionType,
                        isBlackGhost: isBlackVariant,
                        isFoil: isFoil,
                        onFramesReady: (Sprite[] frames) =>
                        {
                            try
                            {
                                if (frames == null || frames.Length == 0)
                                {
                                    LogError("Received null or empty frames array");
                                    return;
                                }

                                if (targetCardUI == null || targetCardUI.gameObject == null)
                                {
                                    LogWarning("CardUI was destroyed before animation could be applied");
                                    return;
                                }

                                // Try to reuse existing animator component (object pooling)
                                var animator = targetCardUI.gameObject.GetComponent<GhostCardAnimatedRenderer>();
                                if (animator != null)
                                {
                                    // Reuse existing component - avoids GC pressure from constant destroy/create
                                    animator.Reinitialize(mainImage, frames, Plugin.AnimationFPS.Value);
                                }
                                else
                                {
                                    // Create new component only if none exists
                                    animator = targetCardUI.gameObject.AddComponent<GhostCardAnimatedRenderer>();
                                    animator.Initialize(mainImage, frames, Plugin.AnimationFPS.Value);
                                }
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
                LogError("Unhandled error in TryApplyAnimation", ex);
            }
        }

        /// <summary>
        /// Patch for SetCardUI - handles all card types (Normal, FullArt, Ghost, Special).
        /// This is the main entry point for applying animations to any card.
        /// </summary>
        [HarmonyPatch("SetCardUI")]
        [HarmonyPatch(new Type[] { typeof(CardData) })]
        [HarmonyPostfix]
        static void SetCardUI_Postfix(CardUI __instance, CardData cardData)
        {
            if (cardData == null)
                return;

            TryApplyAnimation(
                targetCardUI: __instance,
                monsterType: cardData.monsterType,
                borderType: cardData.GetCardBorderType(),
                expansionType: cardData.expansionType,
                isBlackVariant: cardData.isDestiny,
                isFoil: cardData.isFoil
            );
        }
    }
}
