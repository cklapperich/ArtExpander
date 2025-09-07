using HarmonyLib;
using System;
using UnityEngine;

namespace ArtExpander.Patches
{
    [HarmonyPatch(typeof(MonsterData_ScriptableObject))]
    [HarmonyPatch("GetCardFoilMaskSprite")]
    [HarmonyPatch(new Type[] { typeof(ECardExpansionType) })]
    public class GetFoilMaskPatch
    {
        public static bool Prefix(MonsterData_ScriptableObject __instance, ECardExpansionType cardExpansionType, ref Sprite __result)
        {
            var cardData = CardUISetCardPatch.CardDataTracker.GetCurrentCardInfo();
            if (cardData == null) 
            {
                return true; // Fall back to original method
            }
            
            string artPath = Plugin.foilmask_cache.ResolveArtPath(
                cardData.monsterType,
                cardData.borderType,
                cardData.expansionType,
                cardData.isDestiny,
                cardData.isFoil
            );

            if (!string.IsNullOrEmpty(artPath))
            {
                var customSprite = Plugin.foilmask_cache.LoadSprite(artPath);
                if (customSprite != null)
                {
                    __result = customSprite;
                    return false;
                }
            }
            return true;
        }
    }
}