using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataTable_FortuneHero;
using UnityEngine.AI;
using System.Runtime.CompilerServices;

public class StageManager : MonoSingleton<StageManager>
{
    public int currentStage;
    private bool isStart;

    public GameObject[] heroObjects { get; private set; }
    public List<GameObject> monsterObjects { get; private set; }

    public void Initialize()
    {
        currentStage = 1;
        heroObjects = new GameObject[4];
        monsterObjects = new List<GameObject>();
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
                HeroController hero = Instantiate(Resources.Load<GameObject>("Units/Hero"), spawnPos, Quaternion.Euler(0, 0, 0)).GetComponent<HeroController>();
                hero.Initialize(currentHero);
                heroObjects[i] = hero.gameObject;
            }
                
        }
    }

    public void SpawnMonster()
    {
        isStart = true;

        StageData stage = DataManager.Instance.Stage.Get(3000 + currentStage);

        float offsetAngle = Random.Range(0.0f, 360.0f);
        Vector3 offset = new Vector3(
            Mathf.Cos(offsetAngle) * stage.spawnRadius,
            0,
            Mathf.Sin(offsetAngle) * stage.spawnRadius);

        for (int i = 0; i < stage.spawnCount; i++)
        {
            float angle = i * (Mathf.PI * 2 / stage.spawnCount);
            Vector3 spawnPos = new Vector3(
                offset.x + Mathf.Cos(angle) * 3,
                0,
                offset.z + Mathf.Sin(angle) * 3
            );

            MonsterController monster = Instantiate(Resources.Load<GameObject>("Units/Monster"), spawnPos, Quaternion.identity).GetComponent<MonsterController>();
            monster.Initialize(DataManager.Instance.Monster.Get(stage.monsterID));
            monster.transform.LookAt(Vector3.zero);

            monsterObjects.Add(monster.gameObject);
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
        CancelInvoke("SpawnMonster");
        Invoke("SpawnMonster", 1f);

        CameraManager.Instance.SetTarget();
    }

    public void EndStage()
    {
        DespawnMonster();
    }

    public void ClearStage()
    {
        if (isStart)
        {
            if (monsterObjects.Count <= 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (heroObjects[i] != null)
                    {
                        Debug.Log("Å¬¸®¾î!");
                        isStart = false;
                        EndStage();
                        currentStage++;
                        Invoke("StartStage", 3);
                        return;
                    }
                }
            }
        }
    }

    private void Update()
    {
        ClearStage();
    }
}
