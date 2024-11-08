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
    // Always clean up first
    var ghostCard = AccessTools.Field(typeof(CardUI), "m_GhostCard").GetValue(__instance) as CardUI;
    if (ghostCard != null)
    {
        var animators = ghostCard.GetComponentsInChildren<GhostCardAnimatedRenderer>(true);
        foreach(var animator in animators)
        {
            animator.StopAnimation();
            Object.Destroy(animator);
        }
    }

    // Then only add new animation if needed
    if (Plugin.animated_ghost_cache.TryGetAnimation(data.MonsterType, isBlackGhost, out var frames))
    {
        if (ghostCard == null) return;

        var mainImage = AccessTools.Field(typeof(CardUI), "m_MonsterImage").GetValue(ghostCard) as Image;
        var maskImage = AccessTools.Field(typeof(CardUI), "m_MonsterMaskImage").GetValue(ghostCard) as Image;
        
        if (mainImage == null || maskImage == null) return;
        
        var animator = ghostCard.gameObject.AddComponent<GhostCardAnimatedRenderer>();
        animator.Initialize(mainImage, maskImage, frames);
    }
}
    }
}