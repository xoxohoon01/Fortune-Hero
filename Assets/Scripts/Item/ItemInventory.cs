using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ItemInventory : UIBase
{
    public ScrollRect scroll;
    private bool isInitialized;
    private int page;

    public override void Hide()
    {
        scroll.verticalNormalizedPosition = 1.0f;
    }

    public void ChangeAll()
    {
        scroll.verticalNormalizedPosition = 1.0f;
        page = 0;

        int count = 0;
        for (int i = 0; i < GameManager.Instance.itemInventory.itemDatas.Count; i++)
        {
            Item item = GameManager.Instance.itemInventory.itemDatas[i];

            scroll.content.GetChild(i).GetChild(0).GetComponent<TMP_Text>().text = item.data != null ? item.data.ID.ToString() : "";
            scroll.content.GetChild(i).GetChild(1).GetComponent<TMP_Text>().text = item.data != null ? item.amount.ToString() : "";
            count++;
        }
        if (GameManager.Instance.itemInventory.itemDatas.Count < 300)
        {
            for (int i = count; i < 300; i++)
            {
                scroll.content.GetChild(i).GetChild(0).GetComponent<TMP_Text>().text = "";
                scroll.content.GetChild(i).GetChild(1).GetComponent<TMP_Text>().text = "";
            }
        }
    }

    public void ChangeType(int type)
    {
        scroll.verticalNormalizedPosition = 1.0f;
        page = type;

        int count = 0;
        for (int i = 0; i < GameManager.Instance.itemInventory.itemDatas.Count; i++)
        {
            Item item = GameManager.Instance.itemInventory.itemDatas[i];

            if (item.data != null && item.data.type == type)
            {
                scroll.content.GetChild(i).GetChild(0).GetComponent<TMP_Text>().text = item.data.ID.ToString();
                scroll.content.GetChild(i).GetChild(1).GetComponent<TMP_Text>().text = item.amount.ToString();
            }
            else
            {
                scroll.content.GetChild(i).GetChild(0).GetComponent<TMP_Text>().text = "";
                scroll.content.GetChild(i).GetChild(1).GetComponent<TMP_Text>().text = "";
            }

            count++;
        }
        if (GameManager.Instance.itemInventory.itemDatas.Count < 300)
        {
            for (int i = count; i < 300; i++)
            {
                scroll.content.GetChild(i).GetChild(0).GetComponent<TMP_Text>().text = "";
                scroll.content.GetChild(i).GetChild(1).GetComponent<TMP_Text>().text = "";
            }
        }
    }

    public override void Initialize()
    {
        base.Initialize();

        for (int i = 0; i < 300; i++)
        {
            Instantiate(Resources.Load<ItemInventorySlot>("UI/ItemInventorySlot"), scroll.content).Initialize(new Item(), i);
        }

        ChangeAll();
        isInitialized = true;
    }

    public void UpdateSlot()
    {
        if (page == 0)
            ChangeAll();
        else
            ChangeType(page);
    }

    private void OnEnable()
    {
        if (isInitialized)
            UpdateSlot();
    }

    private void Start()
    {
        Initialize();
    }

}
