using System;

[Serializable]
public class CardData
{
    public ECardExpansionType expansionType;

    public EMonsterType monsterType;

    public ECardBorderType borderType;

    public bool isFoil;

    public bool isDestiny;

    public bool isChampionCard;

    public bool isNew;

    public int cardGrade;

    public int gradedCardIndex;

    public ECardBorderType GetCardBorderType()
    {
        return CPlayerData.GetCardBorderType((int)borderType, expansionType);
    }
}
   