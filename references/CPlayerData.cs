using System;
using System.Collections.Generic;
using I2.Loc;
using UnityEngine;

public class CPlayerData : CSingleton<CPlayerData>
{
    public static CPlayerData m_Instance = null;

    public static ELevelName m_CurrentLevelName = ELevelName.Start;

    public static int m_SaveIndex = 0;

    public static int m_SaveCycle = 0;

    public static string m_LastLoginPlayfabID = "";

    public static bool m_CanCloudLoad = true;

    public static float m_SkyboxBlendValue = 0f;

    public static LightTimeData m_LightTimeData;

    public static EGameEventFormat m_GameEventFormat = EGameEventFormat.Standard;

    public static EGameEventFormat m_PendingGameEventFormat = EGameEventFormat.None;

    public static ECardExpansionType m_GameEventExpansionType = ECardExpansionType.Tetramon;

    public static ECardExpansionType m_PendingGameEventExpansionType = ECardExpansionType.Tetramon;

    public static GameReportDataCollect m_GameReportDataCollectPermanent = default(GameReportDataCollect);

    public static GameReportDataCollect m_GameReportDataCollect = default(GameReportDataCollect);

    public static List<GameReportDataCollect> m_GameReportDataCollectPastList = new List<GameReportDataCollect>();

    public static List<ShelfSaveData> m_ShelfSaveDataList = new List<ShelfSaveData>();

    public static List<WarehouseShelfSaveData> m_WarehouseShelfSaveDataList = new List<WarehouseShelfSaveData>();

    public static List<PackageBoxItemaveData> m_PackageBoxItemSaveDataList = new List<PackageBoxItemaveData>();

    public static List<PackageBoxCardSaveData> m_PackageBoxCardSaveDataList = new List<PackageBoxCardSaveData>();

    public static List<InteractableObjectSaveData> m_InteractableObjectSaveDataList = new List<InteractableObjectSaveData>();

    public static List<DecoObjectSaveData> m_DecoObjectSaveDataList = new List<DecoObjectSaveData>();

    public static List<CardShelfSaveData> m_CardShelfSaveDataList = new List<CardShelfSaveData>();

    public static List<PlayTableSaveData> m_PlayTableSaveDataList = new List<PlayTableSaveData>();

    public static List<AutoCleanserSaveData> m_AutoCleanserSaveDataList = new List<AutoCleanserSaveData>();

    public static List<AutoPackOpenerSaveData> m_AutoPackOpenerSaveDataList = new List<AutoPackOpenerSaveData>();

    public static List<WorkbenchSaveData> m_WorkbenchSaveDataList = new List<WorkbenchSaveData>();

    public static List<CashierCounterSaveData> m_CashCounterSaveDataList = new List<CashierCounterSaveData>();

    public static List<CustomerSaveData> m_CustomerSaveDataList = new List<CustomerSaveData>();

    public static List<WorkerSaveData> m_WorkerSaveDataList = new List<WorkerSaveData>();

    public static List<TutorialData> m_TutorialDataList = new List<TutorialData>();

    public static List<CustomerReviewData> m_CustomerReviewDataList = new List<CustomerReviewData>();

    public static int m_CustomerReviewCount = 0;

    public static float m_CustomerReviewScoreAverage = 0f;

    public static List<BillData> m_BillList = new List<BillData>();

    public static List<CardData> m_HoldCardDataList = new List<CardData>();

    public static List<EItemType> m_HoldItemTypeList = new List<EItemType>();

    public static List<EItemType> m_TargetBuyItemList = new List<EItemType>();

    public static bool m_IsShopOpen = false;

    public static bool m_IsShopOnceOpen = false;

    public static bool m_IsWarehouseDoorClosed = false;

    public static bool m_IsItemPriceGenerated = false;

    public static bool m_IsCardPriceGenerated = false;

    public static List<int> m_CurrentTotalItemCountList = new List<int>();

    public static List<float> m_SetItemPriceList = new List<float>();

    public static List<float> m_AverageItemCostList = new List<float>();

    public static List<float> m_GeneratedCostPriceList = new List<float>();

    public static List<float> m_GeneratedMarketPriceList = new List<float>();

    public static List<float> m_ItemPricePercentChangeList = new List<float>();

    public static List<FloatList> m_ItemPricePercentPastChangeList = new List<FloatList>();

    public static List<float> m_SetGameEventPriceList = new List<float>();

    public static List<float> m_GeneratedGameEventPriceList = new List<float>();

    public static List<float> m_GameEventPricePercentChangeList = new List<float>();

    public static List<TransactionData> m_TransactionDataList = new List<TransactionData>();

    public static List<int> m_StockSoldList = new List<int>();

    public static List<int> m_CollectionCardPackCountList = new List<int>();

    public static List<int> m_SpawnBoxRestockIndexWaitingList = new List<int>();

    public static List<int> m_SpawnBoxItemCountWaitingList = new List<int>();

    public static List<int> m_CardCollectedList = new List<int>();

    public static List<int> m_CardCollectedListDestiny = new List<int>();

    public static List<int> m_CardCollectedListGhost = new List<int>();

    public static List<int> m_CardCollectedListGhostBlack = new List<int>();

    public static List<int> m_CardCollectedListMegabot = new List<int>();

    public static List<int> m_CardCollectedListFantasyRPG = new List<int>();

    public static List<int> m_CardCollectedListCatJob = new List<int>();

    public static List<CompactCardDataAmount> m_GradedCardInventoryList = new List<CompactCardDataAmount>();

    public static List<bool> m_IsCardCollectedList = new List<bool>();

    public static List<bool> m_IsCardCollectedListDestiny = new List<bool>();

    public static List<bool> m_IsCardCollectedListGhost = new List<bool>();

    public static List<bool> m_IsCardCollectedListGhostBlack = new List<bool>();

    public static List<bool> m_IsCardCollectedListMegabot = new List<bool>();

    public static List<bool> m_IsCardCollectedListFantasyRPG = new List<bool>();

    public static List<bool> m_IsCardCollectedListCatJob = new List<bool>();

    public static List<float> m_CardPriceSetList = new List<float>();

    public static List<float> m_CardPriceSetListDestiny = new List<float>();

    public static List<float> m_CardPriceSetListGhost = new List<float>();

    public static List<float> m_CardPriceSetListGhostBlack = new List<float>();

    public static List<float> m_CardPriceSetListMegabot = new List<float>();

    public static List<float> m_CardPriceSetListFantasyRPG = new List<float>();

    public static List<float> m_CardPriceSetListCatJob = new List<float>();

    public static List<FloatList> m_GradedCardPriceSetList = new List<FloatList>();

    public static List<FloatList> m_GradedCardPriceSetListDestiny = new List<FloatList>();

    public static List<FloatList> m_GradedCardPriceSetListGhost = new List<FloatList>();

    public static List<FloatList> m_GradedCardPriceSetListGhostBlack = new List<FloatList>();

    public static List<FloatList> m_GradedCardPriceSetListMegabot = new List<FloatList>();

    public static List<FloatList> m_GradedCardPriceSetListFantasyRPG = new List<FloatList>();

    public static List<FloatList> m_GradedCardPriceSetListCatJob = new List<FloatList>();

    public static List<MarketPrice> m_GenCardMarketPriceList = new List<MarketPrice>();

    public static List<MarketPrice> m_GenCardMarketPriceListDestiny = new List<MarketPrice>();

    public static List<MarketPrice> m_GenCardMarketPriceListGhost = new List<MarketPrice>();

    public static List<MarketPrice> m_GenCardMarketPriceListGhostBlack = new List<MarketPrice>();

    public static List<MarketPrice> m_GenCardMarketPriceListMegabot = new List<MarketPrice>();

    public static List<MarketPrice> m_GenCardMarketPriceListFantasyRPG = new List<MarketPrice>();

    public static List<MarketPrice> m_GenCardMarketPriceListCatJob = new List<MarketPrice>();

    public static List<float> m_GenGradedCardPriceMultiplierList = new List<float>();

    public static GradeCardSubmitSet m_CurrentGradeCardSubmitSet = null;

    public static List<GradeCardSubmitSet> m_GradeCardInProgressList = new List<GradeCardSubmitSet>();

    public static List<int> m_CollectionSortingMethodIndexList = new List<int>();

    public static List<int> m_ChampionCardCollectedList = new List<int>();

    public static List<bool> m_IsItemLicenseUnlocked = new List<bool>();

    public static List<bool> m_IsWorkerHired = new List<bool>();

    public static List<bool> m_IsAchievementUnlocked = new List<bool>();

    public static List<int> m_DecorationInventoryList = new List<int>();

    public static List<bool> m_UnlockedDecoWallList = new List<bool>();

    public static List<bool> m_UnlockedDecoFloorList = new List<bool>();

    public static List<bool> m_UnlockedDecoCeilingList = new List<bool>();

    public static bool m_IsScannerRestockUnlocked = false;

    public static bool m_IsWarehouseRoomUnlocked = false;

    public static int m_UnlockRoomCount = 0;

    public static int m_UnlockWarehouseRoomCount = 0;

    public static int m_TotalFameAdd = 0;

    public static int m_CurrentDay = 0;

    public static int m_ShopExpPoint = 0;

    public static int m_ShopLevel = 0;

    public static int m_EquippedWallDecoIndex = 0;

    public static int m_EquippedWallDecoIndexB = 0;

    public static int m_EquippedFloorDecoIndex = 0;

    public static int m_EquippedFloorDecoIndexB = 0;

    public static int m_EquippedCeilingDecoIndex = 0;

    public static int m_EquippedCeilingDecoIndexB = 0;

    public static int m_GradedCollectionSortingMethodIndex = 0;

    public static int m_WorkbenchMinimumCardLimit = 4;

    public static float m_WorkbenchPriceLimit = 0.5f;

    public static float m_WorkbenchPriceMinimum = 0.01f;

    public static ERarity m_WorkbenchRarityLimit = ERarity.None;

    public static ECardExpansionType m_WorkbenchCardExpansionType = ECardExpansionType.Tetramon;

    public static ERestockSortingType m_RestockSortingType = ERestockSortingType.Default;

    public static DateTime m_LastLocalExitTime;

    public static DateTime m_StarterOfferStartTime;

    public static int m_TotalCoinAmount = 0;

    public static string m_DebugString = "";

    public static string m_DebugString2 = "";

    private static string m_PlayerName;

    public static float m_CoinAmount = 0f;

    public static double m_CoinAmountDouble = 0.0;

    public static int m_FamePoint = 0;

    public static int m_CloudSaveCountdown = 0;

    public static int m_LastSelectedPriceGraphCardGrade = 0;

    public static DateTime m_LoginServerTime;

