using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroUpgrade : UIBase
{
    public TMP_Text heroName;
    public Image currentHero;
    public ScrollRect scroll;

    private int selectedHero = -1;

    private List<HeroUpgradeSlot> upgradeSlots;

    public delegate void UpgradeDelegate();
    public UpgradeDelegate upgradeDelegate;

    public override void Hide()
    {
        selectedHero = -1;
        UpdateSelectedHero(null);
    }

    public override void Initialize()
    {
        base.Initialize();

        upgradeSlots = new List<HeroUpgradeSlot>
        {
            Instantiate(Resources.Load<GameObject>("UI/HeroUpgradeSlot"), scroll.content).GetComponent<HeroUpgradeSlot>(),
            Instantiate(Resources.Load<GameObject>("UI/HeroUpgradeSlot"), scroll.content).GetComponent<HeroUpgradeSlot>(),
            Instantiate(Resources.Load<GameObject>("UI/HeroUpgradeSlot"), scroll.content).GetComponent<HeroUpgradeSlot>(),
            Instantiate(Resources.Load<GameObject>("UI/HeroUpgradeSlot"), scroll.content).GetComponent<HeroUpgradeSlot>(),
            Instantiate(Resources.Load<GameObject>("UI/HeroUpgradeSlot"), scroll.content).GetComponent<HeroUpgradeSlot>()
        };
        upgradeSlots[0].Initialize("HP", 10);
        upgradeSlots[1].Initialize("PhysicalDamage", 10);
        upgradeSlots[2].Initialize("MagicalDamage", 10);
        upgradeSlots[3].Initialize("PhysicalArmor", 10);
        upgradeSlots[4].Initialize("MagicalArmor", 10);
    }

    public void ChangeHero(Hero hero)
    {
        if (upgradeSlots == null)
        {
            Initialize();
        }

        if (hero != null)
        {
            scroll.gameObject.SetActive(true);

            // 선택한 영웅 디스플레이 변경
            UpdateSelectedHero(hero);

            // 업데이트
            upgradeSlots[0].ChangeHero(hero.hpUpgrade);
            upgradeSlots[1].ChangeHero(hero.physicalDamageUpgrade);
            upgradeSlots[2].ChangeHero(hero.magicalDamageUpgrade);
            upgradeSlots[3].ChangeHero(hero.physicalArmorUpgrade);
            upgradeSlots[4].ChangeHero(hero.magicalArmorUpgrade);

            // 업그레이드 델리게이트 연결
            upgradeSlots[0].upgradeDelegate = () => { Upgrade(ref hero.hpUpgrade); upgradeSlots[0].ChangeHero(hero.hpUpgrade); };
            upgradeSlots[1].upgradeDelegate = () => { Upgrade(ref hero.physicalDamageUpgrade); upgradeSlots[1].ChangeHero(hero.physicalDamageUpgrade); };
            upgradeSlots[2].upgradeDelegate = () => { Upgrade(ref hero.magicalDamageUpgrade); upgradeSlots[2].ChangeHero(hero.magicalDamageUpgrade); };
            upgradeSlots[3].upgradeDelegate = () => { Upgrade(ref hero.physicalArmorUpgrade); upgradeSlots[3].ChangeHero(hero.physicalArmorUpgrade); };
            upgradeSlots[4].upgradeDelegate = () => { Upgrade(ref hero.magicalArmorUpgrade); upgradeSlots[4].ChangeHero(hero.magicalArmorUpgrade); };
        }
    }

    public void UpdateSelectedHero(Hero hero)
    {
        if (hero != null)
        {
            currentHero.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Heroes/" + DataManager.Instance.Hero.Get(hero.ID).name);
            heroName.text = DataManager.Instance.Hero.Get(hero.ID).name;
        }
        else
        {

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

    private void Start()
    {
        if (upgradeSlots == null)
            Initialize();
    }
}
