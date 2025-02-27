using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/InventorySO")]
class InventorySO : ScriptableObject
{
    public Dictionary<CollectableItem, int> Items = new ();

    public void AddItem(CollectableItem item, int amount = 1) 
    {
        if (Items.ContainsKey(item)) 
        {
            Items[item] += amount;
        } 
        else 
        {
            Items.Add(item, amount);
        }
    } 
}