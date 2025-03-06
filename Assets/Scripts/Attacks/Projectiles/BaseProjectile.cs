using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _speed;
    [SerializeField] private float _lifeTime;

    private LayerMask _layerMask;
    public Vector3 Direction { get; private set; }

    protected virtual void Start() => Destroy(gameObject, _lifeTime);

    protected virtual void Update() 
    {
        transform.Translate(Direction * _speed * Time.deltaTime);   
    }

    public void Init(Vector3 flyDirection, LayerMask? layerMask = null)
    {
        Direction = flyDirection;
        _layerMask = layerMask ?? LayerMask.GetMask("Default");
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision) 
    {
        // Wrong layer
        if (((1 << collision.gameObject.layer) & _layerMask) == 0) return;
        
        if (collision.transform.parent.TryGetComponentInChildren(out BaseDamagable damagable)) 
        {
            damagable.TakeDamage(_damage);

            Destroy(gameObject);
        }
    }
}