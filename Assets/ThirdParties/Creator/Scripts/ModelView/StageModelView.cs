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


public class StageModelView : MonoBehaviour, IInitializable
{
    private MasterModel m_Model;

    public void Initialize()
    {
        m_Model = GameManager.Instance.GetMasterData();
    }
}
