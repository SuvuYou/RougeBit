using UnityEngine;

class ItemDropper : MonoBehaviour
{
    [SerializeField] private CollectableItemSO _item;

    public void DropItem()
    {
        _instantiateItemAt(transform.position);
    }

    public void DropItem(Vector3 dropPosition)
    {
        _instantiateItemAt(dropPosition);
    }

    private void _instantiateItemAt(Vector3 position) 
    {
        var item = Instantiate(_item.ItemPrefab, position, Quaternion.identity);
        item.Initialize(_item.XPValue);

        CollectablesManager.Instance.AddCollectableItem(item);
    }
}