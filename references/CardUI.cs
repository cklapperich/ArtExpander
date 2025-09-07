using System;
using System.Collections.Generic;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200000C RID: 12
public class CardUI : MonoBehaviour
{
	// Token: 0x06000053 RID: 83 RVA: 0x00005EED File Offset: 0x000040ED
	public void InitCard3dUIGroup(Card3dUIGroup card3dUIGroup)
	{
		this.m_Card3dUIGroup = card3dUIGroup;
	}

	// Token: 0x06000054 RID: 84 RVA: 0x00005EF6 File Offset: 0x000040F6
	public void SetAncientArtifactCardUI(EMonsterType ancientCardType)
	{
		this.m_NormalGrp.SetActive(false);
		this.m_FullArtGrp.SetActive(false);
		this.m_AncientArtifactImage.gameObject.SetActive(true);
		this.m_AncientArtifactImage.sprite = InventoryBase.GetAncientArtifactSprite(ancientCardType);
	}

	// Token: 0x06000055 RID: 85 RVA: 0x00005F34 File Offset: 0x00004134
	public void ShowFoilList(bool isActive)
	{
		if (isActive)
		{
			Color color = Color.white * 0.95f;
			color.a = 1f;
			for (int i = 0; i < this.m_FoilDarkenImageList.Count; i++)
			{
				this.m_FoilDarkenImageList[i].color = color;
			}
			if (this.m_FullArtBGImage)
			{
				this.m_FullArtBGImage.color = color;
			}
		}
		else
		{
			for (int j = 0; j < this.m_FoilDarkenImageList.Count; j++)
			{
				this.m_FoilDarkenImageList[j].color = Color.white;
			}
			if (this.m_FullArtBGImage)
			{
				this.m_FullArtBGImage.color = Color.white;
			}
		}
		for (int k = 0; k < this.m_FoilShowList.Count; k++)
		{
			this.m_FoilShowList[k].enabled = isActive;
		}
	}

	// Token: 0x06000056 RID: 86 RVA: 0x00006014 File Offset: 0x00004214
	public void ShowFoilBlendedList(bool isActive)
	{
		for (int i = 0; i < this.m_FoilBlendedShowList.Count; i++)
		{
			this.m_FoilBlendedShowList[i].enabled = isActive;
		}
	}

	// Token: 0x06000057 RID: 87 RVA: 0x0000604C File Offset: 0x0000424C
	public void SetFoilCullListVisibility(bool isActive)
	{
		if (!this.m_IsFarDistanceCulled && isActive)
		{
			return;
		}
		if (this.m_IsFarDistanceCulled && !isActive)
		{
			return;
		}
		for (int i = 0; i < this.m_FoilShowList.Count; i++)
		{
			this.m_FoilShowList[i].gameObject.SetActive(isActive);
		}
		for (int j = 0; j < this.m_FoilBlendedShowList.Count; j++)
		{
			this.m_FoilBlendedShowList[j].gameObject.SetActive(isActive);
		}
	}

	// Token: 0x06000058 RID: 88 RVA: 0x000060D0 File Offset: 0x000042D0
	public void SetFoilMaterialList(List<Material> mat)
	{
		for (int i = 0; i < this.m_FoilShowList.Count; i++)
		{
			this.m_FoilShowList[i].material = mat[i];
		}
		if (this.m_FullArtCard)
		{
			this.m_FullArtCard.SetFoilMaterialList(mat);
		}
		if (this.m_GhostCard)
		{
			this.m_GhostCard.SetFoilMaterialList(mat);
		}
	}

	// Token: 0x06000059 RID: 89 RVA: 0x00006140 File Offset: 0x00004340
	public void SetFoilBlendedMaterialList(List<Material> mat)
	{
		for (int i = 0; i < this.m_FoilBlendedShowList.Count; i++)
		{
			this.m_FoilBlendedShowList[i].material = mat[i];
		}
		if (this.m_FullArtCard)
		{
			this.m_FullArtCard.SetFoilBlendedMaterialList(mat);
		}
		if (this.m_GhostCard)
		{
			this.m_GhostCard.SetFoilBlendedMaterialList(mat);
		}
	}

