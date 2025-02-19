using UnityEngine;

class BaseDamagable : MonoBehaviour
{
    [SerializeField] private GameObject _parent;

    [field: SerializeField] public float Health { get; private set; }

    public virtual void TakeDamage(float damage) 
    { 
        Health -= damage;

        if (Health <= 0) Die();
    }

    public virtual void Die() 
    { 
        Destroy(_parent.gameObject); 
    }
}