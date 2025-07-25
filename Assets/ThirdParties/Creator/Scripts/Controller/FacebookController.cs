﻿using UnityEngine;
using Facebook.Unity;

public class FacebookController : MonoBehaviour, IInitializable
{
    public bool active;

    private VerifyFirebase facebookBaseReady = VerifyFirebase.Verifying;

    public bool IsDone() => active ? facebookBaseReady == VerifyFirebase.Done : false;

    public void Initialize()
    {
        if (active)
        {
            facebookBaseReady = VerifyFirebase.Verifying;

            if (!FB.IsInitialized)
            {
                // Initialize the Facebook SDK
                FB.Init(InitCallback, OnHideUnity);
            }
            else
            {
                // Already initialized, signal an app activation App Event
                FB.ActivateApp();
            }
        }
    }

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
            facebookBaseReady = VerifyFirebase.Done;
        }
        else
        {
            Console.Log("FB", "Failed to Initialize the Facebook SDK");
            facebookBaseReady = VerifyFirebase.Error;
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }
}