	// Token: 0x0600005A RID: 90 RVA: 0x000061B0 File Offset: 0x000043B0
	public void ShowGradedCardCase(bool isShow)
	{
		if (this.m_CardData.cardGrade <= 0)
		{
			isShow = false;
		}
		this.m_GradedCardCaseGrp.SetActive(isShow);
		if (isShow)
		{
			this.m_GradeNumberText.text = this.m_CardData.cardGrade.ToString();
			this.m_GradeDescriptionText.text = GameInstance.GetCardGradeString(this.m_CardData.cardGrade);
			this.m_GradeNameText.text = this.m_MonsterNameText.text;
			this.m_GradeExpansionRarityText.text = LocalizationManager.GetTranslation(this.m_CardData.expansionType.ToString(), true, 0, true, false, null, null, true) + " " + CPlayerData.GetFullCardTypeName(this.m_CardData, false);
			this.m_CardFront.transform.localPosition = this.m_GradedCardFrontScaling.transform.localPosition;
			this.m_CardFront.transform.localScale = this.m_GradedCardFrontScaling.transform.localScale;
			return;
		}
		this.m_CardFront.transform.localPosition = Vector3.zero;
		this.m_CardFront.transform.localScale = Vector3.one;
	}

	// Token: 0x0600005B RID: 91 RVA: 0x000062DC File Offset: 0x000044DC
	public void SetGhostCardUI(MonsterData data, bool isBlackGhost)
	{
		this.m_GhostCard.m_MonsterNameText.text = data.GetName();
		this.m_GhostCard.m_Stat1Text.text = data.BaseStats.Strength.ToString();
		this.m_GhostCard.m_Stat2Text.text = data.BaseStats.Vitality.ToString();
		this.m_GhostCard.m_Stat3Text.text = data.BaseStats.Spirit.ToString();
		this.m_GhostCard.m_Stat4Text.text = data.BaseStats.Magic.ToString();
		this.m_GhostCard.m_FameText.text = CPlayerData.GetCardFameAmount(this.m_CardData).ToString();
		this.m_GhostCard.m_MonsterImage.sprite = data.GetIcon(ECardExpansionType.Ghost);
		this.m_GhostCard.m_MonsterMaskImage.sprite = this.m_GhostCard.m_MonsterImage.sprite;
		this.m_GhostCard.m_FoilGrp.SetActive(this.m_IsFoil);
		this.m_GhostCard.ShowFoilList(this.m_IsFoil);
		this.m_GhostCard.ShowFoilBlendedList(this.m_IsFoil);
		this.m_NormalGrp.SetActive(false);
		this.m_FullArtGrp.SetActive(false);
		this.m_SpecialCardGrp.SetActive(false);
		this.m_GhostCard.gameObject.SetActive(true);
		this.m_AncientArtifactImage.gameObject.SetActive(false);
		if (isBlackGhost)
		{
			this.m_GhostCard.m_NormalGrp.SetActive(false);
			this.m_GhostCard.m_FullArtGrp.SetActive(true);
			return;
		}
		this.m_GhostCard.m_NormalGrp.SetActive(true);
		this.m_GhostCard.m_FullArtGrp.SetActive(false);
	}

	// Token: 0x0600005C RID: 92 RVA: 0x000064A0 File Offset: 0x000046A0
	private void LoadStreamTextureCompleted(CEventPlayer_LoadStreamTextureCompleted evt)
	{
		if (evt.m_FileName == this.m_CardData.expansionType.ToString() + "_" + this.m_CardData.monsterType.ToString())
		{
			CEventManager.RemoveListener<CEventPlayer_LoadStreamTextureCompleted>(new CEventManager.EventDelegate<CEventPlayer_LoadStreamTextureCompleted>(this.LoadStreamTextureCompleted));
			if (evt.m_IsSuccess)
			{
				this.m_MonsterImage.sprite = this.m_MonsterData.GetIcon(this.m_CardData.expansionType);
			}
		}
	}

	// Token: 0x0600005D RID: 93 RVA: 0x0000652A File Offset: 0x0000472A
	private void OnDisable()
	{
		if (Application.isPlaying || Application.isMobilePlatform)
		{
			CEventManager.RemoveListener<CEventPlayer_LoadStreamTextureCompleted>(new CEventManager.EventDelegate<CEventPlayer_LoadStreamTextureCompleted>(this.LoadStreamTextureCompleted));
		}
	}

