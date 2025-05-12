using System;
using System.Collections.Generic;
using DesignPatterns;
using UnityEngine;
using LibraryGame;

[System.Serializable]
public class InfoQuest
{
    public int levelUnlock = 0;

    protected int m_MaxValue;

    public TypeQuest type;

    public InfoQuest(int maxValue, TypeQuest type)
    {
        this.type = type;
        this.m_MaxValue = maxValue;
    }

    protected virtual bool UnLock()
    {
        return GameManager.Instance.GetMasterData().dataStage.value >= levelUnlock;
    }

    public int vaule
    {
        get { return LibraryGameSave.LoadQuestData(type, "vaule", 0); }
        set { LibraryGameSave.SaveQuestData(type, "vaule", value); }
    }

    public virtual int maxValue
    {
        get { return m_MaxValue; }
        set { m_MaxValue = value; }
    }

    public virtual void SetData(int vaule)
    {
        if (UnLock())
        {
            int newVaule = Mathf.Clamp(this.vaule += vaule, 0, maxValue);
            this.vaule = newVaule;
        }
    }
}