using System.Collections.Generic;
using Directory;
using System.Linq;
using UnityEngine.Events;

public class CongratulationRewardData
{
    public List<ItemShopData> data;
    public string log;
    public UnityAction callback;

    public CongratulationRewardData(List<ItemShopData> data, string log = "", UnityAction callback = null)
    {
        this.data = new List<ItemShopData>();
        this.data.AddRange(data);

        this.log = log;
        this.callback = callback;
    }

    public void OnReward(int xData = 1)
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
        }

        callback?.Invoke();
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