	// Token: 0x0600005E RID: 94 RVA: 0x0000654C File Offset: 0x0000474C
	public void SetCardUI(CardData cardData)
	{
		this.m_CardData = cardData;
		if (this.m_CardData.monsterType <= EMonsterType.EarlyPlayer)
		{
			this.m_SpecialCardImage.sprite = InventoryBase.GetSpecialCardImage(this.m_CardData.monsterType);
			this.m_SpecialCardGlowImage.sprite = this.m_SpecialCardImage.sprite;
			this.m_SpecialCardGrp.SetActive(true);
			this.m_NormalGrp.SetActive(false);
			this.m_FullArtGrp.SetActive(false);
			this.m_GhostCard.gameObject.SetActive(false);
			this.m_AncientArtifactImage.gameObject.SetActive(false);
			this.m_FoilGrp.SetActive(true);
			this.ShowFoilList(true);
			this.ShowFoilBlendedList(true);
			return;
		}
		this.m_MonsterData = InventoryBase.GetMonsterData(cardData.monsterType);
		this.m_CardUISetting = InventoryBase.GetCardUISetting(this.m_CardData.expansionType);
		this.m_MonsterImage.sprite = this.m_MonsterData.GetIcon(this.m_CardData.expansionType);
		if (this.m_MonsterImage.sprite == null)
		{
			this.m_MonsterImage.sprite = CSingleton<LoadStreamTexture>.Instance.m_LoadingSprite;
			CEventManager.AddListener<CEventPlayer_LoadStreamTextureCompleted>(new CEventManager.EventDelegate<CEventPlayer_LoadStreamTextureCompleted>(this.LoadStreamTextureCompleted));
		}
		this.m_CardBackImage.sprite = CSingleton<InventoryBase>.Instance.m_MonsterData_SO.GetCardBackSprite(this.m_CardData.expansionType);
		if (!this.m_IsNestedFullArt)
		{
			this.m_CardFoilMaskImage.sprite = CSingleton<InventoryBase>.Instance.m_MonsterData_SO.GetCardFoilMaskSprite(this.m_CardData.expansionType);
		}
		this.m_CardBorderType = this.m_CardData.GetCardBorderType();
		this.m_IsDimensionCard = this.m_CardData.isDestiny;
		this.m_IsFoil = this.m_CardData.isFoil;
		this.m_FoilGrp.SetActive(this.m_IsFoil);
		this.ShowFoilList(this.m_IsFoil);
		this.ShowFoilBlendedList(this.m_IsFoil);
		if (this.m_ExtraFoil)
		{
			if (this.m_CardBorderType == ECardBorderType.Base || this.m_CardBorderType == ECardBorderType.FirstEdition)
			{
				this.m_ExtraFoil.gameObject.SetActive(false);
			}
			else
			{
				this.m_ExtraFoil.gameObject.SetActive(true);
			}
		}
		this.m_IsChampionCard = this.m_CardData.isChampionCard;
		if (this.m_CardData.expansionType == ECardExpansionType.Destiny)
		{
			this.m_CardBGImage.sprite = CSingleton<InventoryBase>.Instance.m_MonsterData_SO.GetCardFrontSprite(EElementIndex.Destiny);
		}
		else if (this.m_IsChampionCard)
		{
			this.m_CardBGImage.sprite = CSingleton<InventoryBase>.Instance.m_MonsterData_SO.GetCardFrontSprite(EElementIndex.Champion);
			this.m_CardBorderType = ECardBorderType.FullArt;
		}
		else if (this.m_CardData.expansionType == ECardExpansionType.Ghost)
		{
			if (this.m_CardData.isDestiny)
			{
				this.m_CardBGImage.sprite = CSingleton<InventoryBase>.Instance.m_MonsterData_SO.GetCardFrontSprite(EElementIndex.GhostBlack);
			}
			else
			{
				this.m_CardBGImage.sprite = CSingleton<InventoryBase>.Instance.m_MonsterData_SO.GetCardFrontSprite(EElementIndex.GhostWhite);
			}
		}
		else
		{
			this.m_CardBGImage.sprite = CSingleton<InventoryBase>.Instance.m_MonsterData_SO.GetCardFrontSprite(this.m_MonsterData.ElementIndex);
		}
		this.m_CardBorderImage.sprite = this.m_CardBorderSpriteList[(int)this.m_CardBorderType];
		this.m_RarityImage.sprite = this.m_CardRaritySpriteList[(int)this.m_MonsterData.Rarity];
		this.m_MonsterNameText.text = this.m_MonsterData.GetName();
		int num = (int)((this.m_MonsterData.MonsterType - EMonsterType.PiggyA) * CPlayerData.GetCardAmountPerMonsterType(this.m_CardData.expansionType, true) + this.m_CardBorderType);
		num++;
		if (this.m_IsFoil)
		{
			num += 6;
		}
		string text;
		if (num < 10)
		{
			text = "00" + num.ToString();
		}
		else if (num < 100)
		{
			text = "0" + num.ToString();
		}
		else
		{
			text = num.ToString();
		}
		this.m_NumberText.text = text;
		if (this.m_IsChampionCard)
		{
			this.m_FameText.gameObject.SetActive(false);
			this.m_DescriptionText.gameObject.SetActive(false);
			this.m_ArtistText.gameObject.SetActive(false);
			this.m_ChampionText.enabled = true;
		}
		else
		{
			this.m_DescriptionText.text = this.m_MonsterData.GetDescription();
			this.m_DescriptionText.gameObject.SetActive(true);
			this.m_ArtistText.text = this.m_MonsterData.GetArtistName();
			this.m_ArtistText.gameObject.SetActive(true);
			if (this.m_MonsterData.PreviousEvolution == EMonsterType.None)
			{
				this.m_EvoBasicGrp.SetActive(true);
				this.m_EvoPreviousStageIcon.gameObject.SetActive(false);
				this.m_EvoPreviousStageNameText.gameObject.SetActive(false);
			}
			else
			{
				this.m_EvoBasicGrp.SetActive(false);
				MonsterData monsterData = InventoryBase.GetMonsterData(this.m_MonsterData.PreviousEvolution);
				this.m_EvoPreviousStageIcon.sprite = monsterData.GetIcon(this.m_CardData.expansionType);
				this.m_EvoPreviousStageNameText.text = monsterData.GetName();
				this.m_EvoPreviousStageNameText.gameObject.SetActive(true);
				this.m_EvoPreviousStageIcon.gameObject.SetActive(true);
			}
			this.m_ChampionText.enabled = false;
		}
		for (int i = 0; i < this.m_ChampionCardEnableObjectList.Count; i++)
		{
			this.m_ChampionCardEnableObjectList[i].SetActive(this.m_IsChampionCard);
		}
		this.m_RarityText.text = this.m_MonsterData.GetRarityName();
		this.m_Stat1Text.text = this.m_MonsterData.BaseStats.Strength.ToString();
		this.m_Stat2Text.text = this.m_MonsterData.BaseStats.Vitality.ToString();
		this.m_Stat3Text.text = this.m_MonsterData.BaseStats.Spirit.ToString();
		this.m_Stat4Text.text = this.m_MonsterData.BaseStats.Magic.ToString();
		this.EvaluateCardUISetting();
		if (this.m_IsNestedFullArt)
		{
			return;
		}
		if (this.m_CardBorderType == ECardBorderType.Base || this.m_CardBorderType == ECardBorderType.FullArt)
		{
			this.m_FirstEditionText.enabled = false;
		}
		else
		{
			if (this.m_CardBorderType == ECardBorderType.FirstEdition)
			{
				this.m_FirstEditionText.text = LocalizationManager.GetTranslation("1st Edition", true, 0, true, false, null, null, true);
			}
			else if (this.m_CardBorderType == ECardBorderType.Silver)
			{
				this.m_FirstEditionText.text = LocalizationManager.GetTranslation("Silver Edition", true, 0, true, false, null, null, true);
			}
			else if (this.m_CardBorderType == ECardBorderType.Gold)
			{
				this.m_FirstEditionText.text = LocalizationManager.GetTranslation("Gold Edition", true, 0, true, false, null, null, true);
			}
			else if (this.m_CardBorderType == ECardBorderType.EX)
			{
				this.m_FirstEditionText.text = "EX";
			}
			this.m_FirstEditionText.enabled = true;
		}
		if (this.m_CardData.expansionType == ECardExpansionType.Ghost && this.m_CardBorderType == ECardBorderType.FullArt)
		{
			this.SetGhostCardUI(this.m_MonsterData, this.m_CardData.isDestiny);
		}
		else if (this.m_CardBorderType == ECardBorderType.FullArt && this.m_FullArtCard)
		{
			this.m_FullArtCard.MarkAsNestedFullArt(true);
			this.m_FullArtCard.SetCardUI(this.m_CardData);
			this.m_FullArtCard.m_MonsterMaskImage.sprite = this.m_FullArtCard.m_MonsterImage.sprite;
			if (this.m_CardData.expansionType == ECardExpansionType.Destiny)
			{
				this.m_FullArtBGImage.sprite = CSingleton<InventoryBase>.Instance.m_MonsterData_SO.GetCardBGSprite(EElementIndex.Destiny);
			}
			else if (this.m_IsChampionCard)
			{
				this.m_FullArtBGImage.sprite = CSingleton<InventoryBase>.Instance.m_MonsterData_SO.GetCardBGSprite(EElementIndex.Champion);
			}
			else if (this.m_CardData.expansionType == ECardExpansionType.Ghost)
			{
				if (this.m_CardData.isDestiny)
				{
					this.m_FullArtBGImage.sprite = CSingleton<InventoryBase>.Instance.m_MonsterData_SO.GetCardBGSprite(EElementIndex.GhostBlack);
				}
				else
				{
					this.m_FullArtBGImage.sprite = CSingleton<InventoryBase>.Instance.m_MonsterData_SO.GetCardBGSprite(EElementIndex.GhostWhite);
				}
			}
			else
			{
				this.m_FullArtBGImage.sprite = CSingleton<InventoryBase>.Instance.m_MonsterData_SO.GetCardBGSprite(this.m_MonsterData.ElementIndex);
			}
			this.m_FullArtCard.m_FoilGrp.SetActive(this.m_IsFoil);
			this.m_FullArtCard.ShowFoilList(this.m_IsFoil);
			this.m_FullArtCard.ShowFoilBlendedList(this.m_IsFoil);
			this.m_FullArtCard.m_FameText.text = this.m_FameText.text;
			this.m_FullArtCard.m_DescriptionText.text = this.m_DescriptionText.text;
			this.m_FullArtCard.m_ArtistText.text = this.m_ArtistText.text;
			this.m_FullArtCard.m_ArtistText.gameObject.SetActive(this.m_ArtistText.gameObject.activeSelf);
			if (this.m_MonsterData.PreviousEvolution == EMonsterType.None)
			{
				this.m_FullArtCard.m_EvoBasicGrp.SetActive(true);
				this.m_FullArtCard.m_EvoPreviousStageNameText.gameObject.SetActive(false);
				this.m_FullArtCard.m_EvoPreviousStageIcon.gameObject.SetActive(false);
			}
			else
			{
				this.m_FullArtCard.m_EvoBasicGrp.SetActive(false);
				MonsterData monsterData2 = InventoryBase.GetMonsterData(this.m_MonsterData.PreviousEvolution);
				this.m_FullArtCard.m_EvoPreviousStageIcon.sprite = monsterData2.GetIcon(this.m_CardData.expansionType);
				this.m_FullArtCard.m_EvoPreviousStageNameText.text = monsterData2.GetName();
				this.m_FullArtCard.m_EvoPreviousStageNameText.gameObject.SetActive(true);
				this.m_FullArtCard.m_EvoPreviousStageIcon.gameObject.SetActive(true);
			}
			this.m_NormalGrp.SetActive(false);
			this.m_FullArtGrp.SetActive(true);
			this.m_SpecialCardGrp.SetActive(false);
			this.m_GhostCard.gameObject.SetActive(false);
			this.m_AncientArtifactImage.gameObject.SetActive(false);
		}
		else
		{
			this.m_NormalGrp.SetActive(true);
			this.m_FullArtGrp.SetActive(false);
			this.m_SpecialCardGrp.SetActive(false);
			this.m_GhostCard.gameObject.SetActive(false);
			this.m_AncientArtifactImage.gameObject.SetActive(false);
		}
		if (this.m_Card3dUIGroup)
		{
			this.m_Card3dUIGroup.EvaluateCardGrade(this.m_CardData);
		}
		this.m_GradedCardTextureImage.sprite = CSingleton<InventoryBase>.Instance.m_MonsterData_SO.GetGradedCardScratchTexture(this.m_CardData.cardGrade);
		this.ShowGradedCardCase(this.m_Show2DGradedCase);
	}

