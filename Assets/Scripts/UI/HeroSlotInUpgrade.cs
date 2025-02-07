using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HeroSlotInUpgrade : MonoBehaviour, IPointerClickHandler
{
    public int currentSlot;

    public void OnPointerClick(PointerEventData eventData)
    {
        UIManager.Instance.Get<HeroUpgrade>().ChangeHero(currentSlot);
    }
}
