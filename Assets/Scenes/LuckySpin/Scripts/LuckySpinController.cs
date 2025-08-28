using Creator;
using TMPro;
using UnityEngine;
using UniRx;

public class LuckySpinController : Controller
{
    public const string LUCKYSPIN_SCENE_NAME = "LuckySpin";

    public override string SceneName()
    {
        return LUCKYSPIN_SCENE_NAME;
    }

    [Header("View")]

    public TextMeshProUGUI infoText;

    private FeatureLukySpin m_Data;

    [SerializeField] SpinWheelController m_SpinWheelController;

    void Start()
    {
        m_Data = GameManager.Instance.GetFeatureData().featureLukySpin;

        // Lắng nghe freeSpinUsed thay đổi
        m_Data.GetData().freeSpinUsed
            .Subscribe(_ => UpdateInfoText())
            .AddTo(this);

        // Lắng nghe adsSpinsUsed thay đổi
        m_Data.GetData().adsSpinsUsed
            .Subscribe(_ => UpdateInfoText())
            .AddTo(this);

        // Lắng nghe reset ngày (lastSpinDate)
        m_Data.GetData().lastSpinDate
            .Subscribe(_ => UpdateInfoText())
            .AddTo(this);

        // Khởi tạo text ban đầu
        UpdateInfoText();

        m_SpinWheelController.Init(m_Data);
    }

    private void UpdateInfoText()
    {
        var (freeLeft, adsLeft) = m_Data.GetRemainingSpins();
        if (infoText) infoText.text = $"Free: {freeLeft} | Ads: {adsLeft}";
    }

    // Nút spin thường
    public void OnFreeSpin()
    {
        if (m_Data.CanSpin(false))
        {
            m_Data.DoSpin(false);
            Console.Log("Free spin thành công!");
        }
        else
        {
            Console.Log("Free spin đã hết!");
        }
    }

    // Nút spin Ads
    public void OnAdsSpin()
    {
        if (m_Data.CanSpin(true))
        {
            m_Data.DoSpin(true);
            Console.Log("Ads spin thành công!");
        }
        else
        {
            Console.Log("Ads spin đã hết!");
        }
    }
}