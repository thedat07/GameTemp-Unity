using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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

    public void View(SoImageItem soImage, InventoryItem data)
    {
        image.sprite = soImage.GetImage(data.GetDataType(), data.GetQuantity());
    }

}

[CreateAssetMenu(fileName = "Data", menuName = "Game/SoImageItem", order = 1)]
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
