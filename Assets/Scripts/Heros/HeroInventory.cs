using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeroInventory : UIBase
{
    public Image currentHeroSlot;

    public Image heroSlot1;
    public Image heroSlot2;
    public Image heroSlot3;
    public Image heroSlot4;

    public TMP_Text heroName;

    public ScrollRect scroll;

    public bool isChangeSlotMode { get; private set; }
    public int currentSelectedHeroInventory = -1;
    public int currentSelectedHero = -1;

    public override void Hide()
    {
        scroll.verticalNormalizedPosition = 1.0f;

        currentSelectedHero = -1;
        currentSelectedHeroInventory = -1;
        isChangeSlotMode = false;

        heroName.text = "";
        currentHeroSlot.transform.GetChild(0).GetComponent<Image>().sprite = null;
    }

    public override void Initialize()
    {
        base.Initialize();

        heroName.text = "";

        currentSelectedHeroInventory = -1;
        currentSelectedHero = -1;

        UpdateHeroSlot();
        UpdateHeroInventory();
    }

    public void ChangeHero(Hero currentHero)
    {
        if (currentHero != null)
        {
            heroName.text = DataManager.Instance.Hero.Get(currentHero.ID)?.name;
            currentHeroSlot.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Heroes/" + DataManager.Instance.Hero.Get(currentHero.ID)?.name);
        }
        else
        {
            heroName.text = "";
            currentHeroSlot.transform.GetChild(0).GetComponent<Image>().sprite = null;
        }
    }

    public void ChangeSlotMode()
    {
        if (currentSelectedHeroInventory != -1 || currentSelectedHero != -1)
            isChangeSlotMode = true;
    }

    public void ReleaseHero()
    {
        if (currentSelectedHero != -1 && GameManager.Instance.heroInventory.hero[currentSelectedHero] != null)
        {
            Hero temp = GameManager.Instance.heroInventory.hero[currentSelectedHero];
            GameManager.Instance.heroInventory.hero[currentSelectedHero] = null;
            GameManager.Instance.heroInventory.heroDatas.Add(temp);
            UpdateHeroInventory();
            UpdateHeroSlot();
            ChangeHero(null);

            currentSelectedHero = -1;
            currentSelectedHeroInventory = -1;

            DatabaseManager.Instance.SaveData(GameManager.Instance.heroInventory, "HeroData");
        }
    }

    public void FinishChangeSlotMode()
    {
        isChangeSlotMode = false;
    }

    public void UpdateHeroSlot()
    {
        heroSlot1.transform.GetChild(0).GetComponent<Image>().sprite = GameManager.Instance.heroInventory.hero[0] != null ? Resources.Load<Sprite>("Sprites/Heroes/" + DataManager.Instance.Hero.Get(GameManager.Instance.heroInventory.hero[0].ID).name) : null;
        heroSlot2.transform.GetChild(0).GetComponent<Image>().sprite = GameManager.Instance.heroInventory.hero[1] != null ? Resources.Load<Sprite>("Sprites/Heroes/" + DataManager.Instance.Hero.Get(GameManager.Instance.heroInventory.hero[1].ID).name) : null;
        heroSlot3.transform.GetChild(0).GetComponent<Image>().sprite = GameManager.Instance.heroInventory.hero[2] != null ? Resources.Load<Sprite>("Sprites/Heroes/" + DataManager.Instance.Hero.Get(GameManager.Instance.heroInventory.hero[2].ID).name) : null;
        heroSlot4.transform.GetChild(0).GetComponent<Image>().sprite = GameManager.Instance.heroInventory.hero[3] != null ? Resources.Load<Sprite>("Sprites/Heroes/" + DataManager.Instance.Hero.Get(GameManager.Instance.heroInventory.hero[3].ID).name) : null;
    }

    public void UpdateHeroInventory()
    {
        for (int i = 0; i < scroll.content.childCount; i++)
        {
            Destroy(scroll.content.GetChild(i).gameObject);
        }

        if (GameManager.Instance.heroInventory.heroDatas.Count < 30)
        {
            int count = 0;
            if (GameManager.Instance.heroInventory.heroDatas.Count > 0)
            {
                for (int i = 0; i < GameManager.Instance.heroInventory.heroDatas.Count; i++)
                {
                    Hero currentHero;
                    if (GameManager.Instance.heroInventory.heroDatas[i] != null)
                        currentHero = GameManager.Instance.heroInventory.heroDatas[i];
                    else
                        currentHero = new Hero();
                    Instantiate(Resources.Load<GameObject>("UI/HeroInventorySlot"), scroll.content).GetComponent<HeroInventorySlot>().Initialize(currentHero, i);
                    count++;
                }
            }
            for (int i = count; i < 30; i++)
            {
                Instantiate(Resources.Load<GameObject>("UI/HeroInventorySlot"), scroll.content).GetComponent<HeroInventorySlot>().Initialize(new Hero(), i);
            }
        }
        else
        {
            for (int i = 0; i < GameManager.Instance.heroInventory.heroDatas.Count; i++)
            {
                Hero currentHero;
                if (GameManager.Instance.heroInventory.heroDatas[i] != null)
                    currentHero = GameManager.Instance.heroInventory.heroDatas[i];
                else
                    currentHero = new Hero();
                Instantiate(Resources.Load<GameObject>("UI/HeroInventorySlot"), scroll.content).GetComponent<HeroInventorySlot>().Initialize(currentHero, i);
            }
        }
    }

    public void OpenHeroItemInventory()
    {
        
        if (currentSelectedHero != -1)
        {
            UIManager.Instance.Show<HeroItemInventory>("FloatingUI");
            UIManager.Instance.Get<HeroItemInventory>().ChangeHero(currentSelectedHero);
        }
        else if (currentSelectedHeroInventory != -1)
        {
            UIManager.Instance.Show<HeroItemInventory>("FloatingUI");
            UIManager.Instance.Get<HeroItemInventory>().ChangeHero(currentSelectedHeroInventory);
        }
    }

    public void OpenHeroUpgrade()
    {
        if (currentSelectedHero != -1)
        {
            UIManager.Instance.Show<HeroUpgrade>("FloatingUI");
            UIManager.Instance.Get<HeroUpgrade>().ChangeHero(currentSelectedHero);
        }
    }

    private void Start()
    {
        Initialize();
    }

    private void OnEnable()
    {
        UpdateHeroInventory();
        UpdateHeroSlot();
    }
}
