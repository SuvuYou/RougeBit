using UnityEngine;
using UnityEngine.Events;

class BaseDamagable : MonoBehaviour
{
    [SerializeField] private GameObject _parent;

    [SerializeField] protected UnityEvent OnTakeDamage;
    [SerializeField] protected UnityEvent OnDie;

    [field: SerializeField] public float Health { get; private set; }

    public virtual void TakeDamage(float damage) 
    { 
        Health -= damage;

        OnTakeDamage?.Invoke();

        if (Health <= 0) Die();
    }

    public virtual void Die() 
    { 
        OnDie?.Invoke();

        Destroy(_parent.gameObject); 
    }
}