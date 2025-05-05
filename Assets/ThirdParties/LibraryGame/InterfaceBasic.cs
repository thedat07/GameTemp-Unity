using UnityEngine;

interface ISwitchable
{
    void TurnOn();
    void TurnOff();
}

public interface IPresenter
{
    void Init();
}
