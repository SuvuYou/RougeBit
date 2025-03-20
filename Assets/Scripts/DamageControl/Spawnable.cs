using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Spawnable : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] protected UnityEvent OnBeforeSpawn;
    [SerializeField] protected UnityEvent OnSpawn;

    private float _spawnDuration = 0f;

    public void SetSpawnDuration(float duration) => _spawnDuration = duration;

    public virtual void Spawn() 
    { 
        StartCoroutine(_eventAfterSpawn());
    }

    private IEnumerator _eventAfterSpawn()
    {
        OnBeforeSpawn?.Invoke();
        yield return new WaitForSeconds(_spawnDuration);
        OnSpawn?.Invoke();
    }
}