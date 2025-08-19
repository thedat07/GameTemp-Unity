using Creator;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public partial class AlertPopupController : Controller
{
    public const string ALERTPOPUP_SCENE_NAME = "AlertPopup";

    public class DataPopup
    {
        public string textTitle;
        public string textContent;

        public DataPopup(string textTitle, string textContent)
        {
            this.textTitle = textTitle;
            this.textContent = textContent;
        }
    }

    public override string SceneName()
    {
        return ALERTPOPUP_SCENE_NAME;
    }

    public override void OnActive(object data)
    {
        if (data != null)
        {
            DataPopup dataPopup = data as DataPopup;
            this.SetTitle(dataPopup.textTitle);
            this.SetText(dataPopup.textContent);
        }
    }
}