using System.Collections.Generic;
using UnityEngine;

class ShopUI : UIWindow
{
    [SerializeField] private CardUI _cardUIPrefab;

    [SerializeField] private UglySerializableDictionary<int, List<Transform>> _cardSpawnPointsPerNumberOfCards = new ();

    private List<CardUI> _cards = new ();

    public void RenderCards(List<CardUI.CardSetupParams> cardsParams)
    {
        var spawnPoints = _cardSpawnPointsPerNumberOfCards.ToDictionary()[cardsParams.Count];

        for (int i = 0; i < cardsParams.Count; i++)
        {
            var cardUI = Instantiate(_cardUIPrefab, spawnPoints[i].transform.position, Quaternion.identity, _uiContainer.transform);
            cardUI.SetupCard(cardsParams[i], () =>
            {
                GameManager.Instance.StartGame();
                ClearCards();
            });

            _cards.Add(cardUI);
        }
    }

    public void ClearCards() 
    {
        foreach (var card in _cards) Destroy(card.gameObject);

        _cards.Clear();
    } 
}