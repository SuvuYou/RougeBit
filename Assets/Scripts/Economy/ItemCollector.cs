using UnityEngine;

class ItemCollector : MonoBehaviour
{
    [SerializeField] private InventorySO _inventory;

    [SerializeField] private Transform _targetTransform;

    [SerializeField] private float _radiousOfPulling;
    [SerializeField] private float _radiousOfConsumsion;

    [SerializeField] private float _approachSpeed;

    [SerializeField] private LayerMask _itemsLayerMask;

    private void Update()
    {
        Collider2D[] items = Physics2D.OverlapCircleAll(_targetTransform.position, _radiousOfPulling, _itemsLayerMask);

        foreach (var item in items)
        {
            if (item.TryGetComponent(out CollectableItem collectableItem))
            {
                float distance = Vector2.Distance(_targetTransform.position, item.transform.position);

                if (distance <= _radiousOfConsumsion)
                {
                    _inventory.AddItem(collectableItem);
                    Destroy(collectableItem.gameObject);

                    return;
                }

                // Determines the percentage of distance to target
                float t = 1 - (distance / _radiousOfPulling);

                collectableItem.transform.position = Vector2.Lerp(collectableItem.transform.position, _targetTransform.position, t * Time.deltaTime * _approachSpeed);
            }
        }
    }
}