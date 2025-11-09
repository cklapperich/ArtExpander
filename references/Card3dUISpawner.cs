using System.Collections.Generic;
using UnityEngine;

public class Card3dUISpawner : CSingleton<Card3dUISpawner>
{
    public static Card3dUISpawner m_Instance;

    public Card3dUIGroup m_Card3dUIPrefab;

    public List<Material> m_FoilMaterialTangentView;

    public List<Material> m_FoilMaterialWorldView;

    public List<Material> m_FoilBlendedMaterialTangentView;

    public List<Material> m_FoilBlendedMaterialWorldView;

    private List<Card3dUIGroup> m_Card3dUIList = new List<Card3dUIGroup>();

    private List<Card3dUIGroup> m_AllCard3dUIList = new List<Card3dUIGroup>();

    private int m_SpawnedCardCount;

    private int m_CullIndex;

    private int m_CullLoopCount;

    private int m_CullLoopCountMaxPerFrame = 50;

    private float m_DotCullLimit = 0.65f;

    private float m_AngleCullLimit = 110f;

    private float m_SimplifyCardDistance = 2f;

    private float m_CullTimer;

    private void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this;
        }
        else if (m_Instance != this)
        {
            Object.Destroy(base.gameObject);
        }
        Object.DontDestroyOnLoad(this);
        m_Card3dUIList = new List<Card3dUIGroup>();
        for (int i = 0; i < base.transform.childCount; i++)
        {
            m_Card3dUIList.Add(base.transform.GetChild(i).gameObject.GetComponent<Card3dUIGroup>());
        }
        for (int j = 0; j < 30; j++)
        {
            AddCardPrefab();
        }
        for (int k = 0; k < m_Card3dUIList.Count; k++)
        {
            m_Card3dUIList[k].gameObject.SetActive(value: false);
        }
        UpdateSimplifyCardDistance();
    }

    private void Update()
    {
        m_CullLoopCount = 0;
        for (int i = 0; i < m_Card3dUIList.Count; i++)
        {
            if ((bool)m_Card3dUIList[m_CullIndex] && m_Card3dUIList[m_CullIndex].GetAlwaysCulling())
            {
                m_Card3dUIList[m_CullIndex].m_CardUIAnimGrp.gameObject.SetActive(value: false);
            }
            else if ((bool)m_Card3dUIList[m_CullIndex] && !m_Card3dUIList[m_CullIndex].m_IgnoreCulling)
            {
                float num = Vector3.Dot((m_Card3dUIList[m_CullIndex].m_ScaleGrp.position - CSingleton<InteractionPlayerController>.Instance.m_Cam.transform.position).normalized, CSingleton<InteractionPlayerController>.Instance.m_Cam.transform.forward);
                float magnitude = (m_Card3dUIList[m_CullIndex].m_ScaleGrp.position - CSingleton<InteractionPlayerController>.Instance.m_Cam.transform.position).magnitude;
                float num2 = Vector3.Angle(m_Card3dUIList[m_CullIndex].m_ScaleGrp.TransformDirection(Vector3.forward), CSingleton<InteractionPlayerController>.Instance.m_Cam.transform.TransformDirection(Vector3.forward));
                if (magnitude > m_SimplifyCardDistance)
                {
                    m_Card3dUIList[m_CullIndex].m_CardUI.SetFoilCullListVisibility(isActive: false);
                    m_Card3dUIList[m_CullIndex].m_CardUI.SetFarDistanceCull();
                }
                else
                {
                    m_Card3dUIList[m_CullIndex].m_CardUI.SetFoilCullListVisibility(isActive: true);
                    m_Card3dUIList[m_CullIndex].m_CardUI.ResetFarDistanceCull();
                }
                if (magnitude > 9f || (magnitude > 1f && num < m_DotCullLimit) || num2 > m_AngleCullLimit)
                {
                    m_Card3dUIList[m_CullIndex].m_CardUIAnimGrp.gameObject.SetActive(value: false);
                }
                else
                {
                    m_Card3dUIList[m_CullIndex].m_CardUIAnimGrp.gameObject.SetActive(value: true);
                }
            }
            m_CullIndex++;
            if (m_CullIndex >= m_Card3dUIList.Count)
            {
                m_CullIndex = 0;
            }
            m_CullLoopCount++;
            if (m_CullLoopCount >= m_CullLoopCountMaxPerFrame)
            {
                m_CullLoopCount = 0;
                break;
            }
        }
    }

    public Card3dUIGroup GetCardUI()
    {
        for (int i = 0; i < m_Card3dUIList.Count; i++)
        {
            if (!m_Card3dUIList[i].IsActive())
            {
                m_Card3dUIList[i].ActivateCard();
                m_Card3dUIList[i].gameObject.SetActive(value: true);
                return m_Card3dUIList[i];
            }
        }
        Card3dUIGroup card3dUIGroup = AddCardPrefab();
        card3dUIGroup.ActivateCard();
        card3dUIGroup.gameObject.SetActive(value: true);
        return card3dUIGroup;
    }

    private Card3dUIGroup AddCardPrefab()
    {
        Card3dUIGroup card3dUIGroup = Object.Instantiate(m_Card3dUIPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity, base.transform);
        card3dUIGroup.transform.localRotation = Quaternion.identity;
        card3dUIGroup.name = "Card3dUIGrp_" + m_SpawnedCardCount;
        card3dUIGroup.gameObject.SetActive(value: false);
        m_Card3dUIList.Add(card3dUIGroup);
        m_SpawnedCardCount++;
        return card3dUIGroup;
    }

    public static void DisableCard(Card3dUIGroup card3dUI)
    {
        card3dUI.transform.parent = CSingleton<Card3dUISpawner>.Instance.transform;
        card3dUI.transform.localPosition = Vector3.zero;
        card3dUI.transform.localRotation = Quaternion.identity;
        card3dUI.gameObject.SetActive(value: false);
    }

    public static void AddCardToManager(Card3dUIGroup card3dUI)
    {
        if (!CSingleton<Card3dUISpawner>.Instance.m_AllCard3dUIList.Contains(card3dUI))
        {
            CSingleton<Card3dUISpawner>.Instance.m_AllCard3dUIList.Add(card3dUI);
        }
    }

    public static void RemoveCardFromManager(Card3dUIGroup card3dUI)
    {
        if (CSingleton<Card3dUISpawner>.Instance.m_AllCard3dUIList.Contains(card3dUI))
        {
            CSingleton<Card3dUISpawner>.Instance.m_AllCard3dUIList.Remove(card3dUI);
        }
    }

    public static void SetAllCardUIBrightness(float brightness)
    {
        for (int i = 0; i < CSingleton<Card3dUISpawner>.Instance.m_AllCard3dUIList.Count; i++)
        {
            if ((bool)CSingleton<Card3dUISpawner>.Instance.m_AllCard3dUIList[i].m_CardUI)
            {
                CSingleton<Card3dUISpawner>.Instance.m_AllCard3dUIList[i].m_CardUI.SetBrightness(brightness);
            }
        }
    }

    protected virtual void OnEnable()
    {
        if (Application.isPlaying || Application.isMobilePlatform)
        {
            CEventManager.AddListener<CEventPlayer_OnSettingUpdated>(OnSettingUpdated);
        }
    }

    protected virtual void OnDisable()
    {
        if (Application.isPlaying || Application.isMobilePlatform)
        {
            CEventManager.RemoveListener<CEventPlayer_OnSettingUpdated>(OnSettingUpdated);
        }
    }

    protected void OnSettingUpdated(CEventPlayer_OnSettingUpdated evt)
    {
        m_DotCullLimit = Mathf.Lerp(0.75f, 0.15f, CSingleton<CGameManager>.Instance.m_CameraFOVSlider);
        m_AngleCullLimit = 110f + CSingleton<CGameManager>.Instance.m_CameraFOVSlider * 40f;
        UpdateSimplifyCardDistance();
    }

    private void UpdateSimplifyCardDistance()
    {
        if (CSingleton<CGameManager>.Instance.m_QualitySettingIndex == 0)
        {
            m_SimplifyCardDistance = 1.1f;
        }
        else if (CSingleton<CGameManager>.Instance.m_QualitySettingIndex == 1)
        {
            m_SimplifyCardDistance = 0.75f;
        }
        else if (CSingleton<CGameManager>.Instance.m_QualitySettingIndex == 2)
        {
            m_SimplifyCardDistance = 0f;
        }
    }
}