	// Token: 0x0600005F RID: 95 RVA: 0x00006FC8 File Offset: 0x000051C8
	private void EvaluateCardUISetting()
	{
		if (this.m_MonsterNameText)
		{
			if (this.m_IsNestedFullArt)
			{
				this.m_MonsterNameText.enabled = this.m_CardUISetting.showNameFullArt;
			}
			else
			{
				this.m_MonsterNameText.enabled = this.m_CardUISetting.showName;
			}
		}
		this.m_Stat1Text.enabled = this.m_CardUISetting.showStat1;
		this.m_Stat2Text.enabled = this.m_CardUISetting.showStat2;
		this.m_Stat3Text.enabled = this.m_CardUISetting.showStat3;
		this.m_Stat4Text.enabled = this.m_CardUISetting.showStat4;
		this.m_Stat1Text.transform.localPosition = this.m_CardUISetting.stat1PosOffset;
		this.m_Stat1Text.transform.localScale = Vector3.one + this.m_CardUISetting.stat1ScaleOffset;
		if (this.m_ArtworkImageLocalPos != Vector3.zero)
		{
			this.m_MonsterImage.transform.localPosition = this.m_ArtworkImageLocalPos;
		}
		this.m_ArtworkImageLocalPos = this.m_MonsterImage.transform.localPosition;
		if (!this.m_IsNestedFullArt)
		{
			if (!this.m_CardUISetting.showEdition && this.m_FirstEditionText.enabled)
			{
				this.m_FirstEditionText.enabled = false;
			}
			this.m_RarityImage.enabled = this.m_CardUISetting.showRarity;
			this.m_RarityText.enabled = this.m_CardUISetting.showRarity;
			this.m_NumberText.enabled = this.m_CardUISetting.showNumber;
			this.m_NumberText.transform.localPosition = this.m_CardUISetting.numberPosOffset;
			this.m_FirstEditionText.transform.localPosition = this.m_CardUISetting.editionPosOffset;
			this.m_MonsterMask.transform.localPosition = this.m_CardUISetting.monsterImagePosOffset;
			this.m_MonsterMask.transform.localScale = Vector3.one + this.m_CardUISetting.monsterImageScaleOffset;
			this.m_MonsterImage.transform.localPosition = this.m_ArtworkImageLocalPos + this.m_CardUISetting.artworkImagePosOffset;
			this.m_MonsterImage.transform.localScale = Vector3.one + -this.m_CardUISetting.monsterImageScaleOffset;
			this.m_MonsterMaskImage.transform.localPosition = this.m_MonsterImage.transform.localPosition;
			this.m_MonsterMaskImage.transform.localScale = this.m_MonsterImage.transform.localScale;
			this.m_MonsterGlowMask.transform.localPosition = this.m_CardUISetting.monsterImagePosOffset;
			this.m_MonsterGlowMask.transform.localScale = this.m_MonsterMask.transform.localScale;
			this.m_FameText.transform.localPosition = this.m_CardUISetting.famePosOffset;
			this.m_FameText.transform.localScale = Vector3.one + this.m_CardUISetting.fameScaleOffset;
			this.m_MonsterNameText.transform.localPosition = this.m_CardUISetting.namePosOffset;
			return;
		}
		this.m_Stat1Text.enabled = this.m_CardUISetting.fullArtShowStat1;
		this.m_MonsterMask.transform.localPosition = this.m_CardUISetting.fullArtMonsterImagePosOffset;
		this.m_MonsterMask.transform.localScale = Vector3.one + this.m_CardUISetting.fullArtMonsterImageScaleOffset;
		this.m_MonsterGlowMask.transform.localPosition = this.m_CardUISetting.fullArtMonsterImagePosOffset;
		this.m_MonsterGlowMask.transform.localScale = this.m_MonsterMask.transform.localScale;
		this.m_FullArtBGImageMask.transform.localScale = this.m_MonsterMask.transform.localScale;
		this.m_FullArtBGImageMask.transform.localScale = this.m_MonsterMask.transform.localScale + this.m_CardUISetting.fullArtBGMaskScaleOffset;
		this.m_MonsterMask.sprite = this.m_CardBGImage.sprite;
		this.m_MonsterImage.transform.localPosition = this.m_ArtworkImageLocalPos + this.m_CardUISetting.fullArtArtworkImagePosOffset;
		this.m_MonsterMaskImage.transform.localPosition = this.m_MonsterImage.transform.localPosition;
		this.m_MonsterMaskImage.transform.localScale = this.m_MonsterImage.transform.localScale;
		this.m_CardFoilMaskImage.sprite = this.m_CardBGImage.sprite;
		this.m_FullArtBGImageMask.sprite = this.m_CardBGImage.sprite;
		this.m_MonsterGlowMask.sprite = this.m_CardBGImage.sprite;
		this.m_FameText.transform.localPosition = this.m_CardUISetting.fullArtFamePosOffset;
		this.m_FameText.transform.localScale = Vector3.one + this.m_CardUISetting.fullArtFameScaleOffset;
		this.m_MonsterNameText.transform.localPosition = this.m_CardUISetting.fullArtNamePosOffset;
	}

