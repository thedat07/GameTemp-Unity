using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public static class Helper
{
    public static T JsonToObject<T>(string value)
    {
        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value);
    }

    public static string ObjectToJson<T>(T classT)
    {
        return Newtonsoft.Json.JsonConvert.SerializeObject(classT);
    }

    public static void SetDelay(this MonoBehaviour gameObject, float timeDelay, UnityAction callBack)
    {
        gameObject.StartCoroutine(Wait());
        IEnumerator Wait()
        {
            yield return new WaitForSeconds(timeDelay);
            callBack?.Invoke();
        }
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
}
