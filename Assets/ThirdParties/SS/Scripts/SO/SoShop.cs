using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Purchasing;
using TMPro;
using com.cyborgAssets.inspectorButtonPro;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SoShop", order = 1)]
public class SoShop : ScriptableObject
{
    public List<ItemShop> purchaseConfig = new List<ItemShop>();
    public List<CointShop> cointConfig = new List<CointShop>();
    public List<AdsShop> adsConfig = new List<AdsShop>();

    public ItemShop GetItemShop(ShopPack pack)
    {
        return purchaseConfig.Find(x => x.pack == pack);
    }

    public ItemShop GetNameItemShop(string productName)
    {
        return purchaseConfig.Find(x => x.GetIDStore() == productName);
    }

    public void AddProduct(ConfigurationBuilder builder)
    {
        foreach (var item in purchaseConfig)
        {
            builder.AddProduct(item.GetIDStore(), item.productType);
        }
    }

    public CointShop GetItemCoin(CointPack pack)
    {
        return cointConfig.Find(x => x.pack == pack);
    }

    public AdsShop GetAdsShop(AdsPack pack)
    {
        return adsConfig.Find(x => x.pack == pack);
    }


    [ContextMenu("ViewId")]
    public void ViewId()
    {
        foreach (var item in purchaseConfig)
        {
            item.productIdStore = ConvertToSnakeCase(string.Format("{0}.{1}", Application.identifier, item.pack.ToString()));
            Debug.Log(ConvertToSnakeCase(string.Format("{0}.{1}", Application.identifier, item.pack.ToString())));
        }

        string ConvertToSnakeCase(string input)
        {
            return input.ToLower().Replace(" ", "_");
        }
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

                PopupCongratulationRewardData rewardData = new PopupCongratulationRewardData(data, pack.ToString());
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

    public void SetData(UnityAction action)
    {
        if (data != null)
        {
            if (data.Count > 0)
            {
                UnityEvent onCompleted = new UnityEvent();
                onCompleted.AddListener(() => { Rewards(); action?.Invoke(); });
                GameManager.Instance.GetAdsPresenter().ShowRewarded(pack.ToString(), onCompleted, null);
            }
        }

        void Rewards()
        {
            List<ItemShopData> dataReward = new List<ItemShopData>();
            dataReward.AddRange(data);

            PopupCongratulationRewardData rewardData = new PopupCongratulationRewardData(data, pack.ToString());
            rewardData.OnReward(1);
        }
    }
}


[System.Serializable]
public class ItemShop
{
    public ShopPack pack;
    public string productIdStore;
    public List<ItemShopData> data;
    public ProductType productType = ProductType.Consumable;

    public string GetIDStore()
    {
        if (productIdStore == "")
        {
            return pack.ToString();
        }
        else
        {
            return productIdStore;
        }
    }

    public string GetPrice()
    {
#if TESTIAP
        return string.Format("?", 0);
#else
        if (GetProduct() == null)
        {
            return string.Format("?");
        }
        else
        {
            return string.Format("{0}", GetProduct().metadata.localizedPriceString);
        }

#endif
    }

    public Product GetProduct()
    {
#if TESTIAP
        return null;
#else
        return GameManager.Instance.GetIAPPresenter().GetProduct(GetIDStore());
#endif
    }

    public bool CanBuy()
    {
        if (pack == ShopPack.RemoveAds)
        {
            return !GameManager.Instance.GetAdsData().isRemoveAds;
        }
        else
        {
            if (productType == ProductType.Consumable)
            {
                return true;
            }
            else
            {
                return !LibraryGame.LibraryGameSave.LoadShopData(GetIDStore(), false);
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
                        text.text = string.Format("{0} coins", LibraryGame.AbbrevationUtility.AbbreviateNumber(itemShop.vaule));
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
                        text.text = string.Format("{0}", LibraryGame.AbbrevationUtility.AbbreviateNumber(itemShop.vaule));
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