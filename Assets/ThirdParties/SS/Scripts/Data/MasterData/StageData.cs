using System;
using DesignPatterns;
using LibraryGame;
using UnityEngine;


[System.Serializable]
public class StageData : MasterDataBase
{
    public StageData(int vaule, MasterDataType type) : base(vaule, type)
    {
        this.type = type;
        this.m_value = vaule;
    }
}
