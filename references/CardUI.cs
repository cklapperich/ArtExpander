using System.Collections.Generic;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    public GameObject m_CardFront;

    public GameObject m_CardBack;

    public GameObject m_CenterFrameMaskGrp;

    public GameObject m_CenterFrameImageGrp;

    public GameObject m_FoilGrp;

    public GameObject m_CenterFoilGlitter;

    public GameObject m_BorderFoilGlitter;

    public GameObject m_GradedCardCaseGrp;

    public bool m_Show2DGradedCase;

    public Transform m_GradedCardFrontScaling;

    public List<Image> m_FoilShowList;

    public List<Image> m_FoilBlendedShowList;

    public List<Image> m_FoilDarkenImageList;

    public Image m_CardBackImage;

    public Image m_CenterFrameImage;

    public Image m_CenterFrameMaskImage;

    public Image m_CardFrontImage;

    public Image m_FadeBarTopImage;

    public Image m_FadeBarBtmImage;

    public Image m_EvoBGImage;

    public Image m_PlayEffectBGImage;

    public Image m_DescriptionBGImage;

    public Image m_CardBorderMask;

    public Image m_CardBorderImage;

    public Image m_CardBGImage;

    public Image m_RarityImage;

    public Image m_StatImage;

    public Image m_BrightnessControl;

    public Image m_GradedCardTextureImage;

    public TextMeshProUGUI m_FirstEditionText;

    public TextMeshProUGUI m_MonsterNameText;

    public TextMeshProUGUI m_NumberText;

    public TextMeshProUGUI m_DescriptionText;

    public TextMeshProUGUI m_RarityText;

    public TextMeshProUGUI m_Stat1Text;

    public TextMeshProUGUI m_Stat2Text;

    public TextMeshProUGUI m_Stat3Text;

    public TextMeshProUGUI m_Stat4Text;

    public TextMeshProUGUI m_ArtistText;

    public TextMeshProUGUI m_GradeNumberText;

    public TextMeshProUGUI m_GradeDescriptionText;

    public TextMeshProUGUI m_GradeNameText;

    public TextMeshProUGUI m_GradeExpansionRarityText;

    public TextMeshProUGUI m_GradeSerialText;

    private CardData m_CardData;

    private MonsterData m_MonsterData;

    private ECardBorderType m_CardBorderType;

    private bool m_IsFoil;

    private bool m_IsDimensionCard;

    private CardUISetting m_CardUISetting;

    private CardUISettingData m_CardUISettingData;

    private Vector3 m_ArtworkImageLocalPos;

    private Card3dUIGroup m_Card3dUIGroup;

    public List<GameObject> m_FarDistanceCullObjList;

    public List<bool> m_FarDistanceCullObjVisibilityList = new List<bool>();

    public bool m_IsFarDistanceCulled;

    public GameObject m_StatGrp;

    public GameObject m_EvoAndArtistNameGrp;

    public GameObject m_EvoGrp;

    public GameObject m_EvoBasicGrp;

    public GameObject m_ArtistGrp;

    public GameObject m_DescriptionGrp;

    public Image m_EvoPreviousStageIcon;

    public TextMeshProUGUI m_EvoPreviousStageNameText;

    public void InitCard3dUIGroup(Card3dUIGroup card3dUIGroup)
    {
        m_Card3dUIGroup = card3dUIGroup;
    }

    public void ShowFoilList(bool isActive)
    {
        if (isActive)
        {
            for (int i = 0; i < m_FoilDarkenImageList.Count; i++)
            {
                m_FoilDarkenImageList[i].color = m_CardUISettingData.cardBGColorFoil;
            }
        }
        else
        {
            for (int j = 0; j < m_FoilDarkenImageList.Count; j++)
            {
                m_FoilDarkenImageList[j].color = m_CardUISettingData.cardBGColorNonFoil;
            }
        }
        for (int k = 0; k < m_FoilShowList.Count; k++)
        {
            m_FoilShowList[k].enabled = isActive;
        }
    }

    public void ShowFoilBlendedList(bool isActive)
    {
        for (int i = 0; i < m_FoilBlendedShowList.Count; i++)
        {
            m_FoilBlendedShowList[i].enabled = isActive;
        }
    }

    public void SetFoilCullListVisibility(bool isActive)
    {
        if (!(!m_IsFarDistanceCulled && isActive) && (!m_IsFarDistanceCulled || isActive))
        {
            for (int i = 0; i < m_FoilShowList.Count; i++)
            {
                m_FoilShowList[i].gameObject.SetActive(isActive);
            }
            for (int j = 0; j < m_FoilBlendedShowList.Count; j++)
            {
                m_FoilBlendedShowList[j].gameObject.SetActive(isActive);
            }
        }
    }

    public void SetFoilMaterialList(List<Material> mat)
    {
        for (int i = 0; i < m_FoilShowList.Count; i++)
        {
            m_FoilShowList[i].material = mat[i];
        }
    }

    public void SetFoilBlendedMaterialList(List<Material> mat)
    {
        for (int i = 0; i < m_FoilBlendedShowList.Count; i++)
        {
            m_FoilBlendedShowList[i].material = mat[i];
        }
    }

    public void ShowGradedCardCase(bool isShow)
    {
        if (m_CardData.cardGrade <= 0)
        {
            isShow = false;
        }
        m_GradedCardCaseGrp.SetActive(isShow);
        if (isShow)
        {
            m_GradeNumberText.text = m_CardData.cardGrade.ToString();
            m_GradeDescriptionText.text = GameInstance.GetCardGradeString(m_CardData.cardGrade);
            m_GradeNameText.text = m_MonsterNameText.text;
            m_GradeExpansionRarityText.text = LocalizationManager.GetTranslation(m_CardData.expansionType.ToString()) + " " + CPlayerData.GetFullCardTypeName(m_CardData);
            m_CardFront.transform.localPosition = m_GradedCardFrontScaling.transform.localPosition;
            m_CardFront.transform.localScale = m_GradedCardFrontScaling.transform.localScale;
        }
        else
        {
            m_CardFront.transform.localPosition = Vector3.zero;
            m_CardFront.transform.localScale = Vector3.one;
        }
    }

    private void LoadStreamTextureCompleted(CEventPlayer_LoadStreamTextureCompleted evt)
    {
        if (evt.m_FileName == m_CardData.expansionType.ToString() + "_" + m_CardData.monsterType)
        {
            CEventManager.RemoveListener<CEventPlayer_LoadStreamTextureCompleted>(LoadStreamTextureCompleted);
            if (evt.m_IsSuccess)
            {
                m_CenterFrameImage.sprite = m_MonsterData.GetIcon(m_CardData.expansionType);
            }
        }
    }

    private void OnDisable()
    {
        if (Application.isPlaying || Application.isMobilePlatform)
        {
            CEventManager.RemoveListener<CEventPlayer_LoadStreamTextureCompleted>(LoadStreamTextureCompleted);
        }
    }

    public void SetCardUI(CardData cardData)
    {
        m_CardData = cardData;
        m_MonsterData = InventoryBase.GetMonsterData(cardData.monsterType);
        m_CardUISetting = InventoryBase.GetCardUISetting(m_CardData.expansionType);
        m_CardUISettingData = m_CardUISetting.GetCardUISettingData(m_CardData.GetCardBorderType(), m_CardData.isDestiny);
        m_CenterFrameImage.sprite = m_MonsterData.GetIcon(m_CardData.expansionType);
        m_CardBackImage.sprite = CSingleton<InventoryBase>.Instance.m_MonsterData_SO.GetCardBackSprite(m_CardData.expansionType);
        m_CardBorderType = m_CardData.GetCardBorderType();
        m_IsDimensionCard = m_CardData.isDestiny;
        m_IsFoil = m_CardData.isFoil;
        m_FoilGrp.SetActive(m_IsFoil);
        ShowFoilList(m_IsFoil);
        ShowFoilBlendedList(m_IsFoil);
        m_CardBGImage.sprite = m_CardUISettingData.GetCardBGSprite(m_MonsterData.ElementIndex);
        m_CardBorderImage.sprite = m_CardUISettingData.GetCardBorderSprite(m_CardBorderType);
        m_RarityImage.sprite = m_CardUISettingData.GetCardRaritySprite(m_MonsterData.Rarity);
        m_CardFrontImage.enabled = m_CardUISettingData.showCardFront;
        m_FadeBarTopImage.enabled = m_CardUISettingData.showFadeBarTop;
        m_FadeBarBtmImage.enabled = m_CardUISettingData.showFadeBarBtm;
        m_EvoAndArtistNameGrp.SetActive(m_CardUISettingData.showEvoAndArtistNameGrp);
        m_CardFrontImage.sprite = m_CardUISettingData.GetCardFrontSprite(m_MonsterData.ElementIndex);
        m_CardFrontImage.transform.localPosition = m_CardUISettingData.cardFrontImagePosOffset;
        m_CardFrontImage.transform.localScale = Vector3.one + m_CardUISettingData.cardFrontImageScaleOffset;
        m_CenterFrameMaskImage.sprite = m_CardUISettingData.cardCenterFrameMask;
        m_EvoAndArtistNameGrp.transform.localPosition = m_CardUISettingData.evoAndArtistNameGrpPosOffset;
        m_EvoAndArtistNameGrp.transform.localScale = Vector3.one + m_CardUISettingData.evoAndArtistNameGrpScaleOffset;
        m_EvoGrp.transform.localPosition = m_CardUISettingData.evoGrpPosOffset;
        m_EvoGrp.transform.localScale = Vector3.one + m_CardUISettingData.evoGrpScaleOffset;
        m_ArtistGrp.transform.localPosition = m_CardUISettingData.artistNameGrpPosOffset;
        m_ArtistGrp.transform.localScale = Vector3.one + m_CardUISettingData.artistNameGrpScaleOffset;
        m_DescriptionGrp.transform.localPosition = m_CardUISettingData.descriptionGrpPosOffset;
        m_DescriptionGrp.transform.localScale = Vector3.one + m_CardUISettingData.descriptionGrpScaleOffset;
        m_StatImage.sprite = m_CardUISettingData.statImage;
        m_StatGrp.transform.localPosition = m_CardUISettingData.statGrpPosOffset;
        m_StatGrp.transform.localScale = Vector3.one + m_CardUISettingData.statGrpScaleOffset;
        m_CenterFrameMaskGrp.transform.localPosition = m_CardUISettingData.centerFrameMaskPosOffset;
        m_CenterFrameMaskGrp.transform.localScale = Vector3.one + m_CardUISettingData.centerFrameMaskScaleOffset;
        m_CenterFrameImageGrp.transform.localPosition = m_CardUISettingData.centerImageGrpPosOffset;
        m_CenterFrameImageGrp.transform.localScale = Vector3.one + m_CardUISettingData.centerImageGrpScaleOffset;
        m_EvoBGImage.color = m_CardUISettingData.evoBGColor;
        m_PlayEffectBGImage.color = m_CardUISettingData.playEffectBGColor;
        m_DescriptionBGImage.color = m_CardUISettingData.descriptionBGColor;
        m_CardBorderMask.sprite = m_CardUISettingData.cardBorderMask;
        m_CardBorderMask.gameObject.SetActive(m_CardUISettingData.showBorder);
        m_CenterFoilGlitter.SetActive(m_CardUISettingData.showCenterFoilGlitter && m_IsFoil);
        m_BorderFoilGlitter.SetActive(m_CardUISettingData.showBorderFoilGlitter && m_IsFoil);
        m_MonsterNameText.text = m_MonsterData.GetName();
        int num = (int)((int)(m_MonsterData.MonsterType - 1) * CPlayerData.GetCardAmountPerMonsterType(m_CardData.expansionType) + m_CardBorderType);
        num++;
        if (m_IsFoil)
        {
            num += 6;
        }
        string text = "";
        text = ((num < 10) ? ("00" + num) : ((num >= 100) ? num.ToString() : ("0" + num)));
        m_NumberText.text = text;
        m_DescriptionText.text = m_MonsterData.GetDescription();
        m_DescriptionText.gameObject.SetActive(value: true);
        m_ArtistText.text = m_MonsterData.GetArtistName();
        m_ArtistText.gameObject.SetActive(value: true);
        if (m_MonsterData.PreviousEvolution == EMonsterType.None)
        {
            m_EvoBasicGrp.SetActive(value: true);
            m_EvoPreviousStageIcon.gameObject.SetActive(value: false);
            m_EvoPreviousStageNameText.gameObject.SetActive(value: false);
        }
        else
        {
            m_EvoBasicGrp.SetActive(value: false);
            MonsterData monsterData = InventoryBase.GetMonsterData(m_MonsterData.PreviousEvolution);
            if (m_CardData.expansionType == ECardExpansionType.Ghost)
            {
                m_EvoPreviousStageIcon.sprite = monsterData.GetIcon(ECardExpansionType.Tetramon);
            }
            else
            {
                m_EvoPreviousStageIcon.sprite = monsterData.GetIcon(m_CardData.expansionType);
            }
            m_EvoPreviousStageNameText.text = monsterData.GetName();
            m_EvoPreviousStageNameText.gameObject.SetActive(value: true);
            m_EvoPreviousStageIcon.gameObject.SetActive(value: true);
        }
        m_RarityText.text = m_MonsterData.GetRarityName();
        m_Stat1Text.text = m_MonsterData.BaseStats.Strength.ToString();
        m_Stat2Text.text = m_MonsterData.BaseStats.Vitality.ToString();
        m_Stat3Text.text = m_MonsterData.BaseStats.Spirit.ToString();
        m_Stat4Text.text = m_MonsterData.BaseStats.Magic.ToString();
        EvaluateCardUISetting();
        if (m_CardBorderType == ECardBorderType.Base || m_CardBorderType == ECardBorderType.FullArt)
        {
            m_FirstEditionText.enabled = false;
        }
        else
        {
            if (m_CardBorderType == ECardBorderType.FirstEdition)
            {
                m_FirstEditionText.text = LocalizationManager.GetTranslation("1st Edition");
            }
            else if (m_CardBorderType == ECardBorderType.Silver)
            {
                m_FirstEditionText.text = LocalizationManager.GetTranslation("Silver Edition");
            }
            else if (m_CardBorderType == ECardBorderType.Gold)
            {
                m_FirstEditionText.text = LocalizationManager.GetTranslation("Gold Edition");
            }
            else if (m_CardBorderType == ECardBorderType.EX)
            {
                m_FirstEditionText.text = "EX";
            }
            m_FirstEditionText.enabled = true;
        }
        if ((bool)m_Card3dUIGroup)
        {
            m_Card3dUIGroup.EvaluateCardGrade(m_CardData);
        }
        m_GradedCardTextureImage.sprite = CSingleton<InventoryBase>.Instance.m_MonsterData_SO.GetGradedCardScratchTexture(m_CardData.cardGrade);
        ShowGradedCardCase(m_Show2DGradedCase);
    }

    private void EvaluateCardUISetting()
    {
        if ((bool)m_MonsterNameText)
        {
            m_MonsterNameText.enabled = m_CardUISettingData.showName;
        }
        m_Stat1Text.enabled = m_CardUISettingData.showStat1;
        m_Stat2Text.enabled = m_CardUISettingData.showStat2;
        m_Stat3Text.enabled = m_CardUISettingData.showStat3;
        m_Stat4Text.enabled = m_CardUISettingData.showStat4;
        m_Stat1Text.transform.localPosition = m_CardUISettingData.stat1PosOffset;
        m_Stat1Text.transform.localScale = Vector3.one + m_CardUISettingData.stat1ScaleOffset;
        m_Stat2Text.transform.localPosition = m_CardUISettingData.stat2PosOffset;
        m_Stat2Text.transform.localScale = Vector3.one + m_CardUISettingData.stat2ScaleOffset;
        m_Stat3Text.transform.localPosition = m_CardUISettingData.stat3PosOffset;
        m_Stat3Text.transform.localScale = Vector3.one + m_CardUISettingData.stat3ScaleOffset;
        m_Stat4Text.transform.localPosition = m_CardUISettingData.stat4PosOffset;
        m_Stat4Text.transform.localScale = Vector3.one + m_CardUISettingData.stat4ScaleOffset;
        if (m_ArtworkImageLocalPos != Vector3.zero)
        {
            m_CenterFrameImage.transform.localPosition = m_ArtworkImageLocalPos;
        }
        m_ArtworkImageLocalPos = m_CenterFrameImage.transform.localPosition;
        if (!m_CardUISettingData.showEdition && m_FirstEditionText.enabled)
        {
            m_FirstEditionText.enabled = false;
        }
        m_RarityImage.enabled = m_CardUISettingData.showRarity;
        m_RarityText.enabled = m_CardUISettingData.showRarity;
        m_NumberText.enabled = m_CardUISettingData.showNumber;
        m_NumberText.transform.localPosition = m_CardUISettingData.numberPosOffset;
        m_FirstEditionText.transform.localPosition = m_CardUISettingData.editionPosOffset;
        m_MonsterNameText.transform.localPosition = m_CardUISettingData.namePosOffset;
    }

    public CardData GetCardData()
    {
        return m_CardData;
    }

    public void SetBrightness(float brightness)
    {
        Color color = m_BrightnessControl.color;
        color.a = (1f - brightness) * 0.95f;
        m_BrightnessControl.color = color;
    }

    public void SetFarDistanceCull()
    {
        if (!m_IsFarDistanceCulled)
        {
            m_IsFarDistanceCulled = true;
            m_FarDistanceCullObjVisibilityList.Clear();
            for (int i = 0; i < m_FarDistanceCullObjList.Count; i++)
            {
                m_FarDistanceCullObjVisibilityList.Add(m_FarDistanceCullObjList[i].activeSelf);
                m_FarDistanceCullObjList[i].SetActive(value: false);
            }
            _ = (bool)m_Card3dUIGroup;
        }
    }

    public void ResetFarDistanceCull()
    {
        if (m_IsFarDistanceCulled)
        {
            m_IsFarDistanceCulled = false;
            for (int i = 0; i < m_FarDistanceCullObjVisibilityList.Count; i++)
            {
                m_FarDistanceCullObjList[i].SetActive(m_FarDistanceCullObjVisibilityList[i]);
            }
            m_FarDistanceCullObjVisibilityList.Clear();
            _ = (bool)m_Card3dUIGroup;
        }
    }

    public bool IsCard3dUIGroupSet()
    {
        return m_Card3dUIGroup != null;
    }
}
