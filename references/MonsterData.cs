using System;
using System.Collections.Generic;
using I2.Loc;
using UnityEngine;

[Serializable]
public class MonsterData
{
    public string Name;

    public string ArtistName;

    public string Description;

    public Vector3 EffectAmount;

    public EElementIndex ElementIndex;

    public ERarity Rarity;

    public EMonsterType MonsterType;

    public EMonsterType NextEvolution;

    public EMonsterType PreviousEvolution;

    public List<EMonsterRole> Roles;

    public Stats BaseStats;

    public List<ESkill> SkillList;

    public Sprite Icon;

    public Sprite GhostIcon;

    public string GetName()
    {
        return LocalizationManager.GetTranslation(Name);
    }

    public string GetArtistName()
    {
        return "Illus. " + ArtistName;
    }

    public string GetDescription()
    {
        if (Description == "")
        {
            return LocalizationManager.GetTranslation("No effect");
        }
        return LocalizationManager.GetTranslation(Description).Replace("XXX", EffectAmount.x.ToString()).Replace("YYY", EffectAmount.y.ToString())
            .Replace("ZZZ", EffectAmount.z.ToString());
    }

    public string GetRarityName()
    {
        return LocalizationManager.GetTranslation(Rarity.ToString());
    }

    public string GetElementName()
    {
        return LocalizationManager.GetTranslation(ElementIndex.ToString());
    }

    public string GetRoleName()
    {
        string text = "";
        string result = "";
        for (int i = 0; i < Roles.Count; i++)
        {
            if (i > 0)
            {
                text += ", ";
            }
            string translation = LocalizationManager.GetTranslation(Roles[i].ToString());
            if (Roles[i] == EMonsterRole.AllRounder)
            {
                translation = LocalizationManager.GetTranslation("All Rounder");
            }
            else if (Roles[i] == EMonsterRole.MagicalAttacker)
            {
                translation = LocalizationManager.GetTranslation("Magical Attacker");
            }
            else if (Roles[i] == EMonsterRole.PhysicalAttacker)
            {
                translation = LocalizationManager.GetTranslation("Physical Attacker");
            }
            text += translation;
            if (i == 0)
            {
                result = translation;
            }
        }
        if (text.Length > 27)
        {
            return result;
        }
        return text;
    }

    public Sprite GetIcon(ECardExpansionType cardExpansionType)
    {
        if (cardExpansionType == ECardExpansionType.Ghost)
        {
            return GhostIcon;
        }
        if ((Object)(object)Icon == (Object)null)
        {
            if (cardExpansionType == ECardExpansionType.None)
            {
                return LoadStreamTexture.GetImage("Special_" + MonsterType);
            }
            return LoadStreamTexture.GetImage(cardExpansionType.ToString() + "_" + MonsterType);
        }
        return Icon;
    }
}
