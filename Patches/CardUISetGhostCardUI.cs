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
            CardData card_data = __instance.GetCardData();

            if (Plugin.animated_ghost_cache.TryGetAnimation(
                monsterType: data.MonsterType,
                borderType: card_data.borderType,
                expansionType: ECardExpansionType.Ghost,
                isBlackGhost:isBlackGhost,
                isFoil: card_data.isFoil,
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