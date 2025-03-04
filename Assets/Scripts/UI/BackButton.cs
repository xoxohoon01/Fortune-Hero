using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BackButton : MonoBehaviour, IPointerClickHandler
{
    public UIBase targetUI;

    public void OnPointerClick(PointerEventData eventData)
    {
        UIManager.Instance.Hide(targetUI.name);
    }
}
