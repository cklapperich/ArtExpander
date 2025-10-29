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
        /// Determines which CardUI object should receive animation based on card properties.
        /// Mirrors the game's logic from CardUI.SetCardUI (lines 360-427).
        /// </summary>
        private static CardUI DetermineTargetCardUI(CardUI instance, CardData cardData)
        {
            if (cardData == null || instance == null)
                return null;

            ECardBorderType borderType = cardData.GetCardBorderType();

            // Ghost FullArt cards use a separate nested CardUI (m_GhostCard)
            if (cardData.expansionType == ECardExpansionType.Ghost && borderType == ECardBorderType.FullArt)
            {
                return AccessTools.Field(typeof(CardUI), "m_GhostCard").GetValue(instance) as CardUI;
            }
            // Other FullArt cards use m_FullArtCard
            else if (borderType == ECardBorderType.FullArt)
            {
                return AccessTools.Field(typeof(CardUI), "m_FullArtCard").GetValue(instance) as CardUI;
            }
            // Normal cards and special cards use the main instance
            else
            {
                return instance;
            }
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
                // Clean up existing animators first
                var existingAnimators = targetCardUI.GetComponents<GhostCardAnimatedRenderer>();
                foreach (var animator in existingAnimators)
                {
                    animator.StopAnimation();
                    UnityEngine.Object.Destroy(animator);
                }

                // Get required Image references
                Image mainImage = AccessTools.Field(typeof(CardUI), "m_MonsterImage").GetValue(targetCardUI) as Image;
                Image maskImage = AccessTools.Field(typeof(CardUI), "m_MonsterMaskImage").GetValue(targetCardUI) as Image;
                Image glowImage = AccessTools.Field(typeof(CardUI), "m_MonsterGlowMask").GetValue(targetCardUI) as Image;

                // Request animation from cache
                try
                {
                    bool loaded_frames = Plugin.animated_ghost_cache.RequestAnimationForCard(
                        monsterType: monsterType,
                        borderType: borderType,
                        expansionType: expansionType,
                        isBlackGhost: isBlackVariant,
                        isFoil: isFoil,
                        onFramesReady: (Sprite[] frames) => {
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

                                var animator = targetCardUI.gameObject.AddComponent<GhostCardAnimatedRenderer>();
                                animator.Initialize(mainImage, maskImage, glowImage, frames, Plugin.AnimationFPS.Value);
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
        /// Patch for SetGhostCardUI - handles Ghost FullArt cards specifically.
        /// Kept for compatibility and edge cases where SetCardUI might not trigger.
        /// </summary>
        [HarmonyPatch("SetGhostCardUI")]
        [HarmonyPostfix]
        static void SetGhostCardUI_Postfix(CardUI __instance, MonsterData data, bool isBlackGhost)
        {
            CardData card_data = __instance.GetCardData();
            if (card_data == null)
                return;

            var ghostCard = AccessTools.Field(typeof(CardUI), "m_GhostCard").GetValue(__instance) as CardUI;

            TryApplyAnimation(
                targetCardUI: ghostCard,
                monsterType: data.MonsterType,
                borderType: card_data.borderType,
                expansionType: card_data.expansionType,
                isBlackVariant: isBlackGhost,
                isFoil: card_data.isFoil
            );
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

            // Skip nested FullArt/Ghost CardUI instances - we only patch the parent
            var isNestedField = AccessTools.Field(typeof(CardUI), "m_IsNestedFullArt");
            if (isNestedField != null)
            {
                bool isNested = (bool)isNestedField.GetValue(__instance);
                if (isNested)
                    return;
            }

            // Determine which CardUI object to animate based on card type
            CardUI targetCardUI = DetermineTargetCardUI(__instance, cardData);

            if (targetCardUI != null)
            {
                TryApplyAnimation(
                    targetCardUI: targetCardUI,
                    monsterType: cardData.monsterType,
                    borderType: cardData.GetCardBorderType(),
                    expansionType: cardData.expansionType,
                    isBlackVariant: cardData.isDestiny,
                    isFoil: cardData.isFoil
                );
            }
        }
    }
}
