using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    [Header("Mouse actions")]
    [SerializeField] private Color _defaultColor = Color.white;
    [SerializeField] private Color _hoverColor = Color.gray;
    [SerializeField] private Color _holdingColor = Color.black;
    [SerializeField] private float _colorTransitionDuration = 0.1f;
    [SerializeField] private Vector3 _hoverOffset;
    [SerializeField] private Vector3 _holdingOffset;
    [SerializeField] private float _moveTransitionDuration = 0.1f;

    [Header("Punch settings")]
    [SerializeField] private Vector3 _punch = Vector3.one;
    [SerializeField] private float _punchDuration = 0.1f;
    [SerializeField] private int _punchVibrato = 2;
    [SerializeField] private float _punchElasticity = 0.5f;

    [Header("Shake settings")]
    [SerializeField] private float _shakeDuration = 0.1f;
    [SerializeField] private float _shakeStrength = 0.5f;
    [SerializeField] private float _shakeRandomness = 2;

    [Header("Card Tilt")]
    [SerializeField] private float _tiltSpeed = 1f;
    [SerializeField] private float _manualTiltAmount = 1f;
    [SerializeField] private float _autoTiltAmount = 1f;

    private Tilemap _backgroundSpriteRenderer;

    private Action _buy, _close;

    private Vector3 _originalPosition;
    private Vector3 _hoverPosition;
    private Vector3 _holdingPosition;

    private Tweener _currentColorTween;
    private Tweener _currentMoveTween;

    private bool _isHolding = false, _isHovering = false;

    private bool _isHoveredOver;

    public void SetIsHoveredOver(bool isHoveredOver) => _isHoveredOver = isHoveredOver;

    public void SetupCard(CardSetupParams setupParams, Action close)
    {
        transform.localScale = Vector3.zero;

        _rarityTextSpriteRenderer.sprite = setupParams.RarityConfig.RarityTitleSprite;
        _rarityTextSpriteRenderer.color = setupParams.RarityConfig.TextTintColor;

        _displayTextSpriteRenderer.sprite = setupParams.TitleSprite;
        _displayTextSpriteRenderer.color = setupParams.RarityConfig.TextTintColor;

        _displaySpriteSpriteRenderer.sprite = setupParams.IconSprite;

        var background = Instantiate(setupParams.RarityConfig.BackgroundSprite, this.transform);

        _backgroundSpriteRenderer = background.GetComponentInChildren<Tilemap>();

        _buy = setupParams.Buy;
        _close = close;

        _originalPosition = this.transform.position;
        _hoverPosition = _originalPosition + _hoverOffset;
        _holdingPosition = _originalPosition + _holdingOffset;
    }

    private void OnDestroy()
    {
        _currentColorTween?.Kill();
        _currentMoveTween?.Kill();
    }

    public void ChangeToHoverState()
    {
        if (_backgroundSpriteRenderer == null || _currentColorTween != null || _currentMoveTween != null || _isHovering) return;

        _isHovering = true;

        _currentColorTween = DOVirtual.Color(_backgroundSpriteRenderer.color, _hoverColor, _colorTransitionDuration, (color) => _backgroundSpriteRenderer.color = color).SetEase(Ease.InOutExpo).OnComplete(() => _currentColorTween = null);
        _currentMoveTween = transform.DOShakePosition(_shakeDuration, strength: _shakeStrength, randomness: _shakeRandomness).SetEase(Ease.InOutExpo).OnComplete(() => 
        {
            transform.DOMove(_hoverPosition, _moveTransitionDuration).SetEase(Ease.InOutExpo).OnComplete(() => _currentMoveTween = null);
        });
    }

    public void ChangeToHoldingState()
    {
        if (_backgroundSpriteRenderer == null || _currentColorTween != null || _currentMoveTween != null || _isHolding) return;

        _isHolding = true;

        _currentColorTween = DOVirtual.Color(_backgroundSpriteRenderer.color, _holdingColor, _colorTransitionDuration, (color) => _backgroundSpriteRenderer.color = color).SetEase(Ease.InOutExpo).OnComplete(() => _currentColorTween = null);
        _currentMoveTween = transform.DOPunchPosition(_punch, _punchDuration, _punchVibrato, _punchElasticity).SetEase(Ease.InOutExpo).OnComplete(() => 
        {
            transform.DOMove(_holdingPosition, _moveTransitionDuration).SetEase(Ease.InOutExpo).OnComplete(() => _currentMoveTween = null);
        });
    }

    public void ChangeToOriginalState()
    {
        if (_backgroundSpriteRenderer == null || _currentColorTween != null || _currentMoveTween != null) return;

        _isHolding = false;
        _isHovering = false;

        _currentColorTween = DOVirtual.Color(_backgroundSpriteRenderer.color, _defaultColor, _colorTransitionDuration, (color) => _backgroundSpriteRenderer.color = color).SetEase(Ease.InOutExpo).OnComplete(() => _currentColorTween = null);
        _currentMoveTween = transform.DOMove(_originalPosition, _moveTransitionDuration).SetEase(Ease.InOutExpo).OnComplete(() => _currentMoveTween = null);
    }

    private void Update() => _cardTilt();

    private void _cardTilt()
    {
        int index = 1;
        float sine = Mathf.Sin(Time.time + index) * (_isHoveredOver ? .2f : 1);
        float cosine = Mathf.Cos(Time.time + index) * (_isHoveredOver ? .2f : 1);

        Vector3 offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float tiltX = _isHoveredOver ? ((offset.y * -1) * _manualTiltAmount) : 0;
        float tiltY = _isHoveredOver ? ((offset.x) * _manualTiltAmount) : 0;

        float lerpX = Mathf.LerpAngle(transform.eulerAngles.x, tiltX + (sine * _autoTiltAmount), _tiltSpeed * Time.deltaTime);
        float lerpY = Mathf.LerpAngle(transform.eulerAngles.y, tiltY + (cosine * _autoTiltAmount), _tiltSpeed * Time.deltaTime);

        transform.eulerAngles = new Vector3(lerpX, lerpY, 0);
    }

    public void Select()
    {
        _buy();
        _close();
    }
}