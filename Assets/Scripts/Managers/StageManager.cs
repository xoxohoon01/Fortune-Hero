using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataTable_FortuneHero;
using UnityEngine.AI;

public class StageManager : MonoSingleton<StageManager>
{
    public int currentStage;

    private GameObject[] heroObjects = new GameObject[4];
    private List<GameObject> monsterObjects = new List<GameObject>();

    public void Initialize()
    {
        currentStage = 1;
    }


    public void SpawnHero()
    {
        for (int i = 0; i < 4; i++)
        {
            Hero currentHero = null;
            if (GameManager.Instance.heroInventory.hero[i] != null)
                currentHero = GameManager.Instance.heroInventory.hero[i];

            float angle = i * (Mathf.PI * 2 / 4) + (Mathf.Deg2Rad * 90);
            Vector3 spawnPos = new Vector3(
                Mathf.Cos(angle) * 2,
                0,
                 Mathf.Sin(angle) * 2
            );

            if (currentHero != null)
            {
                HeroController hero = Instantiate(Resources.Load<GameObject>(DataManager.Instance.Hero.Get(currentHero.ID).prefabPath), spawnPos, Quaternion.Euler(0, 0, 0)).GetComponent<HeroController>();
                hero.Initialize(currentHero);
                heroObjects[i] = hero.gameObject;
            }
                
        }
    }

    public void SpawnMonster()
    {
        StageData stage = DataManager.Instance.Stage.Get(3000 + currentStage);
        for (int i = 0; i < stage.spawnCount; i++)
        {
            float angle = i * (Mathf.PI * 2 / stage.spawnCount);
            Vector3 spawnPos = new Vector3(
                Mathf.Cos(angle) * stage.spawnRadius,
                0,
                 Mathf.Sin(angle) * stage.spawnRadius
            );

            GameObject monster = Instantiate(Resources.Load<GameObject>(DataManager.Instance.Monster.Get(stage.monsterID).prefabPath), spawnPos, Quaternion.identity);
            monster.transform.LookAt(Vector3.zero);

            monsterObjects.Add(monster);
        }
    }


    public void DespawnAll()
    {
        for (int i = 0; i < 4; i++)
        {
            if (heroObjects[i] != null)
            {
                Destroy(heroObjects[i]);
            }
        }
        foreach (GameObject monster in monsterObjects)
        {
            Destroy(monster);
        }
        monsterObjects.Clear();
    }

    public void DespawnHero()
    {
        for (int i = 0; i < 4; i++)
        {
            if (heroObjects[i] != null)
            {
                Destroy(heroObjects[i]);
            }
        }
    }

    public void DespawnMonster()
    {
        foreach (GameObject monster in monsterObjects)
        {
            Destroy(monster);
        }
        monsterObjects.Clear();
    }


    public void StartStage()
    {
        DespawnAll();

        SpawnHero();
        Invoke("SpawnMonster", 1f);
    }

    public void EndStage()
    {
        DespawnAll();
    }

    public void ClearStage()
    {

    }

}
