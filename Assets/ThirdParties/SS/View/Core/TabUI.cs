using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;
using DG.Tweening;
using LibraryGame;
using UnityEngine.UI;

public class TabUI : MonoBehaviour, IPointerClickHandler
{
    public enum Type
    {
        One,
        Multi,
        None
    }

    public enum Status
    {
        IsActive,
        None,
    }

    public Type type;

    public Status status;

    public UnityEvent onTab;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (status == Status.IsActive)
        {
            if (type == Type.One)
            {
                onTab?.Invoke();
                type = Type.None;
            }
            else if (type == Type.Multi)
            {
                onTab?.Invoke();
            }
        }
    }
}
