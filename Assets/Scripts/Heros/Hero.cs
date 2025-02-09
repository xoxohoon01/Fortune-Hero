using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Hero
{
    public int ID; // HeroData ID
    public int level; // 현재 레벨
    public int grade; // 현재 등급

    public Item Weapon;
    public Item Glove;
    public Item Ring;
    public Item Neckless;
    public Item Helmet;
    public Item Top;
    public Item Bottom;
    public Item Shoes;
    public Item Artifact;

    // 업그레이드 상황
    public int hpUpgrade;
    public int physicalDamageUpgrade;
    public int physicalArmorUpgrade;
    public int magicalDamageUpgrade;
    public int magicalArmorUpgrade;
    public int attackSpeedUpgrade;
    public int moveSpeedUpgrade;

    public Hero(int id = 0)
    {
        ID = id;
        if (id != 0)
        {
            level = 1;
            grade = 1;
        }
        else
        {
            level = 0;
            grade = 0;
        }
    }
}
