using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDescription : UIBase
{
    public TMP_Text itemName;
    public TMP_Text itemUpgrade;
    public Image sprite;
    public TMP_Text itemDescription;

    public void Initialize(Item item)
    {
        base.Initialize();

        itemName.text = item.data.name;
        itemUpgrade.text = $"+{item.upgrade}";
        sprite.transform.GetChild(0).GetComponent<TMP_Text>().text = item.data.ID.ToString();
        itemDescription.text = item.data.description;
    }
}
