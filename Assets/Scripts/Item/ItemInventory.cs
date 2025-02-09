using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInventory : UIBase
{
    public ScrollRect scroll;

    public override void Hide()
    {
        scroll.verticalNormalizedPosition = 1.0f;
    }

    public void ChangeAll()
    {
        scroll.verticalNormalizedPosition = 1.0f;

        for (int i = 0; i < scroll.content.childCount; i++)
        {
            Destroy(scroll.content.GetChild(i).gameObject);
        }

        if (GameManager.Instance.itemInventory.itemDatas.Count < 300)
        {
            int count = 0;
            for (int i = 0; i < GameManager.Instance.itemInventory.itemDatas.Count; i++)
            {
                Instantiate(Resources.Load<ItemInventorySlot>("UI/ItemInventorySlot"), scroll.content).Initialize(GameManager.Instance.itemInventory.itemDatas[i], i);
                count++;
            }
            for (int i = count; i < 300; i++)
            {
                Instantiate(Resources.Load<ItemInventorySlot>("UI/ItemInventorySlot"), scroll.content).Initialize(new Item(), i);
            }
        }
        else
        {
            for (int i = 0; i < GameManager.Instance.itemInventory.itemDatas.Count; i++)
            {
                Instantiate(Resources.Load<ItemInventorySlot>("UI/ItemInventorySlot"), scroll.content).Initialize(GameManager.Instance.itemInventory.itemDatas[i], i);
            }
        }
    }

    public void ChangeTab(int type)
    {
        scroll.verticalNormalizedPosition = 1.0f;

        for (int i = 0; i < scroll.content.childCount; i++)
        {
            Destroy(scroll.content.GetChild(i).gameObject);
        }

        if (GameManager.Instance.itemInventory.itemDatas.Count < 300)
        {
            int count = 0;
            for (int i = 0; i < GameManager.Instance.itemInventory.itemDatas.Count; i++)
            {
                if (GameManager.Instance.itemInventory.itemDatas[i].data.type == type)
                {
                    Instantiate(Resources.Load<ItemInventorySlot>("UI/ItemInventorySlot"), scroll.content).Initialize(GameManager.Instance.itemInventory.itemDatas[i], GameManager.Instance.itemInventory.itemDatas[i].amount);
                    count++;
                }
            }
            for (int i = count; i < 300; i++)
            {
                Instantiate(Resources.Load<ItemInventorySlot>("UI/ItemInventorySlot"), scroll.content).Initialize(new Item(0), 0);
            }
        }
        else
        {
            for (int i = 0; i < GameManager.Instance.itemInventory.itemDatas.Count; i++)
            {
                if (GameManager.Instance.itemInventory.itemDatas[i].data.type == type)
                {
                    Instantiate(Resources.Load<ItemInventorySlot>("UI/ItemInventorySlot"), scroll.content).Initialize(GameManager.Instance.itemInventory.itemDatas[i], GameManager.Instance.itemInventory.itemDatas[i].amount);
                }
            }
        }
    }

    public override void Initialize()
    {
        base.Initialize();

        ChangeAll();
    }

    public void UpdateSlot()
    {
        for (int i = 0; i < 300; i++)
        {
            scroll.content.GetChild(i).GetChild(0).GetComponent<TMP_Text>().text = "";
            scroll.content.GetChild(i).GetChild(1).GetComponent<TMP_Text>().text = "";
        }
        for (int i = 0; i < GameManager.Instance.itemInventory.itemDatas.Count; i++)
        {
            scroll.content.GetChild(i).GetChild(0).GetComponent<TMP_Text>().text = GameManager.Instance.itemInventory.itemDatas[i].data.ID.ToString();
            scroll.content.GetChild(i).GetChild(1).GetComponent<TMP_Text>().text = GameManager.Instance.itemInventory.itemDatas[i].amount.ToString();
        }
    }

    private void Start()
    {
        Initialize();
    }

    private void OnEnable()
    {
        ChangeAll();
    }

}
