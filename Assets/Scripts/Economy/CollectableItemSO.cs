using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Collectable Item")]
class CollectableItemSO : ScriptableObject
{
    [field: SerializeField] public CollectableItem ItemPrefab { get; private set; }
    [field: SerializeField] public int XPValue { get; private set; }
}