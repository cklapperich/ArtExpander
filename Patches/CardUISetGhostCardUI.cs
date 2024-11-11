using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using ArtExpander.Core;
//TODO: ADD SUPPORT FOR UNITY ASYNC ASSET BUNDLE LOADING?

namespace ArtExpander.Patches
{
    [HarmonyPatch(typeof(CardUI))]
    public class GhostCardPatch
    {
        [HarmonyPatch("SetGhostCardUI")]
        [HarmonyPostfix]
        static void Postfix(CardUI __instance, MonsterData data, bool isBlackGhost)
        {
            // Clean up existing animators
            var ghostCard = AccessTools.Field(typeof(CardUI), "m_GhostCard").GetValue(__instance) as CardUI;
            if (ghostCard != null)
            {
                var existingAnimators = ghostCard.GetComponents<GhostCardAnimatedRenderer>();
                foreach(var animator in existingAnimators)
                {
                    animator.StopAnimation();
                    Object.Destroy(animator);
                }
            }

            // Use the appropriate ghost border type based on isBlackGhost
            var borderType = isBlackGhost ? ArtCache.GhostBlackBorder : ArtCache.GhostWhiteBorder;

            if (Plugin.animated_ghost_cache.TryGetAnimation(
                monsterType: data.MonsterType,
                borderType: borderType,
                expansionType: ECardExpansionType.Ghost,
                isFoil: __instance.GetCardData().isFoil,
                out var frames))
            {
                if (ghostCard == null)
                {
                    return;
                }

                var mainImage = AccessTools.Field(typeof(CardUI), "m_MonsterImage").GetValue(ghostCard) as Image;
                var maskImage = AccessTools.Field(typeof(CardUI), "m_MonsterMaskImage").GetValue(ghostCard) as Image;
                var glowImage = AccessTools.Field(typeof(CardUI), "m_MonsterGlowMask").GetValue(ghostCard) as Image;
                
                if (mainImage == null || maskImage == null)
                {
                    return;
                }
                
                var animator = ghostCard.gameObject.AddComponent<GhostCardAnimatedRenderer>();
                animator.Initialize(mainImage, maskImage, glowImage, frames);
            }
        }
    }
}