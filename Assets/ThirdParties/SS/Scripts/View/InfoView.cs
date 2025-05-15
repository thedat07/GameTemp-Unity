using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class InfoViewData
{
    public MasterDataType type;
    public GameObject view;
    public InfoShopTextView textView;

    public void Show(InventoryItem data)
    {
        if (this.type == data.GetDataType())
        {
            view.SetActive(true);
            textView.View(data);
        }
    }

    public void Hide()
    {
        view.SetActive(false);
    }
}

[System.Serializable]
public class InfoViewRoot
{
    //public InfoViewData[] textView;
    public InfoView[] infoViews;

    private List<InfoViewData> infoViewDatas;

    private void Set()
    {
        if (infoViewDatas == null)
        {
            infoViewDatas = new List<InfoViewData>();
            //  infoViewDatas.AddRange(this.textView);
            infoViewDatas.AddRange(this.infoViews.Select(x => x.infoViewData));
        }
    }

    public void Init(List<InventoryItem> data)
    {
        Set();
        UpdateView(data);
    }

    public void Init(CointInfoPack itemShop)
    {
        Init(itemShop.data);
    }

    public void Init(AdsInfoPack itemShop)
    {
        Init(itemShop.data);
    }

    private void UpdateView(List<InventoryItem> dataList)
    {
        foreach (var item in infoViewDatas)
        {
            item.Hide();
        }

        foreach (var item in infoViewDatas)
        {
            var match = dataList.Find(x => x.GetDataType() == item.type);
            if (match != null)
            {
                item.Show(match);
            }
        }
    }
}

public class InfoView : MonoBehaviour
{
    public InfoViewData infoViewData;
}
