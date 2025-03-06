using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpeedController : UIBase, IPointerClickHandler
{
    public Image sprite;
    private int currentSpeed = 1;

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (currentSpeed)
        {
            case 1:
                currentSpeed = 2;
                break;
            case 2:
                currentSpeed = 4;
                break;
            case 4:
                currentSpeed = 1;
                break;
        }
        sprite.transform.GetChild(0).GetComponent<TMP_Text>().text = $"x{currentSpeed}";
        Time.timeScale = currentSpeed;
    }
}
