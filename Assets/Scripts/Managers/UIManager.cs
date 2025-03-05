using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    public Dictionary<string, Canvas> canvas;
    private Dictionary<string, UIBase> uiDictionary = new Dictionary<string, UIBase>();
    public Dictionary<string, GameObject> uiObjectDictionary = new Dictionary<string, GameObject>();

    public void CreateCanvas(string name)
    {
        Canvas newCanvas = new GameObject("Canvas_"+name).AddComponent<Canvas>();
        newCanvas.gameObject.layer = LayerMask.NameToLayer("UI");
        newCanvas.renderMode = RenderMode.ScreenSpaceOverlay;

        var canvasScaler = newCanvas.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1080, 2340);
        canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;

        newCanvas.AddComponent<GraphicRaycaster>();
        canvas.Add(name, newCanvas);
    }

    public void Initialize()
    {
        canvas = new Dictionary<string, Canvas>();

        CreateCanvas("HUD");
        CreateCanvas("FloatingUI");

        Show<HUDButtons>("HUD", false);
    }

    public void Open<T>() where T : UIBase
    {
        UIBase newUI = Resources.Load<UIBase>("UI/" + typeof(T).ToString());
        uiDictionary.TryAdd(typeof(T).ToString(), newUI);
        uiObjectDictionary.TryAdd(typeof(T).ToString(), Instantiate(newUI.gameObject, canvas["FloatingUI"].transform));
    }

    public void Close<T>() where T : UIBase
    {
        GameObject currentUI = uiObjectDictionary[typeof(T).ToString()];
        uiObjectDictionary.Remove(typeof(T).ToString());
        Destroy(currentUI);
    }

    public void Show<T>(string name, bool isFloating = true) where T : UIBase
    {
        UIBase newUI = Resources.Load<UIBase>("UI/" + typeof(T).ToString());
        uiDictionary.TryAdd(typeof(T).ToString(), newUI);

        if (uiObjectDictionary.ContainsKey(typeof(T).ToString()))
        {
            uiObjectDictionary[typeof(T).ToString()].SetActive(true);
            uiObjectDictionary[typeof(T).ToString()].transform.SetAsLastSibling();
        }
        else
        {
            GameObject newUIObject = Instantiate(newUI.gameObject, canvas[name].transform);
            newUIObject.name = typeof(T).ToString();
            uiObjectDictionary.TryAdd(typeof(T).ToString(), newUIObject);
            newUIObject.transform.SetAsLastSibling();
        }
    }
    public void Hide<T>() where T : UIBase
    {
        if (uiObjectDictionary.ContainsKey(typeof(T).ToString()))
        {
            uiObjectDictionary[typeof(T).ToString()].GetComponent<UIBase>().Hide();
            uiObjectDictionary[typeof(T).ToString()].SetActive(false);
        }
    }
    public void Hide(string ui)
    {
        if (uiObjectDictionary.ContainsKey(ui))
        {
            uiObjectDictionary[ui].GetComponent<UIBase>().Hide();
            uiObjectDictionary[ui].SetActive(false);
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
