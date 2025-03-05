using System.Collections;
using UnityEngine;
using UnityEngine.Events;

class DamagableWithImmunity : BaseDamagable
{
    [SerializeField] private float _immunityDuration = 0.5f;

    public bool IsImmune { get; private set; }

    public UnityEvent OnImmunityGranted;
    public UnityEvent OnImmunityEnd;

    public override void TakeDamage(float damage) 
    { 
        if (IsImmune) return;

        base.TakeDamage(damage);
        GrantImmunity();
    }

    public void GrantImmunity() => StartCoroutine(_setImmunityForSeconds(_immunityDuration));

    private IEnumerator _setImmunityForSeconds(float seconds) 
    { 
        OnImmunityGranted?.Invoke();
        IsImmune = true; 
        yield return new WaitForSeconds(seconds); 
        IsImmune = false; 
        OnImmunityEnd?.Invoke();
    }
}