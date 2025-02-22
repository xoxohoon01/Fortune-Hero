using System.Collections.Generic;
using UnityEngine;

public class WeightedRandom
{
    private List<KeyValuePair<int, float>> candidates;
    private float totalWeight;

    public float lastRandomValue { get; private set; }
    public float lastItem { get; private set; }

    public WeightedRandom(List<KeyValuePair<int, float>> list)
    {
        candidates = new List<KeyValuePair<int, float>>();
        totalWeight = 0;

        foreach (KeyValuePair<int, float> pair in list)
        {
            totalWeight += pair.Value;
        }

        foreach (KeyValuePair<int, float> pair in list)
        {
            candidates.Add(new KeyValuePair<int, float>(pair.Key, pair.Value / totalWeight));
        }

        candidates.Sort((key, value) => key.Value.CompareTo(value.Value));
    }

    public int GetValue()
    {
        float randomValue = Random.Range(0.0f, 1.0f);
        float currentValue = 0;

        foreach (var item in candidates)
        {
            currentValue += item.Value;
            if (randomValue <= currentValue)
            {
                lastRandomValue = randomValue;
                lastItem = item.Key;
                //Debug.Log($"Total Weight: {totalWeight}, RandomValue: {randomValue}, currentValue: {currentValue}, item: {item.Key}");
                return item.Key;
            }
        }

        //Debug.Log($"Total Weight: {totalWeight}, RandomValue: {randomValue}, currentValue: {currentValue}");
        return default;
    }
}
