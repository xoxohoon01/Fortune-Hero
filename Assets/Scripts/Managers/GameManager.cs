using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class GameManager : MonoSingleton<GameManager>
{
    public HeroInventoryData heroInventory;

    public void Initialize()
    {
        DataManager.Instance.Initialize();
        UIManager.Instance.Initialize();

        HeroInventoryData data = DatabaseManager.Instance.LoadData<HeroInventoryData>("HeroData");
        if (data != null)
        {
            heroInventory = data;
        }
        else
        {
            heroInventory = new HeroInventoryData();
            heroInventory.heroDatas = new List<Hero>();
            for (int i = 0; i < 30; i++)
                heroInventory.heroDatas.Add(new Hero());
        }

        DatabaseManager.Instance.SaveData(heroInventory, "HeroData");
    }

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UIManager.Instance.uiObjectDictionary.TryGetValue("HeroInventory", out GameObject ui);
            if (ui != null && ui.gameObject.activeInHierarchy)
                UIManager.Instance.Hide<HeroInventory>();
            else
            {
                UIManager.Instance.Show<HeroInventory>();
            }
        }
    }

}
