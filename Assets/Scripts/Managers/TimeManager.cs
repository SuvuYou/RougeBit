using System.Collections;
using UnityEngine;

public class TimeManager : Singlton<TimeManager>, IResettable
{
    public void Reset() => Time.timeScale = 1f;

    [SerializeField] private bool _override;
    [SerializeField] private float _timeScale = 0.1f;

    private void Update()
    {
        if(_override) Time.timeScale =_timeScale;
    }

    public void StopTime() => Time.timeScale = 0f;
    public void StartTime() => Time.timeScale = 1f;

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

        Time.timeScale = 1f;
    }
}
