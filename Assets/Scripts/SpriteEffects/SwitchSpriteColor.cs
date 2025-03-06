using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class SwitchSpriteColor : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private float _lerpDuration = 0.5f;

    [SerializeField] private List<Color> _listOfAvailableColors;

    private Coroutine _currentCoroutine;

    private void Start()
    {
        SwitchColor(0);
    }

    public void SwitchColor(int targetColorIndex) 
    {
        if (_currentCoroutine != null) StopCoroutine(_currentCoroutine);

        if (targetColorIndex > _listOfAvailableColors.Count - 1) targetColorIndex = _listOfAvailableColors.Count - 1;

        _currentCoroutine = StartCoroutine(_lerpColor(spriteRenderer.color, _listOfAvailableColors[targetColorIndex], _lerpDuration));
    }

    private IEnumerator _lerpColor(Color startColor, Color endColor, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime / duration;
            spriteRenderer.color = Color.Lerp(startColor, endColor, elapsedTime);
            yield return null;
        }

        spriteRenderer.color = endColor;
    }
}