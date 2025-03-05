using System.Collections;
using UnityEngine;

public class TimeManager : Singlton<MonoBehaviour>
{
    public void StopTimeFor(float duration)
    {
        StartCoroutine(_slowDownTimeFor(duration));
    }

    public void SlowDownTimeFor(float duration, float timeScale)
    {
        StartCoroutine(_slowDownTimeFor(duration, timeScale));
    }

    public void HalfTimeScaleFor(float duration)
    {
        StartCoroutine(_slowDownTimeFor(duration, 0.25f));
    }

    private IEnumerator _slowDownTimeFor(float duration, float timeScale = 0)
    {
        Time.timeScale = timeScale;

        float timeElapsed = 0f;

        while(timeElapsed < duration)
        {
            timeElapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        Time.timeScale = 1;
    }
}
