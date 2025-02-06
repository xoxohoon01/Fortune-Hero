using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroInventory : UIBase
{
    public Image heroSlot1;
    public Image heroSlot2;
    public Image heroSlot3;
    public Image heroSlot4;

    public TMP_Text heroName;
    public TMP_Text hp;
    public TMP_Text mp;
    public TMP_Text physicalDamage;
    public TMP_Text magicalDamage;
    public TMP_Text attackSpeed;
    public TMP_Text moveSpeed;

    public ScrollRect scroll;

    private void OnEnable()
    {
        Initialize();
    }

    private void Initialize()
    {
        for (int i = 0; i < scroll.content.childCount; i++)
        {
            Destroy(scroll.content.GetChild(i).gameObject);
        }

        for (int i = 0; i < GameManager.Instance.heroInventory.heroDatas.Count; i++)
        {
            Hero currentHero;
            if (GameManager.Instance.heroInventory.heroDatas[i] != null)
                currentHero = GameManager.Instance.heroInventory.heroDatas[i];
            else
                currentHero = new Hero();
            Instantiate(Resources.Load<GameObject>("UI/HeroInventorySlot"), scroll.content).GetComponent<HeroInventorySlot>().Initialize(currentHero);
        }
    }

    public void ChangeHero(Hero currentHero)
    {
        heroName.text = DataManager.Instance.Hero.Get(currentHero.ID)?.name;
        hp.text = DataManager.Instance.Hero.Get(currentHero.ID).hp.ToString();
        mp.text = DataManager.Instance.Hero.Get(currentHero.ID).hp.ToString();
        physicalDamage.text = DataManager.Instance.Hero.Get(currentHero.ID).physicalDamage.ToString();
        magicalDamage.text = DataManager.Instance.Hero.Get(currentHero.ID).magicalDamage.ToString();
        attackSpeed.text = DataManager.Instance.Hero.Get(currentHero.ID).attackSpeed.ToString();
        moveSpeed.text = DataManager.Instance.Hero.Get(currentHero.ID).moveSpeed.ToString();
    }
}
