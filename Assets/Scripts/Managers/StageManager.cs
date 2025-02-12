using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataTable_FortuneHero;

public class StageManager : MonoSingleton<StageManager>
{
    public int currentStage;

    public void Initialize()
    {
        currentStage = 1;
    }

    public void SpawnPlayer()
    {

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

            Instantiate(Resources.Load<GameObject>(DataManager.Instance.Monster.Get(stage.monsterID).prefabPath), spawnPos, Quaternion.identity).transform.LookAt(Vector3.zero);
        }
    }

    public void StartStage()
    {
        SpawnPlayer();
        SpawnMonster();
    }

}
