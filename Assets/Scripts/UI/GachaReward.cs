using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class GachaReward : UIBase
{
    public ScrollRect scroll;
    public RectTransform background;

    public override void Hide()
    {
        base.Hide();
        for (int i = 0; i < scroll.content.childCount; i++)
        {
            Destroy(scroll.content.GetChild(i).gameObject);
        }
    }

    public void Initialize(List<Hero> heroList)
    {
        if (heroList != null)
        {
            background.sizeDelta = new Vector2(1000, 350 + 160 * (heroList.Count / 6));
            for (int i = 0; i < heroList.Count; i++)
            {
                Instantiate(Resources.Load<GameObject>("UI/HeroInventorySlot"), scroll.content).transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/Heroes/{DataManager.Instance.Hero.Get(heroList[i].ID)?.name}");
            }
        }
    }

    public void Initialize(List<Item> itemList)
    {
        if (itemList != null)
        {
            background.sizeDelta = new Vector2(1000, 350 + 160 * (itemList.Count / 6));
            for (int i = 0; i < itemList.Count; i++)
            {
                Instantiate(Resources.Load<GameObject>("UI/HeroInventorySlot"), scroll.content).transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/Heroes/{DataManager.Instance.Hero.Get(itemList[i].id)?.name}");
            }
        }
    }

    private void Start()
    {
        base.Initialize();
    }
}
