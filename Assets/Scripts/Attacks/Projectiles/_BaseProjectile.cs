using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    private enum Varients { Player, Enemy };

    [field: SerializeField] public Target TargetComponent { get; private set; }

    [SerializeField] private _BaseProjectileStatsSO _playerVarientStats;
    [SerializeField] private _BaseProjectileStatsSO _enemyVarientStats;

    [SerializeField] private Varients _currentVarient;

    private float _damage;
    private float _speed;
    private float _lifeTime;
    private LayerMask _layerMask;

    public Vector3 Direction { get; private set; }

    protected virtual void Start() 
    {
        _applyVariantStats(_currentVarient == Varients.Player ? _playerVarientStats : _enemyVarientStats);

        Destroy(gameObject, _lifeTime);
    }

    protected virtual void Update() 
    {
        transform.Translate(Direction * _speed * Time.deltaTime);   
    }

    public void Init(Vector3 flyDirection, LayerMask? layerMask = null)
    {
        Direction = flyDirection;
        _layerMask = layerMask ?? LayerMask.GetMask("Default");
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        // Wrong layer
        if (((1 << collision.gameObject.layer) & _layerMask) == 0) return;
        
        if (collision.transform.parent.TryGetComponentInChildren(out BaseDamagable damagable)) 
        {
            damagable.TakeDamage(_damage);

            Destroy(gameObject);
        }
    }

    public void Redirect(float speedMultiplier = 1f)
    {
        var newVarient = _currentVarient == Varients.Enemy ? Varients.Player : Varients.Enemy;

        _currentVarient = newVarient;
        _applyVariantStats(newVarient == Varients.Player ? _playerVarientStats : _enemyVarientStats);

        _speed *= speedMultiplier;

        Direction = -Direction;
    }

    private void _applyVariantStats(_BaseProjectileStatsSO newStats) 
    {
        _damage = newStats.Damage;
        _speed = newStats.Speed;
        _lifeTime = newStats.LifeTime;
        _layerMask = newStats.EnemyLayerMask;
    }
}