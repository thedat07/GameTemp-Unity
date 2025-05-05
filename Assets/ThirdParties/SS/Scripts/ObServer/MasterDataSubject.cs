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

    protected virtual void Start()
    {
        m_MasterData = GameManager.Instance.GetMasterData();
        TigerForge.EventManager.StartListening(MasterData.Key, OnNotify);
        Init();
        UpdateView();
    }

    protected virtual void Init()
    {

    }

    public virtual void OnNotify()
    {
        UpdateView();
    }

    public virtual void UpdateView()
    {

    }

    protected virtual void OnDestroy()
    {
        TigerForge.EventManager.StopListening(MasterData.Key, OnNotify);
    }
}
