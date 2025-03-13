using System.Collections;
using UnityEngine;

class BarUI : MonoBehaviour
{
    [Header("Transform values")]
    [SerializeField] private float _minXScale, _maxXScale;

    [Header("Value config")]
    [SerializeField] private FloatRangeValue _valueRange;

    [Header("Interpolation")]
    [SerializeField] private bool _shouldInterpolate = false;
    [SerializeField] private float _interpolationTime = 0.5f;

    private float _percentageFilled;

    public float PercentageFilled { get => _percentageFilled;  private set { _percentageFilled = Mathf.Clamp(value, 0f, 1f); } }

    private float _displayerPercentageFilled;

    private void Start()
    {
        _valueRange.OnValueChanged += _setValue;
    }

    private void _setValue(float value)
    {
        _setPercentageFilled(value / _valueRange.Config.MaxValue);
    }

    private void _setPercentageFilled(float percentageFilled)
    {
        PercentageFilled = percentageFilled;
        
        if (!_shouldInterpolate) _displayerPercentageFilled = PercentageFilled;
        else StartCoroutine(_lerpFilled(_displayerPercentageFilled, PercentageFilled, _interpolationTime));

        _displayFillBar();
    }

    private IEnumerator _lerpFilled(float startPercentageFilled, float endPercentageFilled, float duration)
    {
        float elapsedTime = 0f;
        _displayerPercentageFilled = startPercentageFilled;

        while (elapsedTime < duration)
        {
            _displayerPercentageFilled = Mathf.Lerp(startPercentageFilled, endPercentageFilled, elapsedTime / duration);
            elapsedTime += Time.deltaTime;

            _displayFillBar();

            yield return null;
        }
    
        _displayerPercentageFilled = endPercentageFilled;
    }


    private void _displayFillBar()
    {
        transform.localScale = new Vector3(_minXScale + (_maxXScale - _minXScale) * _displayerPercentageFilled, 1f, 1f);
    }
}