

using System.Collections;
using UnityEngine;

class FlashAnimation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Color _flashColor;
    [SerializeField] private float _flashDuraionInSeconds = 0.25f;

    private Color _defaultColor;

    private void Awake()
    {
        _defaultColor = _spriteRenderer.color;
    }

    public void Flash()
    {
        StartCoroutine(_flashWithDelay(_flashDuraionInSeconds));
    }

    private IEnumerator _flashWithDelay(float delayInSeconds)
    {
        _spriteRenderer.color = _flashColor;
        yield return new WaitForSeconds(delayInSeconds);
        _spriteRenderer.color = _defaultColor;
    }
}