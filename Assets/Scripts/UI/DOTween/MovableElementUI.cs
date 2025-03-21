using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

class MovableElementUI : MonoBehaviour
{
    [SerializeField] private Vector3 _targetPosition;
    [SerializeField] private Vector3 _startPosition;
    [SerializeField] private float _duration;

    public TweenerCore<Vector3, Vector3, VectorOptions> Move()
    {
        return transform.DOLocalMove(_targetPosition, _duration).SetEase(Ease.InOutExpo).SetUpdate(isIndependentUpdate: true);
    }

    public TweenerCore<Vector3, Vector3, VectorOptions> MoveBack()
    {
        return transform.DOLocalMove(_startPosition, _duration).SetEase(Ease.InOutExpo).SetUpdate(isIndependentUpdate: true);
    }

    public void Reset()
    {
        transform.localPosition = _startPosition;
    }   
}