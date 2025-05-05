using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

namespace LibraryGame
{
    public static class DG
    {
        public static void DoResetDefault(this Transform target)
        {
            target.DORewind();
            target.DOKill();
        }

        public static Tween DoVaule(this TextMeshProUGUI target, int vaule, bool convert = true)
        {
            Tween j;
            if (!convert)
                j = DOVirtual.Int(int.Parse(target.text), vaule, 0.5f, (x) => { target.text = string.Format("{0}", x); });
            else
                j = DOVirtual.Int(int.Parse(target.text), vaule, 0.5f, (x) => { target.Number(x); });
            return j;
        }

        public static Tween DoVaule(this TextMeshProUGUI target, int from, int vaule, bool convert = true, float time = 0.5f)
        {
            Tween j;
            if (!convert)
                j = DOVirtual.Int(from, vaule, time, (x) => { target.text = string.Format("{0}", x); });
            else
                j = DOVirtual.Int(from, vaule, time, (x) => { target.text = string.Format("{0}", AbbrevationUtility.AbbreviateNumber(x)); });
            return j;
        }

        public static Tweener DoVaule(this Text target, int startValue, int endValue, float duration, string fromat = "{0}")
        {
            return DOVirtual.Int(startValue, endValue, duration, (x) =>
            {
                target.text = string.Format(fromat, x);
            });
        }


        public static Tweener DoVauleAbbreviateNumber(this Text target, int startValue, int endValue, float duration, string fromat = "{0}")
        {
            return DOVirtual.Int(startValue, endValue, duration, (x) =>
            {
                target.text = string.Format(fromat, AbbrevationUtility.AbbreviateNumber(x));
            });
        }

        public static Tweener DoVaule(this Text target, int endValue, float duration, string fromat = "{0}")
        {
            if (int.TryParse(target.text, out _))
            {
                return DOVirtual.Int(int.Parse(target.text), endValue, duration, (x) =>
                    {
                        target.text = string.Format(fromat, x);
                    });
            }
            else
            {
                return DOVirtual.Int(0, endValue, duration, (x) =>
                    {
                        target.text = string.Format(fromat, x);
                    });
            }
        }


        public static Tweener DoVauleAbbreviateNumber(this Text target, int endValue, float duration, string fromat = "{0}")
        {
            return DOVirtual.Int(AbbrevationUtility.ConvertAbbreviatedToNumber(target.text), endValue, duration, (x) =>
            {
                target.text = string.Format(fromat, AbbrevationUtility.AbbreviateNumber(x));
            });
        }

        public static Tweener FlashTextRedMultipleTimes(this TextMeshProUGUI textMeshPro, Color flashColor, Color defaultColor)
        {
            textMeshPro.DOKill();
            textMeshPro.color = flashColor;
            return textMeshPro.DOColor(defaultColor, 0.5f).SetEase(Ease.OutFlash).SetLink(textMeshPro.gameObject, LinkBehaviour.KillOnDestroy);
        }
    }
}