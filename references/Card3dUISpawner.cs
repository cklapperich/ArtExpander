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

    private float m_SimplifyCardDistance = 2f;

    private float m_CullTimer;

    private void Awake()
    {
        if ((Object)(object)m_Instance == (Object)null)
        {
            m_Instance = this;
        }
        else if ((Object)(object)m_Instance != (Object)(object)this)
        {
            Object.Destroy((Object)(object)((Component)this).gameObject);
        }
        Object.DontDestroyOnLoad((Object)(object)this);
        m_Card3dUIList = new List<Card3dUIGroup>();
        for (int i = 0; i < ((Component)this).transform.childCount; i++)
        {
            m_Card3dUIList.Add(((Component)((Component)this).transform.GetChild(i)).gameObject.GetComponent<Card3dUIGroup>());
        }
        for (int j = 0; j < 30; j++)
        {
            AddCardPrefab();
        }
        for (int k = 0; k < m_Card3dUIList.Count; k++)
        {
            ((Component)m_Card3dUIList[k]).gameObject.SetActive(false);
        }
        UpdateSimplifyCardDistance();
    }

    private void Update()
    {
        //IL_005a: Unknown result type (might be due to invalid IL or missing references)
        //IL_006e: Unknown result type (might be due to invalid IL or missing references)
        //IL_0073: Unknown result type (might be due to invalid IL or missing references)
        //IL_0078: Unknown result type (might be due to invalid IL or missing references)
        //IL_007b: Unknown result type (might be due to invalid IL or missing references)
        //IL_008f: Unknown result type (might be due to invalid IL or missing references)
        //IL_00b0: Unknown result type (might be due to invalid IL or missing references)
        //IL_00c4: Unknown result type (might be due to invalid IL or missing references)
        //IL_00c9: Unknown result type (might be due to invalid IL or missing references)
        //IL_00ce: Unknown result type (might be due to invalid IL or missing references)
        //IL_00ee: Unknown result type (might be due to invalid IL or missing references)
        //IL_00f3: Unknown result type (might be due to invalid IL or missing references)
        //IL_0107: Unknown result type (might be due to invalid IL or missing references)
        //IL_010c: Unknown result type (might be due to invalid IL or missing references)
        m_CullLoopCount = 0;
        for (int i = 0; i < m_Card3dUIList.Count; i++)
        {
            if (Object.op_Implicit((Object)(object)m_Card3dUIList[m_CullIndex]) && !m_Card3dUIList[m_CullIndex].m_IgnoreCulling)
            {
                Vector3 val = m_Card3dUIList[m_CullIndex].m_ScaleGrp.position - ((Component)CSingleton<InteractionPlayerController>.Instance.m_Cam).transform.position;
                float num = Vector3.Dot(((Vector3)(ref val)).normalized, ((Component)CSingleton<InteractionPlayerController>.Instance.m_Cam).transform.forward);
                Vector3 val2 = m_Card3dUIList[m_CullIndex].m_ScaleGrp.position - ((Component)CSingleton<InteractionPlayerController>.Instance.m_WalkerCtrl).transform.position;
                float magnitude = ((Vector3)(ref val2)).magnitude;
                float num2 = Vector3.Angle(m_Card3dUIList[m_CullIndex].m_ScaleGrp.TransformDirection(Vector3.forward), ((Component)CSingleton<InteractionPlayerController>.Instance.m_Cam).transform.TransformDirection(Vector3.forward));
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
                if (magnitude > 9f || (magnitude > 1f && num < m_DotCullLimit) || num2 > 110f)
                {
                    ((Component)m_Card3dUIList[m_CullIndex].m_CardUIAnimGrp).gameObject.SetActive(false);
                }
                else
                {
                    ((Component)m_Card3dUIList[m_CullIndex].m_CardUIAnimGrp).gameObject.SetActive(true);
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
                ((Component)m_Card3dUIList[i]).gameObject.SetActive(true);
                return m_Card3dUIList[i];
            }
        }
        Card3dUIGroup card3dUIGroup = AddCardPrefab();
        card3dUIGroup.ActivateCard();
        ((Component)card3dUIGroup).gameObject.SetActive(true);
        return card3dUIGroup;
    }

    private Card3dUIGroup AddCardPrefab()
    {
        //IL_0015: Unknown result type (might be due to invalid IL or missing references)
        //IL_001a: Unknown result type (might be due to invalid IL or missing references)
        //IL_0031: Unknown result type (might be due to invalid IL or missing references)
        Card3dUIGroup card3dUIGroup = Object.Instantiate<Card3dUIGroup>(m_Card3dUIPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity, ((Component)this).transform);
        ((Component)card3dUIGroup).transform.localRotation = Quaternion.identity;
        ((Object)card3dUIGroup).name = "Card3dUIGrp_" + m_SpawnedCardCount;
        ((Component)card3dUIGroup).gameObject.SetActive(false);
        m_Card3dUIList.Add(card3dUIGroup);
        m_SpawnedCardCount++;
        return card3dUIGroup;
    }

    public static void DisableCard(Card3dUIGroup card3dUI)
    {
        //IL_001b: Unknown result type (might be due to invalid IL or missing references)
        //IL_002b: Unknown result type (might be due to invalid IL or missing references)
        ((Component)card3dUI).transform.parent = ((Component)CSingleton<Card3dUISpawner>.Instance).transform;
        ((Component)card3dUI).transform.localPosition = Vector3.zero;
        ((Component)card3dUI).transform.localRotation = Quaternion.identity;
        ((Component)card3dUI).gameObject.SetActive(false);
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
            if (Object.op_Implicit((Object)(object)CSingleton<Card3dUISpawner>.Instance.m_AllCard3dUIList[i].m_CardUI))
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
        m_DotCullLimit = Mathf.Lerp(0.75f, 0.35f, CSingleton<CGameManager>.Instance.m_CameraFOVSlider);
        UpdateSimplifyCardDistance();
    }

    private void UpdateSimplifyCardDistance()
    {
        if (CSingleton<CGameManager>.Instance.m_QualitySettingIndex == 0)
        {
            m_SimplifyCardDistance = 2.3f;
        }
        else if (CSingleton<CGameManager>.Instance.m_QualitySettingIndex == 1)
        {
            m_SimplifyCardDistance = 1.5f;
        }
        else if (CSingleton<CGameManager>.Instance.m_QualitySettingIndex == 2)
        {
            m_SimplifyCardDistance = 0f;
        }
    }
}
