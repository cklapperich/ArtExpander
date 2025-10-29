
// File: Patches/GetIconPatch.cs
using HarmonyLib;
using System;
using UnityEngine;

namespace ArtExpander.Patches
{
[HarmonyPatch(typeof(MonsterData))]
[HarmonyPatch("GetIcon")]
[HarmonyPatch(new Type[] { typeof(ECardExpansionType) })]
public class GetIconPatch
{
    private static Sprite TryLoadFromCache(Core.ArtCache cache, CardData cardData)
    {
        string artPath = cache.ResolveArtPath(
            cardData.monsterType,
            cardData.borderType,
            cardData.expansionType,
            cardData.isDestiny,
            cardData.isFoil
        );
        if (!string.IsNullOrEmpty(artPath))
        {
            return cache.LoadSprite(artPath);
        }

        return null;
    }

    public static bool Prefix(MonsterData __instance, ECardExpansionType cardExpansionType, ref Sprite __result)
    {
        var cardData = CardUISetCardPatch.CardDataTracker.GetCurrentCardInfo();
        if (cardData is null)
        {
            return true;
        }
        // Try directory cache first, then bundle cache
        var customSprite = TryLoadFromCache(Plugin.art_cache_directory, cardData)
                        ?? TryLoadFromCache(Plugin.art_cache_bundle, cardData);

        if (customSprite != null)
        {
            __result = customSprite;
            return false;
        }

        // Fall back to original game function
        return true;
    }
}
}