    public static float m_MusicVolumeDecrease;

    public static float m_SoundVolumeDecrease;

    public static int m_TutorialIndex = 0;

    public static int m_TutorialSubgroupIndex = 0;

    public static bool m_HasFinishedTutorial = false;

    public static bool m_IsMainGame = true;

    public static int m_UserSegmentIndex = 0;

    public static bool m_HasGetGhostCard = false;

    public static string m_AuthGameID = "AS394nGOA@laen083KAWmp439-24905we#*%&^jawr84jiR#(JIOI#TRQ*(#)TR*(Q#INTRHereaaerea";

    public static string PlayerName
    {
        get
        {
            return m_PlayerName;
        }
        set
        {
            m_PlayerName = value;
            _ = m_PlayerName == "";
        }
    }

    protected CPlayerData()
    {
    }

    public static string GetPlayerName()
    {
        if (m_PlayerName == null)
        {
            return "";
        }
        return m_PlayerName;
    }

    private void Awake()
    {
        CEventManager.AddListener<CEventPlayer_SetCoin>(CPlayer_OnSetCoin);
        CreateDefaultData();
    }

    public static void ResetData()
    {
        GameInstance.m_IsRestartingGameDeleteAll = true;
        CreateDefaultData();
        CEventManager.QueueEvent(new CEventPlayer_ChangeScene(ELevelName.Start));
    }

    public static void CreateDefaultData(bool isRebornReset = false)
    {
        if (!isRebornReset)
        {
            m_PlayerName = "";
            m_CanCloudLoad = true;
            m_CollectionSortingMethodIndexList.Clear();
        }
        m_TutorialIndex = 0;
        m_TutorialSubgroupIndex = 0;
        m_TutorialDataList.Clear();
        m_HasFinishedTutorial = false;
        m_HasGetGhostCard = false;
        m_CoinAmount = 1000f;
        m_CoinAmountDouble = 1000.0;
        m_FamePoint = 0;
        m_UnlockRoomCount = 0;
        m_UnlockWarehouseRoomCount = 0;
        m_IsScannerRestockUnlocked = false;
        m_IsWarehouseRoomUnlocked = false;
        m_GameReportDataCollect.ResetData();
        CEventManager.QueueEvent(new CEventPlayer_SetCoin(m_CoinAmount, m_CoinAmountDouble));
        CEventManager.QueueEvent(new CEventPlayer_SetFame(0));
        m_SkyboxBlendValue = 0f;
        m_LightTimeData = null;
        m_GameEventFormat = EGameEventFormat.Standard;
        m_PendingGameEventFormat = EGameEventFormat.None;
        m_GameEventExpansionType = ECardExpansionType.Tetramon;
        m_PendingGameEventExpansionType = ECardExpansionType.Tetramon;
        m_LastLocalExitTime = DateTime.UtcNow;
        m_TotalFameAdd = 0;
        m_CurrentDay = 0;
        m_ShopExpPoint = 0;
        m_ShopLevel = 0;
        m_EquippedWallDecoIndex = 0;
        m_EquippedWallDecoIndexB = 0;
        m_EquippedFloorDecoIndex = 0;
        m_EquippedFloorDecoIndexB = 0;
        m_EquippedCeilingDecoIndex = 0;
        m_EquippedCeilingDecoIndexB = 0;
        m_GradedCollectionSortingMethodIndex = 0;
        m_LastSelectedPriceGraphCardGrade = 0;
        m_IsShopOpen = false;
        m_IsShopOnceOpen = false;
        m_IsWarehouseDoorClosed = false;
        m_IsItemPriceGenerated = false;
        m_IsCardPriceGenerated = false;
        m_GameReportDataCollectPermanent = default(GameReportDataCollect);
        m_GameReportDataCollect = default(GameReportDataCollect);
        m_GameReportDataCollectPastList.Clear();
        m_ShelfSaveDataList.Clear();
        m_WarehouseShelfSaveDataList.Clear();
        m_PackageBoxItemSaveDataList.Clear();
        m_PackageBoxCardSaveDataList.Clear();
        m_InteractableObjectSaveDataList.Clear();
        m_DecoObjectSaveDataList.Clear();
        m_CardShelfSaveDataList.Clear();
        m_PlayTableSaveDataList.Clear();
        m_AutoCleanserSaveDataList.Clear();
        m_AutoPackOpenerSaveDataList.Clear();
        m_WorkbenchSaveDataList.Clear();
        m_CashCounterSaveDataList.Clear();
        m_CustomerSaveDataList.Clear();
        m_WorkerSaveDataList.Clear();
        m_CustomerReviewDataList.Clear();
        m_BillList.Clear();
        m_HoldCardDataList.Clear();
        m_HoldItemTypeList.Clear();
        m_TargetBuyItemList.Clear();
        for (int i = 0; i < 6; i++)
        {
            m_CollectionSortingMethodIndexList.Add(2);
        }
        m_CardCollectedList.Clear();
        m_CardCollectedListDestiny.Clear();
        m_CardCollectedListGhost.Clear();
        m_CardCollectedListGhostBlack.Clear();
        m_CardCollectedListMegabot.Clear();
        m_CardCollectedListFantasyRPG.Clear();
        m_CardCollectedListCatJob.Clear();
        m_GradedCardInventoryList.Clear();
        m_IsCardCollectedList.Clear();
        m_IsCardCollectedListDestiny.Clear();
        m_IsCardCollectedListGhost.Clear();
        m_IsCardCollectedListGhostBlack.Clear();
        m_IsCardCollectedListMegabot.Clear();
        m_IsCardCollectedListFantasyRPG.Clear();
        m_IsCardCollectedListCatJob.Clear();
        m_CardPriceSetList.Clear();
        m_CardPriceSetListDestiny.Clear();
        m_CardPriceSetListGhost.Clear();
        m_CardPriceSetListGhostBlack.Clear();
        m_CardPriceSetListMegabot.Clear();
        m_CardPriceSetListFantasyRPG.Clear();
        m_CardPriceSetListCatJob.Clear();
        m_GradedCardPriceSetList.Clear();
        m_GradedCardPriceSetListDestiny.Clear();
        m_GradedCardPriceSetListGhost.Clear();
        m_GradedCardPriceSetListGhostBlack.Clear();
        m_GradedCardPriceSetListMegabot.Clear();
        m_GradedCardPriceSetListFantasyRPG.Clear();
        m_GradedCardPriceSetListCatJob.Clear();
        m_GenCardMarketPriceList.Clear();
        m_GenCardMarketPriceListDestiny.Clear();
        m_GenCardMarketPriceListGhost.Clear();
        m_GenCardMarketPriceListGhostBlack.Clear();
        m_GenCardMarketPriceListMegabot.Clear();
        m_GenCardMarketPriceListFantasyRPG.Clear();
        m_GenCardMarketPriceListCatJob.Clear();
        m_GenGradedCardPriceMultiplierList.Clear();
        m_GradeCardInProgressList.Clear();
        for (int j = 0; j < GetCardCollectionDataCount() + 100; j++)
        {
            m_CardCollectedList.Add(0);
            m_CardCollectedListDestiny.Add(0);
            m_CardCollectedListGhost.Add(0);
            m_CardCollectedListGhostBlack.Add(0);
            m_CardCollectedListMegabot.Add(0);
            m_CardCollectedListFantasyRPG.Add(0);
            m_CardCollectedListCatJob.Add(0);
            m_IsCardCollectedList.Add(item: false);
            m_IsCardCollectedListDestiny.Add(item: false);
            m_IsCardCollectedListGhost.Add(item: false);
            m_IsCardCollectedListGhostBlack.Add(item: false);
            m_IsCardCollectedListMegabot.Add(item: false);
            m_IsCardCollectedListFantasyRPG.Add(item: false);
            m_IsCardCollectedListCatJob.Add(item: false);
            m_CardPriceSetList.Add(0f);
            m_CardPriceSetListDestiny.Add(0f);
            m_CardPriceSetListGhost.Add(0f);
            m_CardPriceSetListGhostBlack.Add(0f);
            m_CardPriceSetListMegabot.Add(0f);
            m_CardPriceSetListFantasyRPG.Add(0f);
            m_CardPriceSetListCatJob.Add(0f);
            FloatList floatList = new FloatList();
            floatList.floatDataList = new List<float>();
            for (int k = 0; k < 10; k++)
            {
                floatList.floatDataList.Add(0f);
            }
            m_GradedCardPriceSetList.Add(floatList);
            floatList = new FloatList();
            floatList.floatDataList = new List<float>();
            for (int l = 0; l < 10; l++)
            {
                floatList.floatDataList.Add(0f);
            }
            m_GradedCardPriceSetListDestiny.Add(floatList);
            floatList = new FloatList();
            floatList.floatDataList = new List<float>();
            for (int m = 0; m < 10; m++)
            {
                floatList.floatDataList.Add(0f);
            }
            m_GradedCardPriceSetListGhost.Add(floatList);
            floatList = new FloatList();
            floatList.floatDataList = new List<float>();
            for (int n = 0; n < 10; n++)
            {
                floatList.floatDataList.Add(0f);
            }
            m_GradedCardPriceSetListGhostBlack.Add(floatList);
            floatList = new FloatList();
            floatList.floatDataList = new List<float>();
            for (int num = 0; num < 10; num++)
            {
                floatList.floatDataList.Add(0f);
            }
            m_GradedCardPriceSetListMegabot.Add(floatList);
            floatList = new FloatList();
            floatList.floatDataList = new List<float>();
            for (int num2 = 0; num2 < 10; num2++)
            {
                floatList.floatDataList.Add(0f);
            }
            m_GradedCardPriceSetListFantasyRPG.Add(floatList);
            floatList = new FloatList();
            floatList.floatDataList = new List<float>();
            for (int num3 = 0; num3 < 10; num3++)
            {
                floatList.floatDataList.Add(0f);
            }
            m_GradedCardPriceSetListCatJob.Add(floatList);
            MarketPrice marketPrice = new MarketPrice();
            marketPrice.pastPricePercentChangeList = new List<float>();
            m_GenCardMarketPriceList.Add(marketPrice);
            marketPrice = new MarketPrice();
            marketPrice.pastPricePercentChangeList = new List<float>();
            m_GenCardMarketPriceListDestiny.Add(marketPrice);
            marketPrice = new MarketPrice();
            marketPrice.pastPricePercentChangeList = new List<float>();
            m_GenCardMarketPriceListGhost.Add(marketPrice);
            marketPrice = new MarketPrice();
            marketPrice.pastPricePercentChangeList = new List<float>();
            m_GenCardMarketPriceListGhostBlack.Add(marketPrice);
            marketPrice = new MarketPrice();
            marketPrice.pastPricePercentChangeList = new List<float>();
            m_GenCardMarketPriceListMegabot.Add(marketPrice);
            marketPrice = new MarketPrice();
            marketPrice.pastPricePercentChangeList = new List<float>();
            m_GenCardMarketPriceListFantasyRPG.Add(marketPrice);
            marketPrice = new MarketPrice();
            marketPrice.pastPricePercentChangeList = new List<float>();
            m_GenCardMarketPriceListCatJob.Add(marketPrice);
        }
        m_SpawnBoxRestockIndexWaitingList.Clear();
        m_SpawnBoxItemCountWaitingList.Clear();
        m_IsItemLicenseUnlocked.Clear();
        m_StockSoldList.Clear();
        m_CurrentTotalItemCountList.Clear();
        m_SetItemPriceList.Clear();
        m_AverageItemCostList.Clear();
        m_GeneratedCostPriceList.Clear();
        m_GeneratedMarketPriceList.Clear();
        m_ItemPricePercentChangeList.Clear();
        m_ItemPricePercentPastChangeList.Clear();
        m_SetGameEventPriceList.Clear();
        m_GeneratedGameEventPriceList.Clear();
        m_GameEventPricePercentChangeList.Clear();
        m_TransactionDataList.Clear();
        m_CollectionCardPackCountList.Clear();
        m_IsItemLicenseUnlocked.Add(item: true);
        m_IsWorkerHired.Clear();
        m_IsAchievementUnlocked.Clear();
        for (int num4 = 0; num4 < 100; num4++)
        {
            m_CollectionCardPackCountList.Add(0);
            m_SetGameEventPriceList.Add(0f);
            m_GeneratedGameEventPriceList.Add(0f);
            m_GameEventPricePercentChangeList.Add(0f);
            m_IsWorkerHired.Add(item: false);
            m_IsAchievementUnlocked.Add(item: false);
        }
        for (int num5 = 0; num5 < 500; num5++)
        {
            m_IsItemLicenseUnlocked.Add(item: false);
            m_CurrentTotalItemCountList.Add(0);
            m_SetItemPriceList.Add(0f);
            m_AverageItemCostList.Add(0f);
            m_GeneratedCostPriceList.Add(0f);
            m_GeneratedMarketPriceList.Add(0f);
            m_ItemPricePercentChangeList.Add(0f);
            FloatList floatList2 = new FloatList();
            floatList2.floatDataList = new List<float>();
            m_ItemPricePercentPastChangeList.Add(floatList2);
            m_StockSoldList.Add(0);
        }
        m_DecorationInventoryList.Clear();
        m_UnlockedDecoWallList.Clear();
        m_UnlockedDecoFloorList.Clear();
        m_UnlockedDecoCeilingList.Clear();
        m_UnlockedDecoWallList.Add(item: true);
        m_UnlockedDecoFloorList.Add(item: true);
        m_UnlockedDecoCeilingList.Add(item: true);
        for (int num6 = 0; num6 < 1000; num6++)
        {
            m_UnlockedDecoWallList.Add(item: false);
            m_UnlockedDecoFloorList.Add(item: false);
            m_UnlockedDecoCeilingList.Add(item: false);
            m_DecorationInventoryList.Add(0);
        }
        m_WorkbenchMinimumCardLimit = 4;
        m_WorkbenchPriceLimit = 0.5f;
        m_WorkbenchPriceMinimum = 0.01f;
        m_WorkbenchRarityLimit = ERarity.None;
        m_WorkbenchCardExpansionType = ECardExpansionType.Tetramon;
        m_RestockSortingType = ERestockSortingType.Default;
        m_CustomerReviewCount = 0;
        m_CustomerReviewScoreAverage = 0f;
        m_CurrentGradeCardSubmitSet = new GradeCardSubmitSet();
        m_CurrentGradeCardSubmitSet.m_CardDataList = new List<CardData>();
        for (int num7 = 0; num7 < 8; num7++)
        {
            m_CurrentGradeCardSubmitSet.m_CardDataList.Add(new CardData());
        }
        m_ChampionCardCollectedList.Clear();
        CSingleton<ShelfManager>.Instance.SaveInteractableObjectData(ignoreFinishLoadingCheck: true);
    }

