using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    public Canvas canvas;
    private Dictionary<string, UIBase> uiDictionary = new Dictionary<string, UIBase>();
    public Dictionary<string, GameObject> uiObjectDictionary = new Dictionary<string, GameObject>();

    public void Initialize()
    {
        canvas = new GameObject("Canvas").AddComponent<Canvas>();
        canvas.gameObject.layer = LayerMask.NameToLayer("UI");
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        var canvasScaler = canvas.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1920, 1080);
        canvasScaler.matchWidthOrHeight = 1;

        canvas.AddComponent<GraphicRaycaster>();
    }

    public void Open<T>() where T : UIBase
    {
        UIBase newUI = Resources.Load<UIBase>("UI/" + typeof(T).ToString());
        uiDictionary.TryAdd(typeof(T).ToString(), newUI);
        uiObjectDictionary.TryAdd(typeof(T).ToString(), Instantiate(newUI.gameObject, canvas.transform));
    }

    public void Close<T>() where T : UIBase
    {
        GameObject currentUI = uiObjectDictionary[typeof(T).ToString()];
        uiObjectDictionary.Remove(typeof(T).ToString());
        Destroy(currentUI);
    }

    public void Show<T>() where T : UIBase
    {
        UIBase newUI = Resources.Load<UIBase>("UI/" + typeof(T).ToString());
        uiDictionary.TryAdd(typeof(T).ToString(), newUI);

        if (uiObjectDictionary.ContainsKey(typeof(T).ToString()))
        {
            uiObjectDictionary[typeof(T).ToString()].SetActive(true);
        }
        else
        {
            uiObjectDictionary.TryAdd(typeof(T).ToString(), Instantiate(newUI.gameObject, canvas.transform));
        }
    }
    public void Hide<T>() where T : UIBase
    {
        UIBase newUI = Resources.Load<UIBase>("UI/" + typeof(T).ToString());
        uiDictionary.TryAdd(typeof(T).ToString(), newUI);

        if (uiObjectDictionary.ContainsKey(typeof(T).ToString()))
        {
            uiObjectDictionary[typeof(T).ToString()].GetComponent<UIBase>().Hide();
            uiObjectDictionary[typeof(T).ToString()].SetActive(false);
        }
    }

    public T Get<T>() where T : UIBase
    {
        uiObjectDictionary.TryGetValue(typeof(T).ToString(), out GameObject ui);
        if (ui != null)
            return ui.GetComponent<T>();
        else
            return null;
    }
}
