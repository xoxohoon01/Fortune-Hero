using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataTable_FortuneHero;
using UnityEngine.AI;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;

public class StageManager : MonoSingleton<StageManager>
{
    public int currentStage;
    public bool isStart { get; private set; }

    public GameObject[] heroObjects { get; private set; }
    public List<GameObject> monsterObjects { get; private set; }

    public void Initialize()
    {
        currentStage = 1;
        heroObjects = new GameObject[4];
        monsterObjects = new List<GameObject>();

        Invoke("StartStage", 3f);
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
                HeroController hero = ObjectPoolManager.Instance.GetObject("Units/Hero").GetComponent<HeroController>();
                hero.transform.position = spawnPos;

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


            MonsterController monster = ObjectPoolManager.Instance.GetObject("Units/Monster").GetComponent<MonsterController>();
            monster.transform.position = spawnPos;
            monster.transform.LookAt(Vector3.zero);

            monster.Initialize(DataManager.Instance.Monster.Get(stage.monsterID), stage.spawnLevel);
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
        isStart = false;
        DespawnAll();
        CancelInvoke("StartStage");
    }

    public void FailStage()
    {
        if (isStart)
        {
            int count = 0;
            for (int i = 0; i < 4; i++)
            {
                if (heroObjects[i] != null)
                {
                    count++;
                    break;
                }
            }

            if (count <= 0)
            {
                Debug.Log("실패함");
                // 이전 스테이지가 있다면 이전 스테이지로
                if (DataManager.Instance.Stage.Get(3000 + currentStage - 1) != null)
                    ChangeStage(currentStage - 1);
                else
                    ChangeStage(currentStage);
                isStart = false;
                return;
            }
        }
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
                        Debug.Log("클리어함");
                        StageData stageData = DataManager.Instance.Stage.Get(3000 + currentStage);
                        GameManager.Instance.GetItems(stageData.rewardItems);
                        GameManager.Instance.itemInventory.gold += stageData.rewardGold;
                        isStart = false;
                        DespawnMonster();
                        
                        // 다음 스테이지가 있다면 다음 스테이지로
                        if (DataManager.Instance.Stage.Get(3000 + currentStage + 1) != null)
                        {
                            ChangeStage(currentStage + 1);
                        }
                        else
                        {
                            ChangeStage(currentStage);
                        }

                        DatabaseManager.Instance.SaveData(GameManager.Instance.playerStage, "StageData");
                        return;
                    }
                }
            }
        }
    }

    public void ChangeStage(int stage)
    {
        currentStage = stage;
        GameManager.Instance.playerStage.currentStage = currentStage;
        DatabaseManager.Instance.SaveData(GameManager.Instance.playerStage, "StageData");

        Invoke("StartStage", 3f);
    }

    private void Update()
    {
        FailStage();
        ClearStage();
    }
}
