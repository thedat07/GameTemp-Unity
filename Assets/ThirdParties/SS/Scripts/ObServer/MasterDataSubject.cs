using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPatterns;

public interface IView
{
    void UpdateView();
}

public class MasterDataSubject : MonoBehaviour, IView
{
    protected MasterData m_MasterData;

    void Start()
    {
        m_MasterData = GameManager.Instance.GetMasterData();
        TigerForge.EventManager.StartListening(MasterData.Key, OnNotify);
        Init();
        UpdateView();
    }

    protected virtual void Init()
    {

    }

    void OnNotify()
    {
        UpdateView();
    }

    public virtual void UpdateView()
    {

    }

    void OnDestroy()
    {
        Destroy();
        TigerForge.EventManager.StopListening(MasterData.Key, OnNotify);
    }

    protected virtual void Destroy()
    {

    }
}
