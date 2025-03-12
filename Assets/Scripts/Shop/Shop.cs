using UnityEngine;

class Shop : MonoBehaviour
{
    [SerializeField] private SerializableDictionary<Rarity, CardRarityConfig> _cardRarityConfigs;
    [SerializeField] private Buyer _buyer = null;

    private void _displayItems()
    {
        if (_buyer.TrySelectRandomItemToBuy(out UpgradablesBundleController item))
        {
            // item.GetNextLevelInfo().Title;
            // item.GetNextLevelInfo().IconSprite;
            // item.GetNextLevelInfo().TitleSprite;
            // item.GetNextLevelInfo().UpgradeRarity;

            // _cardRarityConfigs[item.GetNextLevelInfo().UpgradeRarity];

            // _buyer.Purchace(item);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) _displayItems();
    }
}