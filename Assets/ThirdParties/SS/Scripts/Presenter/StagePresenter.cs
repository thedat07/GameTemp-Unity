using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections;
using System.Linq;
using UnityEngine.Events;


[System.Serializable]
public class StageFirebase
{
    public List<int> levelGame;

    public StageFirebase()
    {
        levelGame = new List<int>();
    }
}


public class StagePresenter : MonoBehaviour
{
    private MasterData m_Data;

    public void Init()
    {
        m_Data = GameManager.Instance.GetMasterData();
    }
}
