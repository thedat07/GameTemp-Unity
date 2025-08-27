using Creator;
using NaughtyAttributes;
using UnityEngine.UI;
using UniRx;
using TMPro;
using UnityEngine;

public class PiggyBankController : Controller
{
    public const string PIGGYBANK_SCENE_NAME = "PiggyBank";

    public override string SceneName()
    {
        return PIGGYBANK_SCENE_NAME;
    }

    public Image slider;

    public TextMeshPro expText;

    private FeaturePiggyBank m_Data;

    void Start()
    {
        m_Data = GameManager.Instance.GetFeatureData().featurePiggyBank;

        m_Data.GetData().exp.Subscribe(newExp => ViewExp(newExp)).AddTo(this);

        m_Data.GetData().isFull.Subscribe(isFull => ViewFull(isFull)).AddTo(this);
    }

    [Button]
    public void OnBuy()
    {
        if (m_Data.BuyPiggy())
        {
            Console.Log("Mua thành công PiggyBank, reset lại!");
        }
        else
        {
            Console.Log("Không thể mua PiggyBank (chưa full hoặc đã hết hạn).");
        }
    }

    void ViewExp(int newExp)
    {
        int maxExp = m_Data.GetMaxExp();

        if (expText)
            expText.text = $"{newExp}/{maxExp}";

        if (slider)
            slider.fillAmount = (float)newExp / maxExp;
    }

    void ViewFull(bool isFull)
    {
        if (isFull)
        {
            if (expText) expText.text = "FULL";
            if (slider) slider.fillAmount = 1f;
        }
    }
}