using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using System;

public class FacebookController : MonoBehaviour
{
    public static FacebookController instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

#if !TESTMODE

        try
        {
            if (!FB.IsInitialized)
            {
                FB.Init(InitCallback, OnHideUnity);
            }
            else
            {
                FB.ActivateApp();
            }

            FB.Mobile.SetAdvertiserTrackingEnabled(true);

        }
        catch (Exception e)
        {
            UnityEngine.Console.Log("FB", e);
        }
#endif

    }

    private void InitCallback()
    {

#if !TESTMODE
        try
        {
            if (FB.IsInitialized)
            {
                FB.ActivateApp();
            }
            else
            {
                UnityEngine.Console.Log("Facebook", "Failed to Initialize the Facebook SDK");
            }
        }
        catch (Exception e)
        {
            UnityEngine.Console.LogException(e);
        }
#endif
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void LogEvent(string eventName, string paramName, string paramValue, string paramName2, string paramValue2)
    {
#if !TESTMODE
        try
        {
            Dictionary<string, object> p = new Dictionary<string, object>();
            p.Add(paramName, paramValue);
            p.Add(paramName2, paramValue2);
            FB.LogAppEvent(
                eventName,
                parameters: p
            );
        }
        catch (Exception e)
        {
            UnityEngine.Console.LogException(e);
        }
#endif
    }

    public void LogEvent(string eventName, string paramName, string paramValue)
    {
#if !TESTMODE
        try
        {
            Dictionary<string, object> p = new Dictionary<string, object>();
            p.Add(paramName, paramValue);
            FB.LogAppEvent(
                eventName,
                parameters: p
            );
        }
        catch (Exception e)
        {
            UnityEngine.Console.LogException(e);
        }
#endif
    }

    public void LogEvent(string eventName)
    {
#if !TESTMODE
        try
        {
            FB.LogAppEvent(
                eventName
            );
        }
        catch (Exception e)
        {
            UnityEngine.Console.LogException(e);
        }
#endif
    }
}
