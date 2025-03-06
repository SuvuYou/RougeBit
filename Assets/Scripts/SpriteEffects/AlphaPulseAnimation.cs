

using System.Collections;
using UnityEngine;

class AlphaPulseAnimation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    [Header("Animation Settings")]
    [SerializeField] private float _targetAlphaValue = 0.1f;
    [SerializeField] private int _numCycles = 4;
    [SerializeField] private float _cycleDuration = 0.5f;

    private Color _defaultColor;
    private Color _semiTransparentColor;

    private void Awake()
    {
        _defaultColor = _spriteRenderer.color;

        _semiTransparentColor = _defaultColor;
        _semiTransparentColor.a = _targetAlphaValue;
    }

    public void Pulse()
    {
        StartCoroutine(PulseSequence(_numCycles));
    }

    private IEnumerator PulseSequence(int cycleCount = 1)
    {
        for (int i = 0; i < cycleCount; i++)
        {
            yield return StartCoroutine(_completeAnimationCycle(_defaultColor, _semiTransparentColor));
        }
    }

    private IEnumerator _completeAnimationCycle(Color originalColor, Color semiTransparentColor)
    {
        float halfCycle = _cycleDuration * 0.5f;

        float timeElapsed = 0f;

        while (timeElapsed < halfCycle)
        {
            float t = timeElapsed / halfCycle;
            t = t * t * (3f - 2f * t);

            _spriteRenderer.color = Color.Lerp(originalColor, semiTransparentColor, t);
            
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        timeElapsed = 0f;
        _spriteRenderer.color = semiTransparentColor;

        while (timeElapsed < halfCycle)
        {
            float t = timeElapsed / halfCycle;
            t = t * t * (3f - 2f * t);

            _spriteRenderer.color = Color.Lerp(semiTransparentColor, originalColor, t);
            
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        _spriteRenderer.color = originalColor;
    }
}