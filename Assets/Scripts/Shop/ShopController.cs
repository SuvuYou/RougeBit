using System.Collections.Generic;
using UnityEngine;

class ShopController : MonoBehaviour
{
    [SerializeField] private UglySerializableDictionary<Rarity, CardRarityConfig> _cardRarityConfigs;

    [SerializeField] private ShopUI _shopUI;
    [SerializeField] private Buyer _buyer;

    private void _displayItems()
    {
        if (_buyer.TrySelectRandomItemsToBuy(3, out List<UpgradablesBundleController> items))
        {
            List<CardUI.CardSetupParams> cardsParams = new ();

            foreach (var item in items)
            {
                cardsParams.Add(new CardUI.CardSetupParams()
                {
                    RarityConfig = _cardRarityConfigs.ToDictionary()[item.GetNextLevelInfo().UpgradeRarity],
                    IconSprite = item.GetNextLevelInfo().IconSprite,
                    TitleSprite = item.GetNextLevelInfo().TitleSprite,
                    Buy = () => _buyer.Purchace(item)
                });
            }

            _shopUI.RenderCards(cardsParams);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) _displayItems();
    }
}