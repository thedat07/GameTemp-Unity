using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;
using System.Linq;
using LibraryGame;

[System.Serializable]
public class ImageItemData
{
    public MasterDataType type;
    public Sprite[] sprites;
}

[System.Serializable]
public class InfoImageView
{
    public Image image;

    public void View(SoImageItem soImage, MasterDataType type, int amount)
    {
        image.sprite = soImage.GetImage(type, amount);
    }

    public void View(SoImageItem soImage, ItemShopData data)
    {
        image.sprite = soImage.GetImage(data.type, data.vaule);
    }

}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SoImageItem", order = 1)]
public class SoImageItem : ScriptableObject
{
    public ImageItemData[] datas;

    public Sprite GetImage(MasterDataType type, int count = 0)
    {
        try
        {
            return datas.First(x => x.type == type).sprites[0];
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
            return null;
        }
    }
}
