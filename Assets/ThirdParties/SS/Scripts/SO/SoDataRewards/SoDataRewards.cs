using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Purchasing;
using TMPro;
using com.cyborgAssets.inspectorButtonPro;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SoDataRewards", order = 1)]
public class SoDataRewards : ScriptableObject
{
    public List<CointShop> cointConfig = new List<CointShop>();

    public List<AdsShop> adsConfig = new List<AdsShop>();

    public CointShop GetItemCoin(CointPack pack)
    {
        return cointConfig.Find(x => x.pack == pack);
    }

    public AdsShop GetAdsShop(AdsPack pack)
    {
        return adsConfig.Find(x => x.pack == pack);
    }
}


[System.Serializable]
public class ItemShopData
{
    public int vaule;
    public MasterDataType type;

    public ItemShopData(int vaule, MasterDataType type)
    {
        this.vaule = vaule;
        this.type = type;
    }

    public ItemShopData()
    {

    }
}

[System.Serializable]
public class CointShop
{
    public CointPack pack;
    public int vaule;
    public List<ItemShopData> data;

    public void SetData()
    {
        if (data != null)
        {
            if (data.Count > 0)
            {
                List<ItemShopData> dataReward = new List<ItemShopData>();
                dataReward.AddRange(data);

                CongratulationRewardData rewardData = new CongratulationRewardData(data, pack.ToString());
                rewardData.OnReward(1);
            }
        }
    }
}

[System.Serializable]
public class AdsShop
{
    public AdsPack pack;
    public List<ItemShopData> data;

    public void SetData(UnityAction onSuccess = null, UnityAction onFail = null, UnityAction onCompleted = null)
    {
        GameManager.Instance.GetAdsPresenter().ShowRewardedVideo(pack.ToString(), OnSuccess, OnFail, OnCompleted);

        void OnSuccess()
        {
            Rewards();
            onSuccess?.Invoke();
        }

        void OnFail()
        {
            onFail?.Invoke();
        }

        void OnCompleted()
        {
            onCompleted?.Invoke();
        }

        void Rewards()
        {
            if (data != null)
            {
                if (data.Count > 0)
                {
                    List<ItemShopData> dataReward = new List<ItemShopData>();
                    dataReward.AddRange(data);

                    CongratulationRewardData rewardData = new CongratulationRewardData(data, pack.ToString());
                    rewardData.OnReward(1);
                }
            }
        }
    }
}

[System.Serializable]
public class InfoShopTextView
{
    public TextMeshProUGUI text;
    public bool abbrevation = false;
    public bool isCoint = false;
    public bool isPlus = false;

    public void View(ItemShopData itemShop)
    {
        if (text == false) return;
        if (itemShop.vaule <= 0)
        {
            text.gameObject.SetActive(false);
        }
        else
        {
            if (itemShop.type == MasterDataType.Money)
            {
                if (isCoint)
                {
                    if (abbrevation)
                    {
                        text.text = string.Format("{0} coins", AbbrevationUtility.AbbreviateNumber(itemShop.vaule));
                    }
                    else
                    {
                        text.text = string.Format("{0} coins", itemShop.vaule);
                    }
                }
                else
                {
                    if (abbrevation)
                    {
                        text.text = string.Format("{0}", AbbrevationUtility.AbbreviateNumber(itemShop.vaule));
                    }
                    else
                    {
                        text.text = string.Format("{0}", itemShop.vaule);
                    }
                }

            }
            else
            {
                if (isPlus)
                {
                    text.text = string.Format("+{0}", itemShop.vaule);
                }
                else
                {
                    text.text = string.Format("x{0}", itemShop.vaule);
                }
            }
        }
    }
}