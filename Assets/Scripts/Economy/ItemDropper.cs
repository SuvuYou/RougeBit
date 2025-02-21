using UnityEngine;

class ItemDropper : MonoBehaviour
{
    [SerializeField] private CollectableItemSO _item;

    public void DropItem()
    {
        Instantiate(_item.ItemPrefab, transform.position, Quaternion.identity);
    }

    public void DropItem(Vector3 dropPosition)
    {
        Instantiate(_item.ItemPrefab, dropPosition, Quaternion.identity);
    }
}