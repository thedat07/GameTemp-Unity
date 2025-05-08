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

    public void Show(ItemShopData data)
    {
        if (this.type == data.type)
        {
            view.SetActive(true);
            textView.View(data);
        }
    }
}

[System.Serializable]
public class InfoViewRoot
{
    public InfoViewData[] textView;

    public InfoView[] infoViews;

    private List<InfoViewData> infoViewDatas = new List<InfoViewData>();

    private void Set()
    {
        infoViewDatas = new List<InfoViewData>();
        infoViewDatas.AddRange(this.textView);
        infoViewDatas.AddRange(this.infoViews.Select(x => x.infoViewData));
    }

    public void Init(List<ItemShopData> data)
    {
        Set();

        foreach (var item in infoViewDatas)
        {
            item.view.SetActive(false);
        }

        foreach (var item in infoViewDatas)
        {
            var getData = data.Find(x => x.type == item.type);
            if (getData != null)
            {
                item.Show(getData);
            }
        }
    }

    public void Init(CointShop itemShop)
    {
        Set();

        foreach (var item in infoViewDatas)
        {
            item.view.SetActive(false);
        }

        foreach (var item in infoViewDatas)
        {
            var getData = itemShop.data.Find(x => x.type == item.type);
            if (getData != null)
            {
                item.Show(getData);
            }
        }
    }

    public void Init(AdsShop itemShop)
    {
        Set();

        foreach (var item in infoViewDatas)
        {
            item.view.SetActive(false);
        }

        foreach (var item in infoViewDatas)
        {
            var getData = itemShop.data.Find(x => x.type == item.type);
            if (getData != null)
            {
                item.Show(getData);
            }
        }
    }
}

public class InfoView : MonoBehaviour
{
    public InfoViewData infoViewData;
}
