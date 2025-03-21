using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

class ShopUI : UIWindow
{
    [SerializeField] private MovableChain _chainTween;

    [SerializeField] private CardUI _cardUIPrefab;

    [SerializeField] private UglySerializableDictionary<int, List<Transform>> _cardSpawnPointsPerNumberOfCards = new ();

    private List<CardUI> _cards = new ();

    private void Start ()
    {
        _chainTween.OnBeforeMoveBack += () => ClearCards();
    }

    private void Update()
    {
        if (!UIRaycastManager.Instance.IsElementHit) return;

        foreach (var card in _cards)
        {
            if (UIRaycastManager.Instance.IsObjectHit(card.gameObject))
            {
                card.SetIsHoveredOver(true);

                foreach (var otherCard in _cards)
                {
                    if (otherCard != card)
                        otherCard.ChangeToOriginalState();
                }

                if (Input.GetMouseButton(0))
                    card.ChangeToHoldingState();     
                else 
                    card.ChangeToHoverState();
                
                if (Input.GetMouseButtonUp(0))
                    card.Select();
            }
            else
            {
                card.SetIsHoveredOver(false);

                if (Input.GetMouseButtonUp(0))
                    card.ChangeToOriginalState();
            }
        }
    }


    public void RenderCards(List<CardUI.CardSetupParams> cardsParams)
    {
        var spawnPoints = _cardSpawnPointsPerNumberOfCards.ToDictionary()[cardsParams.Count];

        for (int i = 0; i < cardsParams.Count; i++)
        {
            var cardUI = Instantiate(_cardUIPrefab, spawnPoints[i].transform.position, Quaternion.identity, _uiContainer.transform);
            cardUI.SetupCard(cardsParams[i], () =>
            {
                GameManager.Instance.StartGame();
            });

            _cards.Add(cardUI);
        }
    }

    public void TweenCards()
    {
        Sequence sequence = DOTween.Sequence();

        foreach (var card in _cards)
        {
            sequence.Append(card.gameObject.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.InOutExpo));
        }
    }

    public async Task ClearCards() 
    {
        Sequence sequence = DOTween.Sequence();

        foreach (var card in _cards)
        {
            sequence.Append(card.gameObject.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InOutExpo));
        }

        await sequence.AsyncWaitForCompletion();

        foreach (var card in _cards) Destroy(card.gameObject);

        _cards.Clear();
    } 
}