	// Token: 0x06000060 RID: 96 RVA: 0x000074F8 File Offset: 0x000056F8
	private void MarkAsNestedFullArt(bool isNested)
	{
		this.m_IsNestedFullArt = isNested;
	}

	// Token: 0x06000061 RID: 97 RVA: 0x00007501 File Offset: 0x00005701
	public void SetIsUnlocked(bool isUnlocked)
	{
		this.m_CardBack.SetActive(!isUnlocked);
		if (this.m_CardFront)
		{
			this.m_CardFront.SetActive(isUnlocked);
		}
	}

	// Token: 0x06000062 RID: 98 RVA: 0x0000752B File Offset: 0x0000572B
	public CardData GetCardData()
	{
		return this.m_CardData;
	}

	// Token: 0x06000063 RID: 99 RVA: 0x00007534 File Offset: 0x00005734
	public void SetBrightness(float brightness)
	{
		Color color = this.m_BrightnessControl.color;
		color.a = (1f - brightness) * 0.95f;
		this.m_BrightnessControl.color = color;
	}

	// Token: 0x06000064 RID: 100 RVA: 0x00007570 File Offset: 0x00005770
	public void SetFarDistanceCull()
	{
		if (this.m_IsFarDistanceCulled)
		{
			return;
		}
		this.m_IsFarDistanceCulled = true;
		this.m_FarDistanceCullObjVisibilityList.Clear();
		for (int i = 0; i < this.m_FarDistanceCullObjList.Count; i++)
		{
			this.m_FarDistanceCullObjVisibilityList.Add(this.m_FarDistanceCullObjList[i].activeSelf);
			this.m_FarDistanceCullObjList[i].SetActive(false);
		}
	}

