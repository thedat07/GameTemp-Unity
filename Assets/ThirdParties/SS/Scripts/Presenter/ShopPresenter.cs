using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using SS.View;

public class ShopPresenter : MonoBehaviour
{
    public SoShop soShop;

    public const string Key = "ShopPresenter";

    public void Buy(ItemShop dataItemShop, UnityAction pass, UnityAction error, UnityAction callBack = null)
    {
        GameManager.Instance.GetIAPPresenter().BuyItem(dataItemShop, (() => { AddDataBuy(dataItemShop, callBack); pass?.Invoke(); }, () => { error?.Invoke(); }));
    }

    public void AddDataBuy(ItemShop dataItemShop, UnityAction callBack = null)
    {
        if (dataItemShop.productType == UnityEngine.Purchasing.ProductType.Consumable)
        {
            var dataBuy = soShop.GetNameItemShop(dataItemShop.GetIDStore()).data;
            if (dataBuy.Count > 0)
            {
                OnReward(dataBuy.ToList(), dataItemShop.GetIDStore(), null, callBack);
            }
        }
        else
        {
            if (LibraryGame.LibraryGameSave.LoadShopData(dataItemShop.GetIDStore(), false) == false)
            {
                var dataBuy = soShop.GetNameItemShop(dataItemShop.GetIDStore()).data;
                OnReward(dataBuy.ToList(), dataItemShop.GetIDStore(), null, callBack);
                LibraryGame.LibraryGameSave.SaveShopData(dataItemShop.GetIDStore(), true);
            }
        }
    }

    public void AddDataRestore(ItemShop dataItemShop)
    {
        if (LibraryGame.LibraryGameSave.LoadShopData(dataItemShop.GetIDStore(), false) == false)
        {
            var dataBuy = soShop.GetNameItemShop(dataItemShop.GetIDStore()).data;
            RestorePurchases(new PopupCongratulationRewardData(dataBuy.ToList(), "Restore"));
            LibraryGame.LibraryGameSave.SaveShopData(dataItemShop.GetIDStore(), true);
            TigerForge.EventManager.EmitEvent(Key);
        }
    }

    public void AddDataRestore(string id)
    {
        ItemShop dataBuy = soShop.GetNameItemShop(id);
        if (dataBuy != null)
        {
            if (dataBuy.productType == UnityEngine.Purchasing.ProductType.NonConsumable)
            {
                AddDataRestore(dataBuy);
            }
        }
    }

    private void RestorePurchases(PopupCongratulationRewardData m_Data)
    {
        List<ItemShopData> dataConvert = Convert(m_Data);
        if (dataConvert.Count > 0)
        {
            foreach (var item in dataConvert)
            {
                if (item.type == MasterDataType.NoAds)
                {
                    GameManager.Instance.GetAdsPresenter().OnRemoveAds();
                }
                else
                {
                    GameManager.Instance.GetMasterPresenter().AddData(item.vaule, item.type, m_Data.log);
                }
            }
        }
    }

    private List<ItemShopData> Convert(PopupCongratulationRewardData m_Data)
    {
        if (m_Data.data != null)
        {
            var groupedByType = m_Data.data
             .GroupBy(item => item.type)
             .Select(group => new ItemShopData
             {
                 vaule = group.Sum(item => item.vaule),
                 type = group.Key
             })
             .ToList();
            return groupedByType;
        }
        else
        {
            return new List<ItemShopData>();
        }
    }

    private void OnReward(List<ItemShopData> data, string log = "", UnityAction action = null, UnityAction callback = null)
    {
        PopupCongratulationRewardData rewardData = new PopupCongratulationRewardData(data, log, action, false, callback);
        rewardData.OnReward(1);
        TigerForge.EventManager.EmitEvent(Key);
    }


    public bool OnBuyCoin(CointPack cointPack, UnityAction onSuccess, UnityAction onFaill, string log)
    {
        CointShop cointShop = soShop.GetItemCoin(cointPack);

        if (cointShop != null)
        {
            GameManager.Instance.GetMasterPresenter().EditMoney(cointShop.vaule, () =>
            {
                onSuccess?.Invoke();
            }, () =>
            {
                onFaill?.Invoke();
            }, log);

            return true;
        }
        else
        {
            onFaill?.Invoke();
        }

        return false;
    }

    public bool IsBuy(string id)
    {
        if (LibraryGame.LibraryGameSave.LoadShopData(id, false) == false)
        {
            return false;
        }
        return true;
    }
}
