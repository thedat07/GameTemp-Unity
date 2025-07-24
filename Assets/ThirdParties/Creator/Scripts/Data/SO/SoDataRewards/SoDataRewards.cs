using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

[CreateAssetMenu(fileName = "Data", menuName = "Game/SoDataRewards", order = 1)]
public class SoDataRewards : ScriptableObject
{
    public List<CointInfoPack> cointConfig = new List<CointInfoPack>();

    public List<AdsInfoPack> adsConfig = new List<AdsInfoPack>();

    public CointInfoPack GetItemCoin(ECointPack pack)
    {
        return cointConfig.Find(x => x.pack == pack);
    }

    public AdsInfoPack GetAdsShop(EAdsPack pack)
    {
        return adsConfig.Find(x => x.pack == pack);
    }
}
