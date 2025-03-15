using System.Collections;
using UnityEngine;
using UnityEngine.Events;

class BaseDamagable : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _parent;

    [field: Header("Stats")]
    [field: SerializeField] public float Health { get; private set; }
    [Tooltip("Health value connector (optional)")]
    [SerializeField] private FloatRangeValue HealthValueConnector;
    [SerializeField] private bool IsDestroyable = true;

    [Header("Events")]
    [SerializeField] protected FloatUnityEvent OnTakeDamage;
    [SerializeField] protected UnityEvent OnDie;

    private float _objectDestroyDelay = 0f; 
    private bool _isDead = false;

    public void SetObjectDestroyDelay(float delay) => _objectDestroyDelay = delay;

    private void Start() 
    {
        if (HealthValueConnector != null) HealthValueConnector.SetValue(Health);
    }

    public virtual void TakeDamage(float damage) 
    { 
        Health -= damage;

        OnTakeDamage?.Invoke(damage);

        Debug.Log("TakeDamageTakeDamageTakeDamageTakeDamageTakeDamageTakeDamageTakeDamage");

        if (Health <= 0 && !_isDead) Die();
    }

    public virtual void Die() 
    { 
        _isDead = true;

        OnDie?.Invoke();

        if (IsDestroyable) StartCoroutine(_destroyAfterSeconds(_objectDestroyDelay));
    }

    private IEnumerator _destroyAfterSeconds(float seconds) 
    { 
        yield return new WaitForSeconds(seconds); 
        
        Destroy(_parent.gameObject); 
    }
}