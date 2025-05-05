﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityMainThreadDispatcher : MonoBehaviour
{
    private static readonly Queue<Action> _executionQueue = new Queue<Action>();

    public void Update()
    {
        lock (_executionQueue)
        {
            while (_executionQueue.Count > 0)
            {
                _executionQueue.Dequeue().Invoke();
            }
        }
    }

    public void Enqueue(IEnumerator action)
    {
        lock (_executionQueue)
        {
            _executionQueue.Enqueue(() =>
            {
                StartCoroutine(action);
            });
        }
    }

    public void Enqueue(Action action)
    {
        Enqueue(ActionWrapper(action));
    }

    private IEnumerator ActionWrapper(Action a)
    {
        a();
        yield return null;
    }

    private static UnityMainThreadDispatcher _instance = null;

    public static bool Exists()
    {
        return _instance != null;
    }

    public static UnityMainThreadDispatcher Instance()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<UnityMainThreadDispatcher>();
        }
        return _instance;
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    private void OnDestroy()
    {
        _instance = null;
    }
}