    public static int GetSellAmountRequiredToCollectPack()
    {
        return 100;
    }

    public static int GetCardCollectionDataCount()
    {
        return 2000;
    }

    public static int GetExpRequiredToLevelUp()
    {
        return 250 + m_ShopLevel * 200 + m_ShopLevel * m_ShopLevel * 10;
    }

    public static int GetExpRequiredToLevelUpAtLevel(int shopLevel)
    {
        return 250 + shopLevel * 200 + shopLevel * shopLevel * 10;
    }

    public static int GetCardAmountPerMonsterType(ECardExpansionType expansionType, bool includeFoilCount = true)
    {
        int num = 6;
        switch (expansionType)
        {
            case ECardExpansionType.Tetramon:
                num = 6;
                break;
            case ECardExpansionType.Destiny:
                num = 6;
                break;
            case ECardExpansionType.Ghost:
                num = 1;
                break;
            case ECardExpansionType.Megabot:
                num = 6;
                break;
            case ECardExpansionType.FantasyRPG:
                num = 6;
                break;
            case ECardExpansionType.CatJob:
                num = 6;
                break;
        }
        if (includeFoilCount)
        {
            return num * 2;
        }
        return num;
    }

    public static ECardBorderType GetCardBorderType(int borderTypeIndex, ECardExpansionType expansionType)
    {
        List<ECardBorderType> list = new List<ECardBorderType>();
        list.Add(ECardBorderType.Base);
        list.Add(ECardBorderType.FirstEdition);
        list.Add(ECardBorderType.Silver);
        list.Add(ECardBorderType.Gold);
        list.Add(ECardBorderType.EX);
        list.Add(ECardBorderType.FullArt);
        if (expansionType == ECardExpansionType.Ghost)
        {
            list.Clear();
            list.Add(ECardBorderType.FullArt);
        }
        return list[borderTypeIndex % GetCardAmountPerMonsterType(expansionType, includeFoilCount: false)];
    }

    public static List<int> GetCardCollectedList(ECardExpansionType expansionType, bool isDimension)
    {
        switch (expansionType)
        {
            case ECardExpansionType.Tetramon:
                return m_CardCollectedList;
            case ECardExpansionType.Destiny:
                return m_CardCollectedListDestiny;
            case ECardExpansionType.Ghost:
                if (isDimension)
                {
                    return m_CardCollectedListGhostBlack;
                }
                return m_CardCollectedListGhost;
            case ECardExpansionType.Megabot:
                return m_CardCollectedListMegabot;
            case ECardExpansionType.FantasyRPG:
                return m_CardCollectedListFantasyRPG;
            case ECardExpansionType.CatJob:
                return m_CardCollectedListCatJob;
            default:
                return null;
        }
    }

    public static List<bool> GetIsCardCollectedList(ECardExpansionType expansionType, bool isDimension)
    {
        switch (expansionType)
        {
            case ECardExpansionType.Tetramon:
                return m_IsCardCollectedList;
            case ECardExpansionType.Destiny:
                return m_IsCardCollectedListDestiny;
            case ECardExpansionType.Ghost:
                if (isDimension)
                {
                    return m_IsCardCollectedListGhostBlack;
                }
                return m_IsCardCollectedListGhost;
            case ECardExpansionType.Megabot:
                return m_IsCardCollectedListMegabot;
            case ECardExpansionType.FantasyRPG:
                return m_IsCardCollectedListFantasyRPG;
            case ECardExpansionType.CatJob:
                return m_IsCardCollectedListCatJob;
            default:
                return null;
        }
    }

    public static EMonsterType GetMonsterTypeFromCardSaveIndex(int index, ECardExpansionType cardExpansionType)
    {
        return InventoryBase.GetShownMonsterList(cardExpansionType)[index / GetCardAmountPerMonsterType(cardExpansionType)];
    }

    public static int GetCardSaveIndex(CardData cardData)
    {
        int num = 0;
        for (int i = 0; i < InventoryBase.GetShownMonsterList(cardData.expansionType).Count; i++)
        {
            if (InventoryBase.GetShownMonsterList(cardData.expansionType)[i] == cardData.monsterType)
            {
                num = (int)(i * GetCardAmountPerMonsterType(cardData.expansionType) + cardData.borderType);
                break;
            }
        }
        if (cardData.isFoil)
        {
            num += GetCardAmountPerMonsterType(cardData.expansionType, includeFoilCount: false);
        }
        return num;
    }

    public static float GetUnlockShopRoomCost(int level)
    {
        float num = 100 + level / 4 * 50;
        if (level > 24)
        {
            num *= 2f;
        }
        else if (level > 19)
        {
            num *= 1.5f;
        }
        return 300f + num * (float)level;
    }

    public static float GetUnlockWarehouseRoomCost(int level)
    {
        float num = 500 + level / 2 * 200;
        if (level > 7)
        {
            num *= 1.5f;
        }
        return 1000f + num * (float)level;
    }

    public static void AddCurrentTotalItemCount(EItemType itemType, int amount)
    {
        m_CurrentTotalItemCountList[(int)itemType] += amount;
        if (m_CurrentTotalItemCountList[(int)itemType] < 0)
        {
            m_CurrentTotalItemCountList[(int)itemType] = 0;
        }
    }

    public static int GetCurrentTotalItemCount(EItemType itemType)
    {
        if (itemType == EItemType.None)
        {
            return 0;
        }
        if (m_CurrentTotalItemCountList[(int)itemType] < 0)
        {
            m_CurrentTotalItemCountList[(int)itemType] = 0;
        }
        return m_CurrentTotalItemCountList[(int)itemType];
    }

    public static void AddItemPricePercentChange(EItemType itemType, float percentChange)
    {
        m_ItemPricePercentChangeList[(int)itemType] += percentChange;
        if (m_ItemPricePercentChangeList[(int)itemType] < -80f)
        {
            m_ItemPricePercentChangeList[(int)itemType] = -80f;
        }
        if (m_ItemPricePercentChangeList[(int)itemType] > 200f)
        {
            m_ItemPricePercentChangeList[(int)itemType] = 200f;
        }
    }

    public static void UpdateItemPricePercentChange()
    {
        for (int i = 0; i < m_ItemPricePercentChangeList.Count; i++)
        {
            if (m_ItemPricePercentPastChangeList[i].floatDataList == null)
            {
                m_ItemPricePercentPastChangeList[i].floatDataList = new List<float>();
            }
            m_ItemPricePercentPastChangeList[i].floatDataList.Add(m_ItemPricePercentChangeList[i]);
            if (m_ItemPricePercentPastChangeList[i].floatDataList.Count > 30)
            {
                m_ItemPricePercentPastChangeList[i].floatDataList.RemoveAt(0);
            }
        }
    }