	// Token: 0x06000065 RID: 101 RVA: 0x000075DC File Offset: 0x000057DC
	public void ResetFarDistanceCull()
	{
		if (!this.m_IsFarDistanceCulled)
		{
			return;
		}
		this.m_IsFarDistanceCulled = false;
		for (int i = 0; i < this.m_FarDistanceCullObjVisibilityList.Count; i++)
		{
			this.m_FarDistanceCullObjList[i].SetActive(this.m_FarDistanceCullObjVisibilityList[i]);
		}
		this.m_FarDistanceCullObjVisibilityList.Clear();
	}

	// Token: 0x0400007B RID: 123
	public GameObject m_NormalGrp;

	// Token: 0x0400007C RID: 124
	public GameObject m_FullArtGrp;

	// Token: 0x0400007D RID: 125
	public GameObject m_SpecialCardGrp;

	// Token: 0x0400007E RID: 126
	public CardUI m_FullArtCard;

	// Token: 0x0400007F RID: 127
	public CardUI m_GhostCard;

	// Token: 0x04000080 RID: 128
	public GameObject m_CardFront;

	// Token: 0x04000081 RID: 129
	public GameObject m_CardBack;

	// Token: 0x04000082 RID: 130
	public GameObject m_FoilGrp;

	// Token: 0x04000083 RID: 131
	public GameObject m_GradedCardCaseGrp;

