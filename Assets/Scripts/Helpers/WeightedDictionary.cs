using System;

[Serializable]
public class WeightedDictionary<TItem> : SerializableDictionary<TItem, float>
{
    public void AddItem(TItem item, float weight)
    {
        if (weight <= 0) return; // Ignore zero or negative weights
        this[item] = weight;
    }

    public TItem GetRandomItem()
    {
        float totalWeight = 0f;
        foreach (var weight in this.Values)
        {
            totalWeight += weight;
        }

        float randomValue = UnityEngine.Random.Range(0, totalWeight);
        float cumulativeWeight = 0f;

        foreach (var kvp in this)
        {
            cumulativeWeight += kvp.Value;
            if (randomValue <= cumulativeWeight)
                return kvp.Key;
        }

        return default;
    }
}