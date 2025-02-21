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
        UpdateSelectedHero();
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

    public void ChangeHero(int slotNumber)
    {
        if (upgradeSlots == null)
        {
            Initialize();
        }

        if (GameManager.Instance.heroInventory.hero[slotNumber] != null)
        {
            selectedHero = slotNumber;

            if (selectedHero != -1)
            {
                scroll.gameObject.SetActive(true);

                // 선택한 영웅 디스플레이 변경
                UpdateSelectedHero();

                // 업데이트
                upgradeSlots[0].ChangeHero(GameManager.Instance.heroInventory.hero[selectedHero].hpUpgrade);
                upgradeSlots[1].ChangeHero(GameManager.Instance.heroInventory.hero[selectedHero].physicalDamageUpgrade);
                upgradeSlots[2].ChangeHero(GameManager.Instance.heroInventory.hero[selectedHero].magicalDamageUpgrade);
                upgradeSlots[3].ChangeHero(GameManager.Instance.heroInventory.hero[selectedHero].physicalArmorUpgrade);
                upgradeSlots[4].ChangeHero(GameManager.Instance.heroInventory.hero[selectedHero].magicalArmorUpgrade);

                // 업그레이드 델리게이트 연결
                upgradeSlots[0].upgradeDelegate = () => { Upgrade(ref GameManager.Instance.heroInventory.hero[selectedHero].hpUpgrade); upgradeSlots[0].ChangeHero(GameManager.Instance.heroInventory.hero[selectedHero].hpUpgrade); };
                upgradeSlots[1].upgradeDelegate = () => { Upgrade(ref GameManager.Instance.heroInventory.hero[selectedHero].physicalDamageUpgrade); upgradeSlots[1].ChangeHero(GameManager.Instance.heroInventory.hero[selectedHero].physicalDamageUpgrade); };
                upgradeSlots[2].upgradeDelegate = () => { Upgrade(ref GameManager.Instance.heroInventory.hero[selectedHero].magicalDamageUpgrade); upgradeSlots[2].ChangeHero(GameManager.Instance.heroInventory.hero[selectedHero].magicalDamageUpgrade); };
                upgradeSlots[3].upgradeDelegate = () => { Upgrade(ref GameManager.Instance.heroInventory.hero[selectedHero].physicalArmorUpgrade); upgradeSlots[3].ChangeHero(GameManager.Instance.heroInventory.hero[selectedHero].physicalArmorUpgrade); };
                upgradeSlots[4].upgradeDelegate = () => { Upgrade(ref GameManager.Instance.heroInventory.hero[selectedHero].magicalArmorUpgrade); upgradeSlots[4].ChangeHero(GameManager.Instance.heroInventory.hero[selectedHero].magicalArmorUpgrade); };
            }
        }
    }

    public void UpdateSelectedHero()
    {
        if (selectedHero != -1)
        {
            currentHero.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Heroes/" + DataManager.Instance.Hero.Get(GameManager.Instance.heroInventory.hero[selectedHero].ID).name);
            heroName.text = DataManager.Instance.Hero.Get(GameManager.Instance.heroInventory.hero[selectedHero].ID).name;
        }
        else
        {
            currentHero.transform.GetChild(0).GetComponent<Image>().sprite = null;
            heroName.text = "";
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
        UpdateSelectedHero();
    }
}
