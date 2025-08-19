// Copyright (C) 2017 gamevanilla. All rights reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement,
// a copy of which is available at http://unity3d.com/company/legal/as_terms.

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public partial class AlertPopupController
{
#pragma warning disable 649
    [SerializeField]
    private Text titleText;

    [SerializeField]
    private Text bodyText;
#pragma warning restore 649

    /// <summary>
    /// Unity's Awake method.
    /// </summary>
    protected void Awake()
    {
        Assert.IsNotNull(titleText);
        Assert.IsNotNull(bodyText);
    }

    /// <summary>
    /// Called when the popup button is pressed.
    /// </summary>
    public void OnButtonPressed()
    {
        OnKeyBack();
    }

    /// <summary>
    /// Called when the close button is pressed.
    /// </summary>
    public void OnCloseButtonPressed()
    {
        OnKeyBack();
    }

    public void SetTitle(string text)
    {
        titleText.text = text;
    }

    /// <summary>
    /// Sets the body text.
    /// </summary>
    /// <param name="text">The body text.</param>
    public void SetText(string text)
    {
        bodyText.text = text;
    }
}
