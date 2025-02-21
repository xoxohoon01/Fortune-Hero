using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroUpgradeSlot : UIBase
{
    public Image upgradeImage;
    public TMP_Text title;
    public TMP_Text currentUpgradeText;
    public TMP_Text maxUpgradeText;

    private int maxUpgrade;
    private int currentUpgrade;

    public delegate void UpgradeDelegate();
    public UpgradeDelegate upgradeDelegate;

    public void Initialize(string target, int max)
    {
        title.text = target;
        maxUpgradeText.text = max.ToString();
        maxUpgrade = max;
        currentUpgradeText.text = currentUpgrade.ToString();
        UpdateSlot();
    }

    public void Upgrade()
    {
        upgradeDelegate?.Invoke();
    }

    public void ChangeHero(int upgrade)
    {
        currentUpgrade = upgrade;
        UpdateSlot();
    }

    public void UpdateSlot()
    {
        currentUpgradeText.text = currentUpgrade.ToString();
    }
}