	// Token: 0x04000084 RID: 132
	public bool m_Show2DGradedCase;

	// Token: 0x04000085 RID: 133
	public Transform m_GradedCardFrontScaling;

	// Token: 0x04000086 RID: 134
	public List<Image> m_FoilShowList;

	// Token: 0x04000087 RID: 135
	public List<Image> m_FoilBlendedShowList;

	// Token: 0x04000088 RID: 136
	public List<Image> m_FoilDarkenImageList;

	// Token: 0x04000089 RID: 137
	public GameObject m_ExtraFoil;

	// Token: 0x0400008A RID: 138
	public Image m_CardBackImage;

	// Token: 0x0400008B RID: 139
	public Image m_MonsterImage;

	// Token: 0x0400008C RID: 140
	public Image m_MonsterMaskImage;

	// Token: 0x0400008D RID: 141
	public Image m_FullArtBGImage;

	// Token: 0x0400008E RID: 142
	public Image m_FullArtBGImageMask;

	// Token: 0x0400008F RID: 143
	public Image m_CardBorderImage;

	// Token: 0x04000090 RID: 144
	public Image m_CardBGImage;

	// Token: 0x04000091 RID: 145
	public Image m_CardFoilMaskImage;

	// Token: 0x04000092 RID: 146
	public Image m_MonsterMask;

	// Token: 0x04000093 RID: 147
	public Image m_MonsterGlowMask;

