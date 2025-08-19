using Creator;
using GameVanilla.Game.UI;
using UnityEngine;
using UnityEngine.UI;

public partial class BuyBoostersPopupController : Controller
{
    public const string BUYBOOSTERSPOPUP_SCENE_NAME = "BuyBoostersPopup";

    public override string SceneName()
    {
        return BUYBOOSTERSPOPUP_SCENE_NAME;
    }

#pragma warning disable 649
    [SerializeField]
    private Sprite horizontalBombSprite;

    [SerializeField]
    private Sprite verticalBombSprite;

    [SerializeField]
    private Sprite dynamiteSprite;

    [SerializeField]
    private Sprite colorBombSprite;

    [SerializeField]
    private Text boosterNameText;

    [SerializeField]
    private Text boosterDescriptionText;

    [SerializeField]
    private Image boosterImage;

    [SerializeField]
    private Text boosterAmountText;

    [SerializeField]
    private Text boosterCostText;

    [SerializeField]
    private Text numCoinsText;

    [SerializeField]
    private ParticleSystem coinParticles;
#pragma warning restore 649

    public class DataPopup
    {
        public BuyBoosterButton button;

        public DataPopup(BuyBoosterButton button)
        {
            this.button = button;
        }
    }

    public override void OnActive(object data)
    {
        if (data != null)
        {
            DataPopup dataPopup = data as DataPopup;
            this.SetBooster(dataPopup.button);
        }
    }
}