    public static void SetItemPrice(EItemType itemType, float price)
    {
        m_SetItemPriceList[(int)itemType] = price;
        CEventManager.QueueEvent(new CEventPlayer_ItemPriceChanged(itemType, price));
    }

    public static float GetItemPrice(EItemType itemType, bool preventZero = true)
    {
        if (itemType == EItemType.None)
        {
            return 0f;
        }
        float num = m_SetItemPriceList[(int)itemType];
        if (preventZero && num <= 0f)
        {
            num = GetAverageItemCost(itemType);
            if (num <= 0f)
            {
                num = GetItemMarketPrice(itemType);
            }
        }
        double num2 = Math.Round(num, 2, MidpointRounding.AwayFromZero);
        if (GameInstance.GetCurrencyConversionRate() > 1f)
        {
            num2 = Math.Round(num, 3, MidpointRounding.AwayFromZero);
        }
        return (float)num2;
    }

    public static float GetItemCost(EItemType itemType)
    {
        if (itemType == EItemType.None)
        {
            return 0f;
        }
        return RestockManager.GetItemCost(itemType);
    }

    public static void UpdateAverageItemCost(EItemType itemType, int addItemCount, float unitCost)
    {
        if (itemType != EItemType.None)
        {
            int num = GetCurrentTotalItemCount(itemType);
            float averageItemCost = GetAverageItemCost(itemType);
            if (num < 0)
            {
                num = 0;
            }
            float num2 = ((float)num * averageItemCost + (float)addItemCount * unitCost) / (float)(num + addItemCount);
            if (num2 < 0f || num2 > GetItemMarketPrice(itemType) * 5f || float.IsNaN(num2))
            {
                num2 = GetItemCost(itemType);
            }
            m_AverageItemCostList[(int)itemType] = num2;
            AddCurrentTotalItemCount(itemType, addItemCount);
        }
    }

    public static void SetAverageItemCost(EItemType itemType, float cost)
    {
        if (itemType != EItemType.None)
        {
            m_AverageItemCostList[(int)itemType] = cost;
        }
    }

    public static float GetAverageItemCost(EItemType itemType)
    {
        if (itemType == EItemType.None)
        {
            return 0f;
        }
        float num = m_AverageItemCostList[(int)itemType];
        if (num < 0f || num > GetItemMarketPrice(itemType) * 5f || float.IsNaN(num))
        {
            num = GetItemCost(itemType);
        }
        return Mathf.Round(num * 100f) / 100f;
    }

    public static float GetItemMarketPrice(EItemType itemType)
    {
        if (itemType == EItemType.None)
        {
            return 0f;
        }
        return RestockManager.GetItemMarketPrice(itemType);
    }

    public static float GetItemMarketPriceCustomPercent(EItemType itemType, float percent)
    {
        if (itemType == EItemType.None)
        {
            return 0f;
        }
        return RestockManager.GetItemMarketPriceCustomPercent(itemType, percent);
    }

    public static bool GetIsItemLicenseUnlocked(int index)
    {
        return m_IsItemLicenseUnlocked[index];
    }

    public static void SetUnlockItemLicense(int index)
    {
        m_IsItemLicenseUnlocked[index] = true;
    }

    public static bool GetIsWorkerHired(int index)
    {
        return m_IsWorkerHired[index];
    }

    public static void SetIsWorkerHired(int index, bool isHired)
    {
        m_IsWorkerHired[index] = isHired;
    }

    public static float GetCardPrice(CardData cardData)
    {
        float num = 0f;
        int cardSaveIndex = GetCardSaveIndex(cardData);
        if (cardData.expansionType == ECardExpansionType.Tetramon)
        {
            num = ((cardData.cardGrade <= 0) ? m_CardPriceSetList[cardSaveIndex] : m_GradedCardPriceSetList[cardSaveIndex].floatDataList[cardData.cardGrade - 1]);
        }
        else if (cardData.expansionType == ECardExpansionType.Destiny)
        {
            num = ((cardData.cardGrade <= 0) ? m_CardPriceSetListDestiny[cardSaveIndex] : m_GradedCardPriceSetListDestiny[cardSaveIndex].floatDataList[cardData.cardGrade - 1]);
        }
        else if (cardData.expansionType == ECardExpansionType.Ghost)
        {
            num = (cardData.isDestiny ? ((cardData.cardGrade <= 0) ? m_CardPriceSetListGhostBlack[cardSaveIndex] : m_GradedCardPriceSetListGhostBlack[cardSaveIndex].floatDataList[cardData.cardGrade - 1]) : ((cardData.cardGrade <= 0) ? m_CardPriceSetListGhost[cardSaveIndex] : m_GradedCardPriceSetListGhost[cardSaveIndex].floatDataList[cardData.cardGrade - 1]));
        }
        else if (cardData.expansionType == ECardExpansionType.Megabot)
        {
            num = ((cardData.cardGrade <= 0) ? m_CardPriceSetListMegabot[cardSaveIndex] : m_GradedCardPriceSetListMegabot[cardSaveIndex].floatDataList[cardData.cardGrade - 1]);
        }
        else if (cardData.expansionType == ECardExpansionType.FantasyRPG)
        {
            num = ((cardData.cardGrade <= 0) ? m_CardPriceSetListFantasyRPG[cardSaveIndex] : m_GradedCardPriceSetListFantasyRPG[cardSaveIndex].floatDataList[cardData.cardGrade - 1]);
        }
        else if (cardData.expansionType == ECardExpansionType.CatJob)
        {
            num = ((cardData.cardGrade <= 0) ? m_CardPriceSetListCatJob[cardSaveIndex] : m_GradedCardPriceSetListCatJob[cardSaveIndex].floatDataList[cardData.cardGrade - 1]);
        }
        double num2 = Math.Round(num, 2, MidpointRounding.AwayFromZero);
        if (GameInstance.GetCurrencyConversionRate() > 1f)
        {
            num2 = Math.Round(num, 3, MidpointRounding.AwayFromZero);
        }
        return (float)num2;
    }

    public static float GetCardPriceUsingIndex(int index, ECardExpansionType expansionType, bool isDestiny, int cardGrade)
    {
        float num = 0f;
        switch (expansionType)
        {
            case ECardExpansionType.Tetramon:
                num = ((cardGrade <= 0) ? m_CardPriceSetList[index] : m_GradedCardPriceSetList[index].floatDataList[cardGrade - 1]);
                break;
            case ECardExpansionType.Destiny:
                num = ((cardGrade <= 0) ? m_CardPriceSetListDestiny[index] : m_GradedCardPriceSetListDestiny[index].floatDataList[cardGrade - 1]);
                break;
            case ECardExpansionType.Ghost:
                num = ((!isDestiny) ? ((cardGrade <= 0) ? m_CardPriceSetListGhost[index] : m_GradedCardPriceSetListGhost[index].floatDataList[cardGrade - 1]) : ((cardGrade <= 0) ? m_CardPriceSetListGhostBlack[index] : m_GradedCardPriceSetListGhostBlack[index].floatDataList[cardGrade - 1]));
                break;
            case ECardExpansionType.Megabot:
                num = ((cardGrade <= 0) ? m_CardPriceSetListMegabot[index] : m_GradedCardPriceSetListMegabot[index].floatDataList[cardGrade - 1]);
                break;
            case ECardExpansionType.FantasyRPG:
                num = ((cardGrade <= 0) ? m_CardPriceSetListFantasyRPG[index] : m_GradedCardPriceSetListFantasyRPG[index].floatDataList[cardGrade - 1]);
                break;
            case ECardExpansionType.CatJob:
                num = ((cardGrade <= 0) ? m_CardPriceSetListCatJob[index] : m_GradedCardPriceSetListCatJob[index].floatDataList[cardGrade - 1]);
                break;
        }
        double num2 = Math.Round(num, 2, MidpointRounding.AwayFromZero);
        if (GameInstance.GetCurrencyConversionRate() > 1f)
        {
            num2 = Math.Round(num, 3, MidpointRounding.AwayFromZero);
        }
        return (float)num2;
    }

    public static void SetCardPrice(CardData cardData, float priceSet)
    {
        int cardSaveIndex = GetCardSaveIndex(cardData);
        if (cardData.expansionType == ECardExpansionType.Tetramon)
        {
            if (cardData.cardGrade > 0)
            {
                m_GradedCardPriceSetList[cardSaveIndex].floatDataList[cardData.cardGrade - 1] = priceSet;
            }
            else
            {
                m_CardPriceSetList[cardSaveIndex] = priceSet;
            }
        }
        else if (cardData.expansionType == ECardExpansionType.Destiny)
        {
            if (cardData.cardGrade > 0)
            {
                m_GradedCardPriceSetListDestiny[cardSaveIndex].floatDataList[cardData.cardGrade - 1] = priceSet;
            }
            else
            {
                m_CardPriceSetListDestiny[cardSaveIndex] = priceSet;
            }
        }
        else if (cardData.expansionType == ECardExpansionType.Ghost)
        {
            if (cardData.isDestiny)
            {
                if (cardData.cardGrade > 0)
                {
                    m_GradedCardPriceSetListGhostBlack[cardSaveIndex].floatDataList[cardData.cardGrade - 1] = priceSet;
                }
                else
                {
                    m_CardPriceSetListGhostBlack[cardSaveIndex] = priceSet;
                }
            }
            else if (cardData.cardGrade > 0)
            {
                m_GradedCardPriceSetListGhost[cardSaveIndex].floatDataList[cardData.cardGrade - 1] = priceSet;
            }
            else
            {
                m_CardPriceSetListGhost[cardSaveIndex] = priceSet;
            }
        }
        else if (cardData.expansionType == ECardExpansionType.Megabot)
        {
            if (cardData.cardGrade > 0)
            {
                m_GradedCardPriceSetListMegabot[cardSaveIndex].floatDataList[cardData.cardGrade - 1] = priceSet;
            }
            else
            {
                m_CardPriceSetListMegabot[cardSaveIndex] = priceSet;
            }
        }
        else if (cardData.expansionType == ECardExpansionType.FantasyRPG)
        {
            if (cardData.cardGrade > 0)
            {
                m_GradedCardPriceSetListFantasyRPG[cardSaveIndex].floatDataList[cardData.cardGrade - 1] = priceSet;
            }
            else
            {
                m_CardPriceSetListFantasyRPG[cardSaveIndex] = priceSet;
            }
        }
        else if (cardData.expansionType == ECardExpansionType.CatJob)
        {
            if (cardData.cardGrade > 0)
            {
                m_GradedCardPriceSetListCatJob[cardSaveIndex].floatDataList[cardData.cardGrade - 1] = priceSet;
            }
            else
            {
                m_CardPriceSetListCatJob[cardSaveIndex] = priceSet;
            }
        }
        CEventManager.QueueEvent(new CEventPlayer_CardPriceChanged(cardData, priceSet));
    }

