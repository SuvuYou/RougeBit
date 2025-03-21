using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;

class ButtonUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Tilemap _backgroundSpriteRenderer;

    [Header("Button interaction settings")]
    [SerializeField] private Color _defaultColor = Color.white;
    [SerializeField] private Color _hoverColor = Color.gray;
    [SerializeField] private Color _holdingColor = Color.black;
    [SerializeField] private float _colorTransitionDuration = 0.1f;

    private Action _click;

    private Tweener _currentColorTween;

    public void SetupButton(Action onClick)
    {
        _click = onClick;
    }

    private void Update()
    {
        if (!UIRaycastManager.Instance.IsElementHit) return;

        if (UIRaycastManager.Instance.IsObjectHit(this.gameObject))
        {
            if (Input.GetMouseButton(0))
                this.ChangeToHoldingState();     
            else 
                this.ChangeToHoverState();
            
            if (Input.GetMouseButtonUp(0))
                _click();
        }
        else
        {
            this.ChangeToOriginalState();
        }
    }

    private void OnDestroy() => _currentColorTween?.Kill();

    public void ChangeToHoverState()
    {
        if (_backgroundSpriteRenderer == null) return;

        if (_currentColorTween != null) return;

        _currentColorTween = DOVirtual.Color(_backgroundSpriteRenderer.color, _hoverColor, _colorTransitionDuration, (color) => _backgroundSpriteRenderer.color = color).SetEase(Ease.OutExpo).OnComplete(() => _currentColorTween = null).SetUpdate(isIndependentUpdate: true);
    }

    public void ChangeToHoldingState()
    {
        if (_backgroundSpriteRenderer == null) return;

        if (_currentColorTween != null) return;

        _currentColorTween = DOVirtual.Color(_backgroundSpriteRenderer.color, _holdingColor, _colorTransitionDuration, (color) => _backgroundSpriteRenderer.color = color).SetEase(Ease.OutExpo).OnComplete(() => _currentColorTween = null).SetUpdate(isIndependentUpdate: true);
    }

    public void ChangeToOriginalState()
    {
        if (_backgroundSpriteRenderer == null) return;

        if (_currentColorTween != null) return;

        _currentColorTween = DOVirtual.Color(_backgroundSpriteRenderer.color, _defaultColor, _colorTransitionDuration, (color) => _backgroundSpriteRenderer.color = color).SetEase(Ease.OutExpo).OnComplete(() => _currentColorTween = null).SetUpdate(isIndependentUpdate: true);
    }
}