using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroUpgrade : UIBase
{
    public TMP_Text heroName;
    public TMP_Text heroLevel;
    public TMP_Text heroTranscendence;
    public TMP_Text heroGrade;
    public Image currentHeroSprite;

    private int currentHeroSlotNumber;
    private int currentHeroSlotNumberInventory;
    private Hero currentHero;

    public delegate void UpgradeDelegate();
    public UpgradeDelegate upgradeDelegate;

    public override void Hide()
    {
        UpdateSelectedHero();
    }

    public void ChangeHero(int heroSlotNumber)
    {
        if (heroSlotNumber != -1)
        {
            currentHeroSlotNumber = heroSlotNumber;
            currentHeroSlotNumberInventory = -1;

            currentHero = GameManager.Instance.heroInventory.hero[heroSlotNumber];

            // 선택한 영웅 디스플레이 변경
            UpdateSelectedHero();
        }
    }

    public void ChangeHeroInventory(int heroSlotNumber)
    {
        if (heroSlotNumber != -1)
        {
            currentHeroSlotNumber = -1;
            currentHeroSlotNumberInventory = heroSlotNumber;

            currentHero = GameManager.Instance.heroInventory.heroDatas[currentHeroSlotNumberInventory];

            // 선택한 영웅 디스플레이 변경
            UpdateSelectedHero();
        }
    }

    public void UpdateSelectedHero()
    {
        if (currentHero != null)
        {
            currentHeroSprite.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Heroes/" + DataManager.Instance.Hero.Get(currentHero.ID).name);
            heroName.text = DataManager.Instance.Hero.Get(currentHero.ID).name;

            heroLevel.text = currentHero.level.ToString();
            heroTranscendence.text = currentHero.transcendence.ToString();
            heroGrade.text = currentHero.grade.ToString();
        }
    }

    public void Upgrade(ref int upgradeTarget)
    {
        if (upgradeTarget < 10)
        {
            upgradeTarget += 1;
            DatabaseManager.Instance.SaveData(GameManager.Instance.heroInventory, "HeroData");
        }
    }

    public void LevelUp(int value)
    {
        if (currentHero != null)
        {
            currentHero.level += value;
            heroLevel.text = currentHero.level.ToString();
            DatabaseManager.Instance.SaveData(GameManager.Instance.heroInventory, "HeroData");
        }
    }

    public void OpenHeroTranscendence()
    {
        UIManager.Instance.Show<HeroTranscendence>("FloatingUI");
        if (currentHeroSlotNumber != -1)
        {
            UIManager.Instance.Get<HeroTranscendence>().Initialize(currentHeroSlotNumber);
        }
        else if (currentHeroSlotNumberInventory != -1)
        {
            UIManager.Instance.Get<HeroTranscendence>().InitializeInventory(currentHeroSlotNumberInventory);
        }
    }

    private void Start()
    {
        base.Initialize();
    }
}
