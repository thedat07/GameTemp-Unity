// Copyright (C) 2017 gamevanilla. All rights reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement,
// a copy of which is available at http://unity3d.com/company/legal/as_terms.

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

using GameVanilla.Core;
using GameVanilla.Game.Common;
using GameVanilla.Game.UI;

public partial class BuyCoinsPopupController
{
    protected void Awake()
    {
        Assert.IsNotNull(iapItemsParent);
        Assert.IsNotNull(iapRowPrefab);
        Assert.IsNotNull(numCoinsText);
        Assert.IsNotNull(coinsParticles);
    }

    protected void Start()
    {
        var coins = PlayerPrefs.GetInt("num_coins");
        numCoinsText.text = coins.ToString("n0");
    }
}
