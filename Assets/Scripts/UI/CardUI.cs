using System;
using UnityEngine;

class CardUI : MonoBehaviour
{
    public struct CardSetupParams
    {
        public Sprite IconSprite;
        public Sprite TitleSprite;
        public CardRarityConfig RarityConfig;
        public Action Buy;
    }

    [SerializeField] private SpriteRenderer _rarityTextSpriteRenderer;
    [SerializeField] private SpriteRenderer _displayTextSpriteRenderer;
    [SerializeField] private SpriteRenderer _displaySpriteSpriteRenderer;

    private Action _buy, _close;

    public void SetupCard(CardSetupParams setupParams, Action close)
    {
        _rarityTextSpriteRenderer.sprite = setupParams.RarityConfig.RarityTitleSprite;
        _rarityTextSpriteRenderer.color = setupParams.RarityConfig.TextTintColor;

        _displayTextSpriteRenderer.sprite = setupParams.TitleSprite;
        _displayTextSpriteRenderer.color = setupParams.RarityConfig.TextTintColor;

        _displaySpriteSpriteRenderer.sprite = setupParams.IconSprite;

        Instantiate(setupParams.RarityConfig.BackgroundSprite, this.transform);

        _buy = setupParams.Buy;
        _close = close;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D[] hit = Physics2D.RaycastAll(ray.origin, ray.direction);

            if (hit.Length == 0)
            {
                return;
            }

            foreach (RaycastHit2D h in hit)
            {
                if (h.collider != null && h.collider.gameObject == this.gameObject)
                {
                    _buy();
                    _close();
                }
            }
        }
    }
}