    public static void SetCardGeneratedMarketPrice(int cardIndex, ECardExpansionType expansionType, bool isDestiny, float price)
    {
        switch (expansionType)
        {
            case ECardExpansionType.Tetramon:
                m_GenCardMarketPriceList[cardIndex].generatedMarketPrice = price;
                break;
            case ECardExpansionType.Destiny:
                m_GenCardMarketPriceListDestiny[cardIndex].generatedMarketPrice = price;
                break;
            case ECardExpansionType.Ghost:
                if (isDestiny)
                {
                    m_GenCardMarketPriceListGhost[cardIndex].generatedMarketPrice = price;
                }
                else
                {
                    m_GenCardMarketPriceListGhostBlack[cardIndex].generatedMarketPrice = price;
                }
                break;
            case ECardExpansionType.Megabot:
                m_GenCardMarketPriceListMegabot[cardIndex].generatedMarketPrice = price;
                break;
            case ECardExpansionType.FantasyRPG:
                m_GenCardMarketPriceListFantasyRPG[cardIndex].generatedMarketPrice = price;
                break;
            case ECardExpansionType.CatJob:
                m_GenCardMarketPriceListCatJob[cardIndex].generatedMarketPrice = price;
                break;
        }
    }

    public static void AddCardPricePercentChange(int cardIndex, ECardExpansionType expansionType, bool isDestiny, float percentChange)
    {
        switch (expansionType)
        {
            case ECardExpansionType.Tetramon:
                m_GenCardMarketPriceList[cardIndex].pricePercentChangeList += percentChange;
                if (m_GenCardMarketPriceList[cardIndex].pricePercentChangeList < -80f)
                {
                    m_GenCardMarketPriceList[cardIndex].pricePercentChangeList = -80f;
                }
                if (m_GenCardMarketPriceList[cardIndex].pricePercentChangeList > 200f)
                {
                    m_GenCardMarketPriceList[cardIndex].pricePercentChangeList = 200f;
                }
                break;
            case ECardExpansionType.Destiny:
                m_GenCardMarketPriceListDestiny[cardIndex].pricePercentChangeList += percentChange;
                if (m_GenCardMarketPriceListDestiny[cardIndex].pricePercentChangeList < -80f)
                {
                    m_GenCardMarketPriceListDestiny[cardIndex].pricePercentChangeList = -80f;
                }
                if (m_GenCardMarketPriceListDestiny[cardIndex].pricePercentChangeList > 200f)
                {
                    m_GenCardMarketPriceListDestiny[cardIndex].pricePercentChangeList = 200f;
                }
                break;
            case ECardExpansionType.Ghost:
                if (isDestiny)
                {
                    m_GenCardMarketPriceListGhost[cardIndex].pricePercentChangeList += percentChange;
                    if (m_GenCardMarketPriceListGhost[cardIndex].pricePercentChangeList < -80f)
                    {
                        m_GenCardMarketPriceListGhost[cardIndex].pricePercentChangeList = -80f;
                    }
                    if (m_GenCardMarketPriceListGhost[cardIndex].pricePercentChangeList > 200f)
                    {
                        m_GenCardMarketPriceListGhost[cardIndex].pricePercentChangeList = 200f;
                    }
                }
                else
                {
                    m_GenCardMarketPriceListGhostBlack[cardIndex].pricePercentChangeList += percentChange;
                    if (m_GenCardMarketPriceListGhostBlack[cardIndex].pricePercentChangeList < -80f)
                    {
                        m_GenCardMarketPriceListGhostBlack[cardIndex].pricePercentChangeList = -80f;
                    }
                    if (m_GenCardMarketPriceListGhostBlack[cardIndex].pricePercentChangeList > 200f)
                    {
                        m_GenCardMarketPriceListGhostBlack[cardIndex].pricePercentChangeList = 200f;
                    }
                }
                break;
            case ECardExpansionType.Megabot:
                m_GenCardMarketPriceListMegabot[cardIndex].pricePercentChangeList += percentChange;
                if (m_GenCardMarketPriceListMegabot[cardIndex].pricePercentChangeList < -80f)
                {
                    m_GenCardMarketPriceListMegabot[cardIndex].pricePercentChangeList = -80f;
                }
                if (m_GenCardMarketPriceListMegabot[cardIndex].pricePercentChangeList > 200f)
                {
                    m_GenCardMarketPriceListMegabot[cardIndex].pricePercentChangeList = 200f;
                }
                break;
            case ECardExpansionType.FantasyRPG:
                m_GenCardMarketPriceListFantasyRPG[cardIndex].pricePercentChangeList += percentChange;
                if (m_GenCardMarketPriceListFantasyRPG[cardIndex].pricePercentChangeList < -80f)
                {
                    m_GenCardMarketPriceListFantasyRPG[cardIndex].pricePercentChangeList = -80f;
                }
                if (m_GenCardMarketPriceListFantasyRPG[cardIndex].pricePercentChangeList > 200f)
                {
                    m_GenCardMarketPriceListFantasyRPG[cardIndex].pricePercentChangeList = 200f;
                }
                break;
            case ECardExpansionType.CatJob:
                m_GenCardMarketPriceListCatJob[cardIndex].pricePercentChangeList += percentChange;
                if (m_GenCardMarketPriceListCatJob[cardIndex].pricePercentChangeList < -80f)
                {
                    m_GenCardMarketPriceListCatJob[cardIndex].pricePercentChangeList = -80f;
                }
                if (m_GenCardMarketPriceListCatJob[cardIndex].pricePercentChangeList > 200f)
                {
                    m_GenCardMarketPriceListCatJob[cardIndex].pricePercentChangeList = 200f;
                }
                break;
        }
    }

    public static float GetCardPricePercentChange(int cardIndex, ECardExpansionType expansionType, bool isDestiny)
    {
        switch (expansionType)
        {
            case ECardExpansionType.Tetramon:
                return m_GenCardMarketPriceList[cardIndex].pricePercentChangeList;
            case ECardExpansionType.Destiny:
                return m_GenCardMarketPriceListDestiny[cardIndex].pricePercentChangeList;
            case ECardExpansionType.Ghost:
                if (isDestiny)
                {
                    return m_GenCardMarketPriceListGhost[cardIndex].pricePercentChangeList;
                }
                return m_GenCardMarketPriceListGhostBlack[cardIndex].pricePercentChangeList;
            case ECardExpansionType.Megabot:
                return m_GenCardMarketPriceListMegabot[cardIndex].pricePercentChangeList;
            case ECardExpansionType.FantasyRPG:
                return m_GenCardMarketPriceListFantasyRPG[cardIndex].pricePercentChangeList;
            case ECardExpansionType.CatJob:
                return m_GenCardMarketPriceListCatJob[cardIndex].pricePercentChangeList;
            default:
                return 0f;
        }
    }

    public static void UpdatePastCardPricePercentChange()
    {
        for (int i = 0; i < m_GenCardMarketPriceList.Count; i++)
        {
            if (m_GenCardMarketPriceList[i].pastPricePercentChangeList == null)
            {
                m_GenCardMarketPriceList[i].pastPricePercentChangeList = new List<float>();
            }
            m_GenCardMarketPriceList[i].pastPricePercentChangeList.Add(m_GenCardMarketPriceList[i].pricePercentChangeList);
            if (m_GenCardMarketPriceList[i].pastPricePercentChangeList.Count > 30)
            {
                m_GenCardMarketPriceList[i].pastPricePercentChangeList.RemoveAt(0);
            }
        }
        for (int j = 0; j < m_GenCardMarketPriceListDestiny.Count; j++)
        {
            if (m_GenCardMarketPriceListDestiny[j].pastPricePercentChangeList == null)
            {
                m_GenCardMarketPriceListDestiny[j].pastPricePercentChangeList = new List<float>();
            }
            m_GenCardMarketPriceListDestiny[j].pastPricePercentChangeList.Add(m_GenCardMarketPriceListDestiny[j].pricePercentChangeList);
            if (m_GenCardMarketPriceListDestiny[j].pastPricePercentChangeList.Count > 30)
            {
                m_GenCardMarketPriceListDestiny[j].pastPricePercentChangeList.RemoveAt(0);
            }
        }
        for (int k = 0; k < m_GenCardMarketPriceListGhost.Count; k++)
        {
            if (m_GenCardMarketPriceListGhost[k].pastPricePercentChangeList == null)
            {
                m_GenCardMarketPriceListGhost[k].pastPricePercentChangeList = new List<float>();
            }
            m_GenCardMarketPriceListGhost[k].pastPricePercentChangeList.Add(m_GenCardMarketPriceListGhost[k].pricePercentChangeList);
            if (m_GenCardMarketPriceListGhost[k].pastPricePercentChangeList.Count > 30)
            {
                m_GenCardMarketPriceListGhost[k].pastPricePercentChangeList.RemoveAt(0);
            }
        }
        for (int l = 0; l < m_GenCardMarketPriceListGhostBlack.Count; l++)
        {
            if (m_GenCardMarketPriceListGhostBlack[l].pastPricePercentChangeList == null)
            {
                m_GenCardMarketPriceListGhostBlack[l].pastPricePercentChangeList = new List<float>();
            }
            m_GenCardMarketPriceListGhostBlack[l].pastPricePercentChangeList.Add(m_GenCardMarketPriceListGhostBlack[l].pricePercentChangeList);
            if (m_GenCardMarketPriceListGhostBlack[l].pastPricePercentChangeList.Count > 30)
            {
                m_GenCardMarketPriceListGhostBlack[l].pastPricePercentChangeList.RemoveAt(0);
            }
        }
        for (int m = 0; m < m_GenCardMarketPriceListMegabot.Count; m++)
        {
            if (m_GenCardMarketPriceListMegabot[m].pastPricePercentChangeList == null)
            {
                m_GenCardMarketPriceListMegabot[m].pastPricePercentChangeList = new List<float>();
            }
            m_GenCardMarketPriceListMegabot[m].pastPricePercentChangeList.Add(m_GenCardMarketPriceListMegabot[m].pricePercentChangeList);
            if (m_GenCardMarketPriceListMegabot[m].pastPricePercentChangeList.Count > 30)
            {
                m_GenCardMarketPriceListMegabot[m].pastPricePercentChangeList.RemoveAt(0);
            }
        }
        for (int n = 0; n < m_GenCardMarketPriceListFantasyRPG.Count; n++)
        {
            if (m_GenCardMarketPriceListFantasyRPG[n].pastPricePercentChangeList == null)
            {
                m_GenCardMarketPriceListFantasyRPG[n].pastPricePercentChangeList = new List<float>();
            }
            m_GenCardMarketPriceListFantasyRPG[n].pastPricePercentChangeList.Add(m_GenCardMarketPriceListFantasyRPG[n].pricePercentChangeList);
            if (m_GenCardMarketPriceListFantasyRPG[n].pastPricePercentChangeList.Count > 30)
            {
                m_GenCardMarketPriceListFantasyRPG[n].pastPricePercentChangeList.RemoveAt(0);
            }
        }
        for (int num = 0; num < m_GenCardMarketPriceListCatJob.Count; num++)
        {
            if (m_GenCardMarketPriceListCatJob[num].pastPricePercentChangeList == null)
            {
                m_GenCardMarketPriceListCatJob[num].pastPricePercentChangeList = new List<float>();
            }
            m_GenCardMarketPriceListCatJob[num].pastPricePercentChangeList.Add(m_GenCardMarketPriceListCatJob[num].pricePercentChangeList);
            if (m_GenCardMarketPriceListCatJob[num].pastPricePercentChangeList.Count > 30)
            {
                m_GenCardMarketPriceListCatJob[num].pastPricePercentChangeList.RemoveAt(0);
            }
        }
    }

