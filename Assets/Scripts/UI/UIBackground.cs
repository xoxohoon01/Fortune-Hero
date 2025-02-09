using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIBackground : UIBase, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        UIManager.Instance.Hide(transform.parent.name);
    }
}
