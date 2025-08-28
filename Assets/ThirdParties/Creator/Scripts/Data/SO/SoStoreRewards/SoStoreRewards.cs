using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SoStoreRewards", order = 1)]
public class SoStoreRewards : ScriptableObject
{
    [System.Serializable]
    public class RewardData
    {
        public int amount;
        public InventoryItem[] rewards;

        public RewardData()
        {
            amount = 0;
            rewards = new InventoryItem[0];
        }
    }

    public RewardData[] datas;

    public int GetAmount(int index)
    {
        if (index < datas.Length)
        {
            return datas[index].amount;
        }
        return Int16.MaxValue;
    }

    public InventoryItem[] GetRewardData(int index)
    {
        if (index < datas.Length)
        {
            return datas[index].rewards;
        }
        return new InventoryItem[0];
    }
}
