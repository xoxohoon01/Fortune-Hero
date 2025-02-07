using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIBase : MonoBehaviour
{
    GameObject clickedObject;
    List<RaycastResult> raycastResults;

    public virtual void Opened(params object[] param)
    {

    }

    public virtual void Hide()
    {
    }


    protected void ClickOnTargetUI()
    {
        GraphicRaycaster raycaster = transform.root.GetComponent<GraphicRaycaster>();
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(eventData, results);
        if (results.Count > 0)
        {
            clickedObject = results[0].gameObject;
            return;
        }
    }
    protected bool IsClickingOnTargetUI()
    {
        GraphicRaycaster raycaster = transform.root.GetComponent<GraphicRaycaster>();

        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(eventData, results);

        foreach (var result in results)
        {
            if (result.gameObject == gameObject || result.gameObject.transform.IsChildOf(gameObject.transform))
            {
                return true;
            }
        }

        if (clickedObject != null)
        {
            if (clickedObject == gameObject || clickedObject.transform.IsChildOf(gameObject.transform))
            {
                return true;
            }
        }
        return false;
    }


    protected void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClickOnTargetUI();
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (IsClickingOnTargetUI() == false)
            {
                UIManager.Instance.Hide(gameObject.name);
            }
            clickedObject = null;
        }
    }
}