	// Token: 0x04000094 RID: 148
	public Image m_RarityImage;

	// Token: 0x04000095 RID: 149
	public Image m_AncientArtifactImage;

	// Token: 0x04000096 RID: 150
	public Image m_SpecialCardImage;

	// Token: 0x04000097 RID: 151
	public Image m_SpecialCardGlowImage;

	// Token: 0x04000098 RID: 152
	public Image m_BrightnessControl;

	// Token: 0x04000099 RID: 153
	public Image m_GradedCardTextureImage;

	// Token: 0x0400009A RID: 154
	public TextMeshProUGUI m_FirstEditionText;

	// Token: 0x0400009B RID: 155
	public TextMeshProUGUI m_MonsterNameText;

	// Token: 0x0400009C RID: 156
	public TextMeshProUGUI m_NumberText;

	// Token: 0x0400009D RID: 157
	public TextMeshProUGUI m_FameText;

	// Token: 0x0400009E RID: 158
	public TextMeshProUGUI m_DescriptionText;

	// Token: 0x0400009F RID: 159
	public TextMeshProUGUI m_ChampionText;

	// Token: 0x040000A0 RID: 160
	public TextMeshProUGUI m_RarityText;

	// Token: 0x040000A1 RID: 161
	public TextMeshProUGUI m_Stat1Text;

	// Token: 0x040000A2 RID: 162
	public TextMeshProUGUI m_Stat2Text;

	// Token: 0x040000A3 RID: 163
	public TextMeshProUGUI m_Stat3Text;

	// Token: 0x040000A4 RID: 164
	public TextMeshProUGUI m_Stat4Text;

	// Token: 0x040000A5 RID: 165
	public TextMeshProUGUI m_ArtistText;

	// Token: 0x040000A6 RID: 166
	public TextMeshProUGUI m_GradeNumberText;

	// Token: 0x040000A7 RID: 167
	public TextMeshProUGUI m_GradeDescriptionText;

	// Token: 0x040000A8 RID: 168
	public TextMeshProUGUI m_GradeNameText;

	// Token: 0x040000A9 RID: 169
	public TextMeshProUGUI m_GradeExpansionRarityText;

	// Token: 0x040000AA RID: 170
	public TextMeshProUGUI m_GradeSerialText;

	// Token: 0x040000AB RID: 171
	public List<Sprite> m_CardBorderSpriteList;

	// Token: 0x040000AC RID: 172
	public List<Sprite> m_CardBGSpriteList;

	// Token: 0x040000AD RID: 173
	public List<Sprite> m_CardRaritySpriteList;

	// Token: 0x040000AE RID: 174
	public List<Sprite> m_CardElementBGSpriteList;

	// Token: 0x040000AF RID: 175
	private CardData m_CardData;

	// Token: 0x040000B0 RID: 176
	private MonsterData m_MonsterData;

	// Token: 0x040000B1 RID: 177
	private ECardBorderType m_CardBorderType;

	// Token: 0x040000B2 RID: 178
	private bool m_IsNestedFullArt;

	// Token: 0x040000B3 RID: 179
	private bool m_IsFoil;

	// Token: 0x040000B4 RID: 180
	private bool m_IsDimensionCard;

	// Token: 0x040000B5 RID: 181
	private bool m_IsChampionCard;

	// Token: 0x040000B6 RID: 182
	public List<GameObject> m_ChampionCardEnableObjectList;

	// Token: 0x040000B7 RID: 183
	private CardUISetting m_CardUISetting;

	// Token: 0x040000B8 RID: 184
	private Vector3 m_ArtworkImageLocalPos;

	// Token: 0x040000B9 RID: 185
	private Card3dUIGroup m_Card3dUIGroup;

	// Token: 0x040000BA RID: 186
	public List<GameObject> m_FarDistanceCullObjList;

	// Token: 0x040000BB RID: 187
	private List<bool> m_FarDistanceCullObjVisibilityList = new List<bool>();

	// Token: 0x040000BC RID: 188
	private bool m_IsFarDistanceCulled;

	// Token: 0x040000BD RID: 189
	public GameObject m_EvoGrp;

	// Token: 0x040000BE RID: 190
	public GameObject m_EvoBasicGrp;

	// Token: 0x040000BF RID: 191
	public Image m_EvoPreviousStageIcon;

	// Token: 0x040000C0 RID: 192
	public TextMeshProUGUI m_EvoPreviousStageNameText;
}
