using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Unity.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ObjectPoolManager : MonoSingleton<ObjectPoolManager>
{
    public Dictionary<string, ObjectPool<GameObject>> pools;

    public void Initialize()
    {
        pools = new Dictionary<string, ObjectPool<GameObject>>();
        foreach (var monster in DataManager.Instance.Monster.Get())
        {
            AddPool(monster.prefabPath);
        }

        foreach (var hero in DataManager.Instance.Hero.Get())
        {
            AddPool(hero.prefabPath);
        }

        AddPool("Projectiles/Arrow");
        AddPool("Projectiles/Fireball");

        AddPool("Units/Hero");
        AddPool("Units/Monster");
        AddPool("Units/Skill");
    }

    public void AddPool(string prefabPath)
    {
        Addressables.LoadAssetAsync<GameObject>($"Assets/Prefabs/{prefabPath}.prefab").Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject newGameObject = handle.Result;

                // ������Ʈ Ǯ �ʱ�ȭ
                ObjectPool<GameObject> newPool = new ObjectPool<GameObject>(
                    createFunc: () => Instantiate(newGameObject), // Addressable�� �ҷ��� ������ �ν��Ͻ�ȭ
                    actionOnGet: obj => obj.SetActive(true),
                    actionOnRelease: obj => obj.SetActive(false),
                    actionOnDestroy: obj => Destroy(obj),
                    collectionCheck: false, // �÷��� üũ (������)
                    defaultCapacity: 10,    // �⺻ Ǯ ũ��
                    maxSize: 20             // �ִ� Ǯ ũ��
                );

                pools.TryAdd(prefabPath, newPool);
            }
            else
            {
                Debug.LogError("������ �ε� ����!");
            }
        };
    }

    public GameObject GetObject(string poolName, Transform parent = null)
    {
        GameObject newGameObject = pools[poolName].Get();
        if (parent != null)
        {
            newGameObject.transform.SetParent(parent, false);
        }
        return newGameObject;
    }

    public void GetObject(GameObject newGameObject)
    {
        newGameObject.SetActive(true);
    }
}
