using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

class ShopController : MonoBehaviour
{
    [SerializeField] private UglySerializableDictionary<Rarity, CardRarityConfig> _cardRarityConfigs;

    [SerializeField] private ShopUI _shopUI;
    [SerializeField] private Buyer _buyer;

    public UnityEvent OnItemsPurchased;

    public void DisplayItems()
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
                    Buy = () =>
                    {
                        _buyer.Purchace(item);

                        OnItemsPurchased?.Invoke();
                    }
                });
            }

            _shopUI.RenderCards(cardsParams);
        }
    }
}