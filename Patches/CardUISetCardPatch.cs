using System;
using HarmonyLib;
using UnityEngine;
using MonoMod.RuntimeDetour;  // If needed for Harmony patches

namespace ArtExpander.Patches
{
    [HarmonyPatch(typeof(CardUI))]
    [HarmonyPatch("SetCardUI")]
    [HarmonyPatch(new Type[] { typeof(CardData) })]
    public class CardUISetCardPatch
    {
        // Made internal so it's accessible within the assembly but not exposed publicly
        internal static class CardDataTracker
        {
            private static CardData currentCardData;

            public static void SetCurrentCard(CardData data)
            {
                currentCardData = data;
            }

            public static void ClearCurrentCard()
            {
                currentCardData = null;
            }

            public static (ECardBorderType borderType, bool isDestiny, bool isFoil, EMonsterType monsterType) GetCurrentCardInfo()
            {   
                if (currentCardData != null)
                {
                    return (currentCardData.borderType, currentCardData.isDestiny, currentCardData.isFoil, currentCardData.monsterType);
                }
                return (ECardBorderType.Base, false, false, EMonsterType.PiggyA);
            }
        }

        static void Prefix(CardData cardData)
        {
            CardDataTracker.SetCurrentCard(cardData);
        }

        static void Postfix()
        {
            CardDataTracker.ClearCurrentCard();
        }
    }
}