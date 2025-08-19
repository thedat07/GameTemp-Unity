using Creator;
using UnityEngine;
using UnityEngine.UI;

public partial class BuyCoinsPopupController : Controller
{
    public const string BUYCOINSPOPUP_SCENE_NAME = "BuyCoinsPopup";

    public override string SceneName()
    {
        return BUYCOINSPOPUP_SCENE_NAME;
    }

#pragma warning disable 649
    [SerializeField]
    private GameObject iapItemsParent;

    [SerializeField]
    private GameObject iapRowPrefab;

    [SerializeField]
    private Text numCoinsText;

    [SerializeField]
    private ParticleSystem coinsParticles;
#pragma warning restore 649
}