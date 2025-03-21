using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> _levelBars;
    [SerializeField] private Color _completedColor;
    [SerializeField] private Color _notCompletedColor;

    [SerializeField] private float _tweenDuration = 0.2f;

    private int _cachedLevel = -1;

    private Sequence _currentSequence;

    private void Awake()
    {
        for (int i = 0; i < _levelBars.Count; i++)
        {
            _levelBars[i].color = _notCompletedColor;
        }
    }

    private void Update()
    {
        if (GameManager.Instance.GameLevels.CurrentLevelNumber == _cachedLevel) return;

        _cachedLevel = GameManager.Instance.GameLevels.CurrentLevelNumber;

        _currentSequence?.Kill();

        _currentSequence = DOTween.Sequence();

        for (int i = _levelBars.Count - 1; i >= 0; i--)
        {
            _currentSequence.Append(_levelBars[i].DOColor(i < _cachedLevel ? _completedColor : _notCompletedColor, _tweenDuration));
        }
    }

    private void OnDestroy() => _currentSequence?.Kill();
}