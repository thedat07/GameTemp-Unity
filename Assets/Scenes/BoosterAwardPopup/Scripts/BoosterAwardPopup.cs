// Copyright (C) 2017 gamevanilla. All rights reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement,
// a copy of which is available at http://unity3d.com/company/legal/as_terms.

using System.Collections;

using UnityEngine;

/// <summary>
/// This class contains the logic associated to the popup for awarding boosters at the end of a game.
/// </summary>
public partial class BoosterAwardPopupController
{
    /// <summary>
    /// Unity's Start method.
    /// </summary>
    protected void Start()
    {
        StartCoroutine(AutoClose());
    }

    /// <summary>
    /// This coroutine automatically closes the popup after its animation has finished.
    /// </summary>
    /// <returns>The coroutine.</returns>
    private IEnumerator AutoClose()
    {
        yield return new WaitForSeconds(1.5f);
        OnKeyBack();
    }
}

