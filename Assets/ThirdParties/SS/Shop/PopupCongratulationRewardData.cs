using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.View;
using System.Linq;
using UnityEngine.Events;

public class PopupCongratulationRewardData
{
    public List<ItemShopData> data;
    public string log;
    public UnityAction action;
    public UnityAction actionAds;
    public bool ads;

    public PopupCongratulationRewardData(List<ItemShopData> data, string log = "", UnityAction action = null, bool ads = true, UnityAction actionAds = null)
    {
        this.data = new List<ItemShopData>();
        this.data.AddRange(data);

        this.log = log;
        this.action = action;
        this.ads = ads;
        this.actionAds = actionAds;
    }

    public void OnReward(int xData, bool callback = false)
    {
        List<ItemShopData> dataConvert = Convert();

        if (dataConvert.Count > 0)
        {
            for (int i = 0; i < dataConvert.Count; i++)
            {
                if (dataConvert[i].type == MasterDataType.NoAds)
                {
                    GameManager.Instance.GetAdsPresenter().OnRemoveAds();
                }
                else
                {
                    int newVaule = dataConvert[i].vaule * xData;
                    GameManager.Instance.GetMasterPresenter().AddData(newVaule, dataConvert[i].type, log);
                }
            }
            Manager.Object.PlayEffect();
            action?.Invoke();
        }
    }

    private List<ItemShopData> Convert()
    {
        if (data != null)
        {
            var groupedByType = data
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

}
