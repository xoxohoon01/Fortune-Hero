using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataTable_FortuneHero;

[System.Serializable]
public class Status
{
    public float hp;
    public float physicalDamage;
    public float magicalDamage;
    public float physicalArmor;
    public float magicalArmor;
    public float attackSpeed;
    public float moveSpeed;
    public float attackRange;

    public Status(Hero hero)
    {
        HeroData heroData = DataManager.Instance.Hero.Get(hero.ID);
        physicalDamage = heroData.physicalDamage + (heroData.physicalDamage_PerLevel * hero.level) + (heroData.physicalDamage_PerGrade * hero.grade);
        magicalDamage = heroData.magicalDamage + (heroData.magicalDamage_PerLevel * hero.level) + (heroData.magicalDamage_PerGrade * hero.grade);
        physicalArmor = heroData.physicalArmor + (heroData.physicalArmor_PerLevel * hero.level) + (heroData.physicalArmor_PerGrade * hero.grade);
        magicalArmor = heroData.magicalArmor + (heroData.magicalArmor_PerLevel * hero.level) + (heroData.magicalArmor_PerGrade * hero.grade);
        attackSpeed = heroData.attackSpeed;
        moveSpeed = heroData.moveSpeed;
        attackRange = heroData.attackRange;

        hp = heroData.hp + (heroData.hp_PerLevel * hero.level) + (heroData.hp_PerGrade * hero.grade);
    }

    public Status(Monster monster)
    {
        MonsterData monsterData = DataManager.Instance.Monster.Get(monster.ID);
        physicalDamage = monsterData.physicalDamage + (monsterData.physicalDamage_PerLevel * monster.level);
        magicalDamage = monsterData.magicalDamage + (monsterData.magicalDamage_PerLevel * monster.level);
        physicalArmor = monsterData.physicalArmor + (monsterData.physicalArmor_PerLevel * monster.level);
        magicalArmor = monsterData.magicalArmor + (monsterData.magicalArmor_PerLevel * monster.level);
        attackSpeed = monsterData.attackSpeed;
        moveSpeed = monsterData.moveSpeed;
        attackRange = monsterData.attackRange;

        hp = monsterData.hp + (monsterData.hp_PerLevel * monster.level);
    }
}
