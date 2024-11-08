
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
    public static bool Prefix(MonsterData __instance, ECardExpansionType cardExpansionType, ref Sprite __result)
    {
        var (borderType, isDestiny) = CardUISetCardPatch.CardDataTracker.GetCurrentCardInfo();

        string artPath = Plugin.art_cache.ResolveArtPath(
            __instance.MonsterType,
            borderType,
            cardExpansionType,
            isDestiny
        );

        if (!string.IsNullOrEmpty(artPath))
        {
            var customSprite = Plugin.art_cache.LoadSprite(artPath);
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