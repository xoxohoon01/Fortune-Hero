using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class GachaCatalogSlot : MonoBehaviour, IPointerClickHandler
{
    public TMP_Text currentCatalogName;
    private int currentGachaID;

    public void Initialize(int id)
    {
        currentCatalogName.text = DataManager.Instance.GachaShop.Get(id).name;
        currentGachaID = id;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UIManager.Instance.Get<GachaShop>().SelectCacha(currentGachaID);
    }
}
