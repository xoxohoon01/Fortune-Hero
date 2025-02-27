using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroTranscendence : UIBase
{
    public ScrollRect scroll;
    public Image currentHeroSlot;
    public Image selectedHeroSlot;
    public TMP_Text heroName;
    public TMP_Text heroLevel;
    public TMP_Text heroTranscendence;
    public TMP_Text heroGrade;

    private Hero currentHero;
    private int currentHeroSlotNumber = -1;
    private int currentHeroInventorySlotNumber = -1;

    private int selectedHeroSlotNumber = -1;

    public void Initialize(int heroSlotNumber)
    {
        currentHeroSlotNumber = heroSlotNumber;
        currentHeroInventorySlotNumber = -1;

        currentHero = GameManager.Instance.heroInventory.hero[currentHeroSlotNumber];

        selectedHeroSlotNumber = -1;
        selectedHeroSlot.transform.GetChild(0).GetComponent<Image>().sprite = null;

        UpdateHero(currentHero);
        UpdateHeroInventory();
    }

    public void InitializeInventory(int heroSlotNumber)
    {
        currentHeroSlotNumber = -1;
        currentHeroInventorySlotNumber = heroSlotNumber;

        currentHero = GameManager.Instance.heroInventory.heroDatas[currentHeroInventorySlotNumber];

        selectedHeroSlotNumber = -1;
        selectedHeroSlot.transform.GetChild(0).GetComponent<Image>().sprite = null;

        UpdateHero(currentHero);
        UpdateHeroInventory();
    }

    public void SelectHero(int heroSlot)
    {
        selectedHeroSlotNumber = heroSlot;

        selectedHeroSlot.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/Heroes/{DataManager.Instance.Hero.Get(GameManager.Instance.heroInventory.heroDatas[selectedHeroSlotNumber].ID).name}");
    }

    public void Transcend()
    {
        if (currentHeroSlotNumber != -1 && currentHeroSlotNumber != selectedHeroSlotNumber)
        {
            if (currentHero != GameManager.Instance.heroInventory.heroDatas[selectedHeroSlotNumber])
            {
                selectedHeroSlot.transform.GetChild(0).GetComponent<Image>().sprite = null;
                GameManager.Instance.RemoveHero(selectedHeroSlotNumber);
                currentHero.transcendence++;
                UpdateHero(currentHero);
                UpdateHeroInventory();

                DatabaseManager.Instance.SaveData(GameManager.Instance.heroInventory, "HeroData");
                UIManager.Instance.Get<HeroUpgrade>()?.UpdateSelectedHero();
                UIManager.Instance.Get<HeroInventory>()?.UpdateHeroInventory();
            }
        }
        else if (currentHero != null && selectedHeroSlotNumber != -1)
        {
            if (currentHero != GameManager.Instance.heroInventory.heroDatas[selectedHeroSlotNumber])
            {
                selectedHeroSlot.transform.GetChild(0).GetComponent<Image>().sprite = null;
                GameManager.Instance.RemoveHero(selectedHeroSlotNumber);
                currentHero.transcendence++;
                UpdateHero(currentHero);
                UpdateHeroInventory();

                DatabaseManager.Instance.SaveData(GameManager.Instance.heroInventory, "HeroData");
                UIManager.Instance.Get<HeroUpgrade>()?.UpdateSelectedHero();
                UIManager.Instance.Get<HeroInventory>()?.UpdateHeroInventory();
            }
        }
    }

    public void UpdateHero(Hero hero)
    {
        if (hero != null)
        {
            currentHero = hero;
            heroName.text = DataManager.Instance.Hero.Get(hero.ID)?.name;
            heroLevel.text = currentHero.level.ToString();
            heroTranscendence.text = currentHero.transcendence.ToString();
            heroGrade.text = currentHero.grade.ToString();
            currentHeroSlot.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Heroes/" + DataManager.Instance.Hero.Get(hero.ID)?.name);
        }
        else
        {
            heroName.text = "";
            currentHeroSlot.transform.GetChild(0).GetComponent<Image>().sprite = null;
        }
    }

    public void UpdateHeroInventory()
    {
        for (int i = 0; i < scroll.content.childCount; i++)
        {
            Destroy(scroll.content.GetChild(i).gameObject);
        }

        if (GameManager.Instance.heroInventory.heroDatas.Count < 30)
        {
            Hero targetHero;
            int count = 0;
            if (GameManager.Instance.heroInventory.heroDatas.Count > 0)
            {
                for (int i = 0; i < GameManager.Instance.heroInventory.heroDatas.Count; i++)
                {
                    targetHero = GameManager.Instance.heroInventory.heroDatas[i];
                    if (currentHeroSlotNumber != -1 && GameManager.Instance.heroInventory.hero[currentHeroSlotNumber].ID == targetHero.ID)
                    {
                        Instantiate(Resources.Load<GameObject>("UI/HeroInventorySlot"), scroll.content).GetComponent<HeroInventorySlot>().Initialize(targetHero, i);
                        count++;
                    }
                    else if (currentHeroInventorySlotNumber != -1 && GameManager.Instance.heroInventory.heroDatas[currentHeroInventorySlotNumber].ID == targetHero.ID && currentHeroInventorySlotNumber != i)
                    {
                        Instantiate(Resources.Load<GameObject>("UI/HeroInventorySlot"), scroll.content).GetComponent<HeroInventorySlot>().Initialize(targetHero, i);
                        count++;
                    }
                }
            }
            for (int i = count; i < 30; i++)
            {
                targetHero = new Hero();
                Instantiate(Resources.Load<GameObject>("UI/HeroInventorySlot"), scroll.content).GetComponent<HeroInventorySlot>().Initialize(targetHero, i);
            }
        }
        else
        {
            for (int i = 0; i < GameManager.Instance.heroInventory.heroDatas.Count; i++)
            {
                Hero targetHero;
                if (GameManager.Instance.heroInventory.heroDatas[i] != null)
                    targetHero = GameManager.Instance.heroInventory.heroDatas[i];
                else
                    targetHero = new Hero();

                if (targetHero.ID == currentHero.ID)
                {
                    if (currentHeroSlotNumber != -1)
                    {
                        Instantiate(Resources.Load<GameObject>("UI/HeroInventorySlot"), scroll.content).GetComponent<HeroInventorySlot>().Initialize(targetHero, i);
                    }
                    else if (currentHeroInventorySlotNumber != -1 && currentHeroInventorySlotNumber != selectedHeroSlotNumber)
                    {
                        Instantiate(Resources.Load<GameObject>("UI/HeroInventorySlot"), scroll.content).GetComponent<HeroInventorySlot>().Initialize(currentHero, i);
                    }
                }
            }
        }
    }

    private void Start()
    {
        base.Initialize();
    }
}
