using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Inventory/List", order = 5)]
public class MonsterData_ScriptableObject : ScriptableObject
{
    public List<EMonsterType> m_ShownMonsterList;

    public List<EMonsterType> m_ShownGhostMonsterList;

    public List<EMonsterType> m_ShownMegabotList;

    public List<EMonsterType> m_ShownFantasyRPGList;

    public List<EMonsterType> m_ShownCatJobList;

    public List<MonsterData> m_DataList;

    public List<MonsterData> m_MegabotDataList;

    public List<MonsterData> m_FantasyRPGDataList;

    public List<MonsterData> m_CatJobDataList;

    public List<Color> m_RarityColor;

    public List<Sprite> m_CardBackImageList;

    public List<Sprite> m_CardFoilMaskImageList;

    public List<Sprite> m_GradedCardScratchTextureList;

    public List<GradeCardServiceData> m_GradeCardServiceDataList;

    public List<CardUISetting> m_CardUISettingList;

    public List<Sprite> m_TetramonImageList;

    public List<MonsterData> m_SpecialCardImageList;

    public MonsterData GetMonsterData(string monsterType)
    {
        for (int i = 0; i < m_DataList.Count; i++)
        {
            if (m_DataList[i].MonsterType.ToString() == monsterType)
            {
                return m_DataList[i];
            }
        }
        return m_DataList[0];
    }

    public Color GetRarityColor(ERarity rarity)
    {
        return m_RarityColor[(int)rarity];
    }

    public Sprite GetGradedCardScratchTexture(int cardGrade)
    {
        return m_GradedCardScratchTextureList[cardGrade];
    }

    public GradeCardServiceData GetGradeCardServiceData(int serviceLevel)
    {
        return m_GradeCardServiceDataList[serviceLevel];
    }

    public Sprite GetCardBackSprite(ECardExpansionType cardExpansionType)
    {
        return m_CardBackImageList[(int)cardExpansionType];
    }

    public Sprite GetCardFoilMaskSprite(ECardExpansionType cardExpansionType)
    {
        return m_CardFoilMaskImageList[(int)cardExpansionType];
    }
}
