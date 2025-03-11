using UnityEngine;

class Shop : MonoBehaviour
{
    [SerializeField] private Buyer _buyer = null;

    private void _displayItems()
    {
        if (_buyer.TrySelectRandomItemToBuy(out UpgradablesBundleController item))
        {
            // Debug.Log(item.GetNextLevelInfo().Title);
            _buyer.Purchace(item);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) _displayItems();
    }
}