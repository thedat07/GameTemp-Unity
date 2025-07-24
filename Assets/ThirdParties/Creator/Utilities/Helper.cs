using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UniRx;

public static class Helper
{
    public static readonly MasterDataType[] ShopAllowedTypes = new[]
    {
        MasterDataType.Money,
        MasterDataType.Booster1,
        MasterDataType.Booster2,
        MasterDataType.Booster3,
        MasterDataType.Booster4,
        MasterDataType.NoAds
    };

    public static void StopEverythingInScene()
    {
        // Stop all coroutines on all active MonoBehaviours
        MonoBehaviour[] allBehaviours = Object.FindObjectsOfType<MonoBehaviour>();
        foreach (var behaviour in allBehaviours)
        {
            behaviour.StopAllCoroutines();
        }

        // Kill all active DOTween tweens (also complete them if needed)
        DOTween.KillAll(); // Pass 'true' if you want to complete tweens before killing

        // Optional: reset time scale if it was changed
        Time.timeScale = 1f;

        // Optional: clear DOTween memory (pools, etc.)
        DOTween.Clear();
    }

    public static List<InventoryItem> Convert(List<InventoryItem> data)
    {
        if (data != null)
        {
            var groupedByType = data
             .GroupBy(item => item.GetDataType())
             .Select(group => new InventoryItem(
                new ItemData()
                {
                    type = group.Key
                },
                group.Sum(item => item.GetQuantity())
             ))
             .ToList();
            return groupedByType;
        }
        else
        {
            return new List<InventoryItem>();
        }
    }

    public static T JsonToObject<T>(string value)
    {
        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value);
    }

    public static string ObjectToJson<T>(T classT)
    {
        return Newtonsoft.Json.JsonConvert.SerializeObject(classT);
    }

    public static void SetDelay(this MonoBehaviour monoBehaviour, float timeDelay, UnityAction callBack)
    {
        monoBehaviour.StartCoroutine(DelayCoroutine(timeDelay, callBack));
    }

    public static void SetDelayNextFrame(this MonoBehaviour monoBehaviour, UnityAction callBack)
    {
        monoBehaviour.StartCoroutine(DelayNextFrameCoroutine(callBack));
    }

    private static IEnumerator DelayCoroutine(float delay, UnityAction callBack)
    {
        yield return new WaitForSeconds(delay);
        callBack?.Invoke();
    }

    private static IEnumerator DelayNextFrameCoroutine(UnityAction callBack)
    {
        yield return new WaitForEndOfFrame();
        callBack?.Invoke();
    }

    public static DG.Tweening.Sequence SetDelay(float delay, UnityAction callBack, GameObject target = null, LinkBehaviour behaviour = LinkBehaviour.KillOnDestroy)
    {
        DG.Tweening.Sequence sequence = DOTween.Sequence();

        sequence.SetLink(target, behaviour);

        sequence.Append(DOVirtual.DelayedCall(delay, () => { callBack?.Invoke(); }));

        return sequence;
    }

    public static DG.Tweening.Sequence DOTweenSequence(GameObject target, LinkBehaviour behaviour = LinkBehaviour.KillOnDestroy)
    {
        DG.Tweening.Sequence sequence = DOTween.Sequence();
        sequence.SetLink(target, behaviour);
        return sequence;
    }

    public static bool IsNotNone(ItemData itemData)
    {
        return itemData != null && itemData.type != MasterDataType.None;
    }
}
