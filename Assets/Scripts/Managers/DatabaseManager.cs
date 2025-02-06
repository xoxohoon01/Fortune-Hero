using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class DatabaseManager : Singleton<DatabaseManager>
{
    public void SaveData<T>(T data, string fileName)
    {
        string newData = JsonConvert.SerializeObject(data, Formatting.Indented);
        string path = Path.Combine(Application.persistentDataPath, fileName + ".json");
        File.WriteAllText(path, newData);
    }

    public T LoadData<T>(string fileName)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName + ".json");
        if (File.Exists(path))
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
        else
            return default;
    }
}
