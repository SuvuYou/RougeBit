using System.Collections;
using UnityEngine;
using UnityEngine.Events;

class DamagableWithImmunity : BaseDamagable
{
    [Space(10)]
    [Header("Immunity Settings")]
    [SerializeField] private float _immunityDuration = 0.5f;

    [Header("Events")]
    public UnityEvent<float> OnImmunityGranted;
    public UnityEvent OnImmunityEnd;

    public bool IsImmune { get; private set; }

    public override void TakeDamage(float damage) 
    { 
        if (IsImmune) return;

        base.TakeDamage(damage);
        GrantImmunity();
    }

    public void GrantImmunity() => StartCoroutine(_setImmunityForSeconds(_immunityDuration));

    private IEnumerator _setImmunityForSeconds(float seconds) 
    { 
        OnImmunityGranted?.Invoke(_immunityDuration);
        IsImmune = true; 
        yield return new WaitForSeconds(seconds); 
        IsImmune = false; 
        OnImmunityEnd?.Invoke();
    }
}