using System;
using DesignPatterns;
using LibraryGame;
using UnityEngine;

[System.Serializable]
public class MasterDataBase
{
    protected int m_value;

    public MasterDataType type;

    public MasterDataBase(int value, MasterDataType type)
    {
        this.type = type;
        this.m_value = value;
    }

    public int value
    {
        get { return LibraryGameSave.LoadMasterData(type, "value", m_value); }
        set { LibraryGameSave.SaveMasterData(type, "value", value); }
    }

    public virtual void AddValue(int value)
    {
        int newValue = Mathf.Clamp(this.value + value, 0, int.MaxValue);
        this.value = newValue;
    }

    public virtual void SetValue(int value)
    {
        int newValue = Mathf.Clamp(value, 0, int.MaxValue);
        this.value = newValue;
    }
}
