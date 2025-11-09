using System;
using System.Collections.Generic;

[Serializable]
public class CardUISetting
{
    public ECardExpansionType expansionType;

    public bool openPackCanUseRarity;

    public bool openPackCanHaveDuplicate;

    public List<CardUISettingData> cardUISettingDataList;

    public CardUISettingData GetCardUISettingData(ECardBorderType cardBorderType, bool isDestiny)
    {
        if (isDestiny)
        {
            for (int i = cardUISettingDataList.Count / 2; i < cardUISettingDataList.Count; i++)
            {
                if (cardUISettingDataList[i].applicableBorderList.Contains(cardBorderType))
                {
                    return cardUISettingDataList[i];
                }
            }
        }
        for (int j = 0; j < cardUISettingDataList.Count; j++)
        {
            if (cardUISettingDataList[j].applicableBorderList.Contains(cardBorderType))
            {
                return cardUISettingDataList[j];
            }
        }
        return cardUISettingDataList[0];
    }
}
