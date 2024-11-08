// using HarmonyLib;
// using UnityEngine;
// using UnityEngine.UI;

// namespace ArtExpander.Patches
// {
//     [HarmonyPatch(typeof(CardUI))]
//     public class GhostCardPatch
//     {
//         [HarmonyPatch("SetGhostCardUI")]
//         [HarmonyPostfix]
//         static void Postfix(CardUI __instance, MonsterData data, bool isBlackGhost)
//         {
//             if (Plugin.animated_ghost_cache.TryGetAnimation(data.MonsterType, isBlackGhost, out var frames))
//             {
//                 // Get the ghost card component via reflection since it's private
//                 var ghostCard = AccessTools.Field(typeof(CardUI), "m_GhostCard").GetValue(__instance) as CardUI;
//                 if (ghostCard == null) return;

//                 var mainImage = AccessTools.Field(typeof(CardUI), "m_MonsterImage").GetValue(ghostCard) as Image;
//                 var maskImage = AccessTools.Field(typeof(CardUI), "m_MonsterMaskImage").GetValue(ghostCard) as Image;
                
//                 if (mainImage == null || maskImage == null) return;

//                 // Add our animator component
//                 var animator = ghostCard.gameObject.AddComponent<GhostCardAnimatedRenderer>();
//                 animator.Initialize(mainImage, maskImage, frames);
//             }
//         }
//     }
// }