using System.Collections;
using UnityEngine;
using UnityEngine.Events;

class BaseDamagable : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _parent;

    [field: Header("Stats")]
    [field: SerializeField] public float Health { get; private set; }

    [Header("Events")]
    [SerializeField] protected UnityEvent OnTakeDamage;
    [SerializeField] protected UnityEvent OnDie;

    private float _objectDestroyDelay = 0f; 

    public void SetObjectDestroyDelay(float delay) => _objectDestroyDelay = delay;

    public virtual void TakeDamage(float damage) 
    { 
        Health -= damage;

        OnTakeDamage?.Invoke();

        if (Health <= 0) Die();
    }

    public virtual void Die() 
    { 
        OnDie?.Invoke();

        StartCoroutine(_destroyAfterSeconds(_objectDestroyDelay));
    }

    private IEnumerator _destroyAfterSeconds(float seconds) 
    { 
        yield return new WaitForSeconds(seconds); 
        
        Destroy(_parent.gameObject); 
    }
}