    public static List<float> GetPastCardPricePercentChange(int cardIndex, ECardExpansionType expansionType, bool isDestiny)
    {
        switch (expansionType)
        {
            case ECardExpansionType.Tetramon:
                return m_GenCardMarketPriceList[cardIndex].pastPricePercentChangeList;
            case ECardExpansionType.Destiny:
                return m_GenCardMarketPriceListDestiny[cardIndex].pastPricePercentChangeList;
            case ECardExpansionType.Ghost:
                if (isDestiny)
                {
                    return m_GenCardMarketPriceListGhost[cardIndex].pastPricePercentChangeList;
                }
                return m_GenCardMarketPriceListGhostBlack[cardIndex].pastPricePercentChangeList;
            case ECardExpansionType.Megabot:
                return m_GenCardMarketPriceListMegabot[cardIndex].pastPricePercentChangeList;
            case ECardExpansionType.FantasyRPG:
                return m_GenCardMarketPriceListFantasyRPG[cardIndex].pastPricePercentChangeList;
            case ECardExpansionType.CatJob:
                return m_GenCardMarketPriceListCatJob[cardIndex].pastPricePercentChangeList;
            default:
                return null;
        }
    }

    public static float GetCardMarketPrice(CardData cardData)
    {
        int cardSaveIndex = GetCardSaveIndex(cardData);
        if (cardData.expansionType == ECardExpansionType.Tetramon)
        {
            return m_GenCardMarketPriceList[cardSaveIndex].GetMarketPrice(cardSaveIndex, cardData.cardGrade);
        }
        if (cardData.expansionType == ECardExpansionType.Destiny)
        {
            return m_GenCardMarketPriceListDestiny[cardSaveIndex].GetMarketPrice(cardSaveIndex, cardData.cardGrade);
        }
        if (cardData.expansionType == ECardExpansionType.Ghost)
        {
            if (cardData.isDestiny)
            {
                return m_GenCardMarketPriceListGhost[cardSaveIndex].GetMarketPrice(cardSaveIndex, cardData.cardGrade);
            }
            return m_GenCardMarketPriceListGhostBlack[cardSaveIndex].GetMarketPrice(cardSaveIndex, cardData.cardGrade);
        }
        if (cardData.expansionType == ECardExpansionType.Megabot)
        {
            return m_GenCardMarketPriceListMegabot[cardSaveIndex].GetMarketPrice(cardSaveIndex, cardData.cardGrade);
        }
        if (cardData.expansionType == ECardExpansionType.FantasyRPG)
        {
            return m_GenCardMarketPriceListFantasyRPG[cardSaveIndex].GetMarketPrice(cardSaveIndex, cardData.cardGrade);
        }
        if (cardData.expansionType == ECardExpansionType.CatJob)
        {
            return m_GenCardMarketPriceListCatJob[cardSaveIndex].GetMarketPrice(cardSaveIndex, cardData.cardGrade);
        }
        return 0f;
    }

    public static float GetCardMarketPrice(int index, ECardExpansionType expansionType, bool isDestiny, int cardGrade)
    {
        switch (expansionType)
        {
            case ECardExpansionType.Tetramon:
                return m_GenCardMarketPriceList[index].GetMarketPrice(index, cardGrade);
            case ECardExpansionType.Destiny:
                return m_GenCardMarketPriceListDestiny[index].GetMarketPrice(index, cardGrade);
            case ECardExpansionType.Ghost:
                if (isDestiny)
                {
                    return m_GenCardMarketPriceListGhost[index].GetMarketPrice(index, cardGrade);
                }
                return m_GenCardMarketPriceListGhostBlack[index].GetMarketPrice(index, cardGrade);
            case ECardExpansionType.Megabot:
                return m_GenCardMarketPriceListMegabot[index].GetMarketPrice(index, cardGrade);
            case ECardExpansionType.FantasyRPG:
                return m_GenCardMarketPriceListFantasyRPG[index].GetMarketPrice(index, cardGrade);
            case ECardExpansionType.CatJob:
                return m_GenCardMarketPriceListCatJob[index].GetMarketPrice(index, cardGrade);
            default:
                return 0f;
        }
    }

    public static float GetCardMarketPriceCustomPercent(int index, ECardExpansionType expansionType, bool isDestiny, float percent, int cardGrade)
    {
        switch (expansionType)
        {
            case ECardExpansionType.Tetramon:
                return m_GenCardMarketPriceList[index].GetMarketPriceCustomPercent(percent, index, cardGrade);
            case ECardExpansionType.Destiny:
                return m_GenCardMarketPriceListDestiny[index].GetMarketPriceCustomPercent(percent, index, cardGrade);
            case ECardExpansionType.Ghost:
                if (isDestiny)
                {
                    return m_GenCardMarketPriceListGhost[index].GetMarketPriceCustomPercent(percent, index, cardGrade);
                }
                return m_GenCardMarketPriceListGhostBlack[index].GetMarketPriceCustomPercent(percent, index, cardGrade);
            case ECardExpansionType.Megabot:
                return m_GenCardMarketPriceListMegabot[index].GetMarketPriceCustomPercent(percent, index, cardGrade);
            case ECardExpansionType.FantasyRPG:
                return m_GenCardMarketPriceListFantasyRPG[index].GetMarketPriceCustomPercent(percent, index, cardGrade);
            case ECardExpansionType.CatJob:
                return m_GenCardMarketPriceListCatJob[index].GetMarketPriceCustomPercent(percent, index, cardGrade);
            default:
                return 0f;
        }
    }

    public static void AddCard(CardData cardData, int addAmount)
    {
        int cardSaveIndex = GetCardSaveIndex(cardData);
        if (cardData.cardGrade > 0)
        {
            CompactCardDataAmount compactCardDataAmount = new CompactCardDataAmount();
            compactCardDataAmount.cardSaveIndex = cardSaveIndex;
            compactCardDataAmount.amount = cardData.cardGrade;
            compactCardDataAmount.expansionType = cardData.expansionType;
            compactCardDataAmount.isDestiny = cardData.isDestiny;
            compactCardDataAmount.gradedCardIndex = m_GradedCardInventoryList.Count + 1;
            m_GradedCardInventoryList.Add(compactCardDataAmount);
        }
        else if (cardData.expansionType == ECardExpansionType.Tetramon)
        {
            m_CardCollectedList[cardSaveIndex] += addAmount;
            m_IsCardCollectedList[cardSaveIndex] = true;
        }
        else if (cardData.expansionType == ECardExpansionType.Destiny)
        {
            m_CardCollectedListDestiny[cardSaveIndex] += addAmount;
            m_IsCardCollectedListDestiny[cardSaveIndex] = true;
        }
        else if (cardData.expansionType == ECardExpansionType.Ghost)
        {
            if (cardData.isDestiny)
            {
                m_CardCollectedListGhostBlack[cardSaveIndex] += addAmount;
                m_IsCardCollectedListGhostBlack[cardSaveIndex] = true;
            }
            else
            {
                m_CardCollectedListGhost[cardSaveIndex] += addAmount;
                m_IsCardCollectedListGhost[cardSaveIndex] = true;
            }
        }
        else if (cardData.expansionType == ECardExpansionType.Megabot)
        {
            m_CardCollectedListMegabot[cardSaveIndex] += addAmount;
            m_IsCardCollectedListMegabot[cardSaveIndex] = true;
        }
        else if (cardData.expansionType == ECardExpansionType.FantasyRPG)
        {
            m_CardCollectedListFantasyRPG[cardSaveIndex] += addAmount;
            m_IsCardCollectedListFantasyRPG[cardSaveIndex] = true;
        }
        else if (cardData.expansionType == ECardExpansionType.CatJob)
        {
            m_CardCollectedListCatJob[cardSaveIndex] += addAmount;
            m_IsCardCollectedListCatJob[cardSaveIndex] = true;
        }
    }

    public static void RemoveGradedCard(CardData cardData, bool ignoreGradedCardIndex = false)
    {
        int cardSaveIndex = GetCardSaveIndex(cardData);
        for (int i = 0; i < m_GradedCardInventoryList.Count; i++)
        {
            if (m_GradedCardInventoryList[i].cardSaveIndex == cardSaveIndex && m_GradedCardInventoryList[i].amount == cardData.cardGrade && m_GradedCardInventoryList[i].expansionType == cardData.expansionType && m_GradedCardInventoryList[i].isDestiny == cardData.isDestiny)
            {
                if (ignoreGradedCardIndex)
                {
                    m_GradedCardInventoryList.RemoveAt(i);
                    break;
                }
                if (m_GradedCardInventoryList[i].gradedCardIndex == cardData.gradedCardIndex)
                {
                    m_GradedCardInventoryList.RemoveAt(i);
                    break;
                }
            }
        }
    }

