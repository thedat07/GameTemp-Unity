using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/FeatureTSCSO", order = 1)]
public class FeatureTSCSO : ScriptableObject
{
    [System.Serializable]
    public class RewardData
    {
        public int amount;
        public InventoryItem[] reward;
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
}
