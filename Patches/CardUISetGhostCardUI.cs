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
    // First clean up any existing animators
    var ghostCard = AccessTools.Field(typeof(CardUI), "m_GhostCard").GetValue(__instance) as CardUI;
    if (ghostCard != null)
    {   
        var animator = ghostCard.GetComponent<GhostCardAnimatedRenderer>();
        if (animator != null)
        {
            animator.StopAnimation();
            UnityEngine.Object.Destroy(animator);
        }
    }

    // Try to get animation frames
    Sprite[] frames;
    if (!Plugin.animated_ghost_cache.TryGetAnimation(data.MonsterType, isBlackGhost, out frames))
    {
        // TryGetAnimation will start async load if needed
        return; // Use default ghost icon while loading
    }

    // We have frames, set up the animation
    if (frames != null && frames.Length > 0)
    {
        if (ghostCard == null) return;

        var mainImage = AccessTools.Field(typeof(CardUI), "m_MonsterImage").GetValue(ghostCard) as Image;
        var maskImage = AccessTools.Field(typeof(CardUI), "m_MonsterMaskImage").GetValue(ghostCard) as Image;
        var glowImage = AccessTools.Field(typeof(CardUI), "m_MonsterGlowMask").GetValue(ghostCard) as Image;
        if (mainImage == null || maskImage == null) return;

        var animator = ghostCard.gameObject.AddComponent<GhostCardAnimatedRenderer>();
        animator.Initialize(mainImage, maskImage, glowImage, frames);
    }
}
    }
}