    public static void ReduceCard(CardData cardData, int reduceAmount)
    {
        int cardSaveIndex = GetCardSaveIndex(cardData);
        if (cardData.expansionType == ECardExpansionType.Tetramon)
        {
            m_CardCollectedList[cardSaveIndex] -= reduceAmount;
        }
        else if (cardData.expansionType == ECardExpansionType.Destiny)
        {
            m_CardCollectedListDestiny[cardSaveIndex] -= reduceAmount;
        }
        else if (cardData.expansionType == ECardExpansionType.Ghost)
        {
            if (cardData.isDestiny)
            {
                m_CardCollectedListGhostBlack[cardSaveIndex] -= reduceAmount;
            }
            else
            {
                m_CardCollectedListGhost[cardSaveIndex] -= reduceAmount;
            }
        }
        else if (cardData.expansionType == ECardExpansionType.Megabot)
        {
            m_CardCollectedListMegabot[cardSaveIndex] -= reduceAmount;
        }
        else if (cardData.expansionType == ECardExpansionType.FantasyRPG)
        {
            m_CardCollectedListFantasyRPG[cardSaveIndex] -= reduceAmount;
        }
        else if (cardData.expansionType == ECardExpansionType.CatJob)
        {
            m_CardCollectedListCatJob[cardSaveIndex] -= reduceAmount;
        }
    }

    public static void ReduceCardUsingIndex(int index, ECardExpansionType expansionType, bool isDestiny, int reduceAmount)
    {
        switch (expansionType)
        {
            case ECardExpansionType.Tetramon:
                m_CardCollectedList[index] -= reduceAmount;
                break;
            case ECardExpansionType.Destiny:
                m_CardCollectedListDestiny[index] -= reduceAmount;
                break;
            case ECardExpansionType.Ghost:
                if (isDestiny)
                {
                    m_CardCollectedListGhostBlack[index] -= reduceAmount;
                }
                else
                {
                    m_CardCollectedListGhost[index] -= reduceAmount;
                }
                break;
            case ECardExpansionType.Megabot:
                m_CardCollectedListMegabot[index] -= reduceAmount;
                break;
            case ECardExpansionType.FantasyRPG:
                m_CardCollectedListFantasyRPG[index] -= reduceAmount;
                break;
            case ECardExpansionType.CatJob:
                m_CardCollectedListCatJob[index] -= reduceAmount;
                break;
        }
    }

    public static void SetCard(CardData cardData, int amount)
    {
        int cardSaveIndex = GetCardSaveIndex(cardData);
        if (cardData.expansionType == ECardExpansionType.Tetramon)
        {
            m_CardCollectedList[cardSaveIndex] = amount;
        }
        else if (cardData.expansionType == ECardExpansionType.Destiny)
        {
            m_CardCollectedListDestiny[cardSaveIndex] = amount;
        }
        else if (cardData.expansionType == ECardExpansionType.Ghost)
        {
            if (cardData.isDestiny)
            {
                m_CardCollectedListGhostBlack[cardSaveIndex] = amount;
            }
            else
            {
                m_CardCollectedListGhost[cardSaveIndex] = amount;
            }
        }
        else if (cardData.expansionType == ECardExpansionType.Megabot)
        {
            m_CardCollectedListMegabot[cardSaveIndex] = amount;
        }
        else if (cardData.expansionType == ECardExpansionType.FantasyRPG)
        {
            m_CardCollectedListFantasyRPG[cardSaveIndex] = amount;
        }
        else if (cardData.expansionType == ECardExpansionType.CatJob)
        {
            m_CardCollectedListCatJob[cardSaveIndex] = amount;
        }
    }

    public static CardData GetCardData(int cardIndex, ECardExpansionType expansionType, bool isDestiny)
    {
        return new CardData
        {
            monsterType = GetMonsterTypeFromCardSaveIndex(cardIndex, expansionType),
            isFoil = (cardIndex % GetCardAmountPerMonsterType(expansionType) >= GetCardAmountPerMonsterType(expansionType, includeFoilCount: false)),
            borderType = (ECardBorderType)(cardIndex % GetCardAmountPerMonsterType(expansionType, includeFoilCount: false)),
            isDestiny = isDestiny,
            expansionType = expansionType
        };
    }

    public static CardData GetGradedCardData(CompactCardDataAmount compactCardData)
    {
        return new CardData
        {
            monsterType = GetMonsterTypeFromCardSaveIndex(compactCardData.cardSaveIndex, compactCardData.expansionType),
            isFoil = (compactCardData.cardSaveIndex % GetCardAmountPerMonsterType(compactCardData.expansionType) >= GetCardAmountPerMonsterType(compactCardData.expansionType, includeFoilCount: false)),
            borderType = (ECardBorderType)(compactCardData.cardSaveIndex % GetCardAmountPerMonsterType(compactCardData.expansionType, includeFoilCount: false)),
            isDestiny = compactCardData.isDestiny,
            expansionType = compactCardData.expansionType,
            cardGrade = compactCardData.amount,
            gradedCardIndex = compactCardData.gradedCardIndex
        };
    }

    public static bool IsSameGradedCard(CompactCardDataAmount cardDataA, CompactCardDataAmount cardDataB)
    {
        if (cardDataA.cardSaveIndex == cardDataB.cardSaveIndex && cardDataA.expansionType == cardDataB.expansionType && cardDataA.isDestiny == cardDataB.isDestiny && cardDataA.amount == cardDataB.amount)
        {
            return true;
        }
        return false;
    }

    public static bool HasGradedCardInAlbum(CardData cardData)
    {
        for (int i = 0; i < m_GradedCardInventoryList.Count; i++)
        {
            CardData gradedCardData = GetGradedCardData(m_GradedCardInventoryList[i]);
            int cardSaveIndex = GetCardSaveIndex(gradedCardData);
            int cardSaveIndex2 = GetCardSaveIndex(cardData);
            if (gradedCardData.cardGrade == cardData.cardGrade && gradedCardData.expansionType == cardData.expansionType && cardSaveIndex == cardSaveIndex2)
            {
                return true;
            }
        }
        return false;
    }

    public static void AddChampionCard(EMonsterType monsterType)
    {
        m_ChampionCardCollectedList.Add((int)monsterType);
    }

    public static void AddSpecialEventCard(EMonsterType monsterType)
    {
        if (monsterType >= EMonsterType.None)
        {
            Debug.LogError("monstertype should be negative for special event");
        }
        else
        {
            m_ChampionCardCollectedList.Add((int)monsterType);
        }
    }

    public static string GetCardBorderName(ECardBorderType cardBorder)
    {
        string result = LocalizationManager.GetTranslation("Basic");
        switch (cardBorder)
        {
            case ECardBorderType.FirstEdition:
                result = LocalizationManager.GetTranslation("First Edition");
                break;
            case ECardBorderType.Silver:
                result = LocalizationManager.GetTranslation("Silver Edition");
                break;
            case ECardBorderType.Gold:
                result = LocalizationManager.GetTranslation("Gold Edition");
                break;
            case ECardBorderType.EX:
                result = "EX";
                break;
            case ECardBorderType.FullArt:
                result = LocalizationManager.GetTranslation("Full Art");
                break;
        }
        return result;
    }

    public static string GetFullCardTypeName(CardData cardData, bool ignoreRarity = false)
    {
        string rarityName = InventoryBase.GetMonsterData(cardData.monsterType).GetRarityName();
        if (cardData.isFoil)
        {
            if (ignoreRarity)
            {
                return GetCardBorderName(cardData.GetCardBorderType()) + " " + LocalizationManager.GetTranslation("Foil");
            }
            return GetCardBorderName(cardData.GetCardBorderType()) + " " + rarityName + " " + LocalizationManager.GetTranslation("Foil");
        }
        if (ignoreRarity)
        {
            return GetCardBorderName(cardData.GetCardBorderType());
        }
        return GetCardBorderName(cardData.GetCardBorderType()) + " " + rarityName;
    }

    public static int GetCardAmount(CardData cardData)
    {
        int cardSaveIndex = GetCardSaveIndex(cardData);
        return GetCardCollectedList(cardData.expansionType, cardData.isDestiny)[cardSaveIndex];
    }

    public static int GetCardAmountByIndex(int index, ECardExpansionType cardExpansionType, bool isDimensionCard)
    {
        return GetCardCollectedList(cardExpansionType, isDimensionCard)[index];
    }

    public static int GetCardFameAmount(CardData cardData)
    {
        int num = 1;
        ERarity rarity = InventoryBase.GetMonsterData(cardData.monsterType).Rarity;
        if (cardData.GetCardBorderType() == ECardBorderType.Base)
        {
            num = 1;
        }
        else if (cardData.GetCardBorderType() == ECardBorderType.FirstEdition)
        {
            num = 5;
        }
        else if (cardData.GetCardBorderType() == ECardBorderType.Silver)
        {
            num = 10;
        }
        else if (cardData.GetCardBorderType() == ECardBorderType.Gold)
        {
            num = 20;
        }
        else if (cardData.GetCardBorderType() == ECardBorderType.EX)
        {
            num = 50;
        }
        else if (cardData.GetCardBorderType() == ECardBorderType.FullArt)
        {
            num = 200;
        }
        switch (rarity)
        {
            case ERarity.Common:
                num = num;
                break;
            case ERarity.Rare:
                num += 5;
                break;
            case ERarity.Epic:
                num += 15;
                break;
            case ERarity.Legendary:
                num += 40;
                break;
        }
        if (cardData.expansionType == ECardExpansionType.Tetramon)
        {
            num = num;
        }
        else if (cardData.expansionType == ECardExpansionType.Destiny)
        {
            num = 9 + Mathf.RoundToInt((float)num * 1.25f);
        }
        else if (cardData.expansionType == ECardExpansionType.Ghost)
        {
            num = 18 + Mathf.RoundToInt((float)num * 2f);
        }
        else if (cardData.expansionType == ECardExpansionType.Megabot)
        {
            num = 13 + Mathf.RoundToInt((float)num * 1.4f);
        }
        else if (cardData.expansionType == ECardExpansionType.FantasyRPG)
        {
            num = 13 + Mathf.RoundToInt((float)num * 1.1f);
        }
        else if (cardData.expansionType == ECardExpansionType.CatJob)
        {
            num = 1 + Mathf.RoundToInt((float)num * 1.02f);
        }
        num = Mathf.RoundToInt((float)num * (1f + (float)cardData.GetCardBorderType() * 0.25f));
        if (cardData.isFoil)
        {
            num = (int)(num + ((int)(cardData.GetCardBorderType() + 1) * 5 * (int)(cardData.GetCardBorderType() + 1) + cardData.GetCardBorderType()));
            num = Mathf.RoundToInt((float)num * 2.5f);
        }
        return num;
    }

    public static bool IsCardUnlocked(ECardExpansionType cardExpansionType, int index, bool isDimensionCard)
    {
        return GetCardCollectedList(cardExpansionType, isDimensionCard)[index] >= 1;
    }

