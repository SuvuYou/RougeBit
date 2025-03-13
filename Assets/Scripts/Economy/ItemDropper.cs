using UnityEngine;

class ItemDropper : MonoBehaviour
{
    [SerializeField] private CollectableItemSO _item;

    public void DropItem()
    {
        var item = Instantiate(_item.ItemPrefab, transform.position, Quaternion.identity);
        item.Initialize(_item.XPValue);
    }

    public void DropItem(Vector3 dropPosition)
    {
        var item = Instantiate(_item.ItemPrefab, dropPosition, Quaternion.identity);
        item.Initialize(_item.XPValue);
    }
}