    public static bool HasEnoughCardAmount(ECardExpansionType cardExpansionType, int index, int amount, bool isDimensionCard)
    {
        return GetCardCollectedList(cardExpansionType, isDimensionCard)[index] >= amount;
    }

    public static int GetTotalCardCollectedAmount()
    {
        return 0 + GetCardCollectedAmount(ECardExpansionType.Tetramon, isDimensionCard: false) + GetCardCollectedAmount(ECardExpansionType.Destiny, isDimensionCard: true) + GetCardCollectedAmount(ECardExpansionType.Ghost, isDimensionCard: false) + GetCardCollectedAmount(ECardExpansionType.Ghost, isDimensionCard: true) + GetCardCollectedAmount(ECardExpansionType.Megabot, isDimensionCard: false) + GetCardCollectedAmount(ECardExpansionType.FantasyRPG, isDimensionCard: false) + GetCardCollectedAmount(ECardExpansionType.CatJob, isDimensionCard: false);
    }

    public static int GetCardCollectedAmount(ECardExpansionType cardExpansionType, bool isDimensionCard)
    {
        int num = 0;
        int num2 = InventoryBase.GetShownMonsterList(cardExpansionType).Count * GetCardAmountPerMonsterType(cardExpansionType);
        for (int i = 0; i < num2; i++)
        {
            if (GetCardCollectedList(cardExpansionType, isDimensionCard)[i] >= 1)
            {
                num++;
            }
        }
        return num;
    }

    public static float GetCardAlbumTotalValue(ECardExpansionType cardExpansionType, bool isDimensionCard)
    {
        float num = 0f;
        for (int i = 0; i < GetCardCollectedList(cardExpansionType, isDimensionCard).Count; i++)
        {
            num += GetCardMarketPrice(i, cardExpansionType, isDimensionCard, 0) * (float)GetCardCollectedList(cardExpansionType, isDimensionCard)[i];
        }
        return num;
    }

    public static BillData GetBill(EBillType billType)
    {
        for (int i = 0; i < m_BillList.Count; i++)
        {
            if (m_BillList[i].billType == billType)
            {
                return m_BillList[i];
            }
        }
        BillData billData = new BillData();
        billData.billType = billType;
        m_BillList.Add(billData);
        return billData;
    }

    public static void UpdateBill(EBillType billType, int dueDayChange, float amountToPayChange)
    {
        for (int i = 0; i < m_BillList.Count; i++)
        {
            if (m_BillList[i].billType == billType)
            {
                if (amountToPayChange > 0f || m_BillList[i].billDayPassed > 0)
                {
                    m_BillList[i].billDayPassed += dueDayChange;
                    m_BillList[i].amountToPay += amountToPayChange;
                }
                break;
            }
        }
    }

    public static void SetBill(EBillType billType, int dayPassed, float amountToPay)
    {
        for (int i = 0; i < m_BillList.Count; i++)
        {
            if (m_BillList[i].billType == billType)
            {
                m_BillList[i].billDayPassed = dayPassed;
                m_BillList[i].amountToPay = amountToPay;
                break;
            }
        }
    }

    public static void AddDecoItemToInventory(EDecoObject decoObject, int count)
    {
        m_DecorationInventoryList[(int)decoObject] += count;
    }

    public static void RemoveDecoItemFromInventory(EDecoObject decoObject, int count)
    {
        m_DecorationInventoryList[(int)decoObject] -= count;
    }

    public static int GetDecoItemInventoryCount(EDecoObject decoObject)
    {
        return m_DecorationInventoryList[(int)decoObject];
    }

    public static void SetUnlockDecoWall(int decoIndex, bool isUnlocked)
    {
        m_UnlockedDecoWallList[decoIndex] = isUnlocked;
    }

    public static bool IsDecoWallUnlocked(int decoIndex)
    {
        return m_UnlockedDecoWallList[decoIndex];
    }

    public static void SetUnlockDecoFloor(int decoIndex, bool isUnlocked)
    {
        m_UnlockedDecoFloorList[decoIndex] = isUnlocked;
    }

    public static bool IsDecoFloorUnlocked(int decoIndex)
    {
        return m_UnlockedDecoFloorList[decoIndex];
    }

    public static void SetUnlockDecoCeiling(int decoIndex, bool isUnlocked)
    {
        m_UnlockedDecoCeilingList[decoIndex] = isUnlocked;
    }

    public static bool IsDecoCeilingUnlocked(int decoIndex)
    {
        return m_UnlockedDecoCeilingList[decoIndex];
    }

    private void OnEnable()
    {
        if (Application.isPlaying || Application.isMobilePlatform)
        {
            CEventManager.AddListener<CEventPlayer_SetCoin>(CPlayer_OnSetCoin);
            CEventManager.AddListener<CEventPlayer_AddCoin>(CPlayer_OnAddCoin);
            CEventManager.AddListener<CEventPlayer_ReduceCoin>(CPlayer_OnReduceCoin);
            CEventManager.AddListener<CEventPlayer_AddFame>(CPlayer_OnAddFame);
            CEventManager.AddListener<CEventPlayer_SetFame>(CPlayer_OnSetFame);
            CEventManager.AddListener<CEventPlayer_AddShopExp>(CPlayer_OnAddShopExp);
            CEventManager.AddListener<CEventPlayer_SetShopExp>(CPlayer_OnSetShopExp);
            CEventManager.AddListener<CEventPlayer_ChangeScene>(CPlayer_OnChangeScene);
        }
    }

    private void OnDisable()
    {
        if (Application.isPlaying || Application.isMobilePlatform)
        {
            CEventManager.RemoveListener<CEventPlayer_SetCoin>(CPlayer_OnSetCoin);
            CEventManager.RemoveListener<CEventPlayer_AddCoin>(CPlayer_OnAddCoin);
            CEventManager.RemoveListener<CEventPlayer_ReduceCoin>(CPlayer_OnReduceCoin);
            CEventManager.RemoveListener<CEventPlayer_AddFame>(CPlayer_OnAddFame);
            CEventManager.RemoveListener<CEventPlayer_SetFame>(CPlayer_OnSetFame);
            CEventManager.RemoveListener<CEventPlayer_AddShopExp>(CPlayer_OnAddShopExp);
            CEventManager.RemoveListener<CEventPlayer_SetShopExp>(CPlayer_OnSetShopExp);
            CEventManager.RemoveListener<CEventPlayer_ChangeScene>(CPlayer_OnChangeScene);
        }
    }

    private void CPlayer_OnChangeScene(CEventPlayer_ChangeScene evt)
    {
        m_CurrentLevelName = evt.m_SceneName;
    }

    private void CPlayer_OnAddCoin(CEventPlayer_AddCoin evt)
    {
        m_CoinAmount += evt.m_CoinValue;
        m_CoinAmountDouble += evt.m_CoinValue;
        if (GameInstance.GetCurrencyConversionRate() > 1f)
        {
            m_CoinAmountDouble = Math.Round(m_CoinAmountDouble, 3, MidpointRounding.AwayFromZero);
        }
        else
        {
            m_CoinAmountDouble = Math.Round(m_CoinAmountDouble, 2, MidpointRounding.AwayFromZero);
        }
        if (m_CoinAmount < -100000000f)
        {
            m_CoinAmount = 0f;
            CEventManager.QueueEvent(new CEventPlayer_SetCoin(0f, 0.0));
        }
    }

    private void CPlayer_OnReduceCoin(CEventPlayer_ReduceCoin evt)
    {
        m_CoinAmount -= evt.m_CoinValue;
        m_CoinAmountDouble -= evt.m_CoinValue;
        if (GameInstance.GetCurrencyConversionRate() > 1f)
        {
            m_CoinAmountDouble = Math.Round(m_CoinAmountDouble, 3, MidpointRounding.AwayFromZero);
        }
        else
        {
            m_CoinAmountDouble = Math.Round(m_CoinAmountDouble, 2, MidpointRounding.AwayFromZero);
        }
    }

    private void CPlayer_OnSetCoin(CEventPlayer_SetCoin evt)
    {
        m_CoinAmount = evt.m_CoinValue;
        m_CoinAmountDouble = evt.m_CoinValueDouble;
        if (GameInstance.GetCurrencyConversionRate() > 1f)
        {
            m_CoinAmountDouble = Math.Round(m_CoinAmountDouble, 3, MidpointRounding.AwayFromZero);
        }
        else
        {
            m_CoinAmountDouble = Math.Round(m_CoinAmountDouble, 2, MidpointRounding.AwayFromZero);
        }
    }

    private void CPlayer_OnAddFame(CEventPlayer_AddFame evt)
    {
        m_FamePoint += evt.m_FameValue;
        m_FamePoint = Mathf.Clamp(m_FamePoint, 0, 99999999);
        m_TotalFameAdd += evt.m_FameValue;
    }

    private void CPlayer_OnSetFame(CEventPlayer_SetFame evt)
    {
        m_FamePoint = evt.m_FameValue;
        m_FamePoint = Mathf.Clamp(m_FamePoint, 0, 99999999);
    }

    private void CPlayer_OnAddShopExp(CEventPlayer_AddShopExp evt)
    {
        m_ShopExpPoint += evt.m_ExpValue;
        m_GameReportDataCollect.storeExpGained += evt.m_ExpValue;
        m_GameReportDataCollectPermanent.storeExpGained += evt.m_ExpValue;
        bool flag = false;
        while (m_ShopExpPoint >= GetExpRequiredToLevelUp())
        {
            m_ShopExpPoint -= GetExpRequiredToLevelUp();
            m_ShopLevel++;
            m_GameReportDataCollect.storeLevelGained++;
            m_GameReportDataCollectPermanent.storeLevelGained++;
            flag = true;
            TutorialManager.AddTaskValue(ETutorialTaskCondition.ShopLevel, 1f);
        }
        if (flag)
        {
            CEventManager.QueueEvent(new CEventPlayer_ShopLeveledUp(m_ShopLevel));
        }
    }

    private void CPlayer_OnSetShopExp(CEventPlayer_SetShopExp evt)
    {
        m_ShopExpPoint = evt.m_ExpValue;
        bool flag = false;
        while (m_ShopExpPoint >= GetExpRequiredToLevelUp())
        {
            m_ShopExpPoint -= GetExpRequiredToLevelUp();
            m_ShopLevel++;
            flag = true;
        }
        if (flag)
        {
            CEventManager.QueueEvent(new CEventPlayer_ShopLeveledUp(m_ShopLevel));
        }
    }

    public static bool CheckAuthGameID()
    {
        if (m_AuthGameID == "AS394nGOA@laen083KAWmp439-24905we#*%&^jawr84jiR#(JIOI#TRQ*(#)TR*(Q#INTRHereaaerea")
        {
            return true;
        }
        return false;
    }
}
