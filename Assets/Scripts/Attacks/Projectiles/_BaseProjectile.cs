using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    private enum Varients { Player, Enemy };

    [field: SerializeField] public Target TargetComponent { get; private set; }

    [SerializeField] private _BaseProjectileStatsSO _playerVarientStats;
    [SerializeField] private _BaseProjectileStatsSO _enemyVarientStats;

    [SerializeField] private Varients _currentVarient;
    
    private _BaseProjectileStatsSO _currentStats;

    public Vector3 Direction { get; private set; }

    protected virtual void Start() 
    {
        _applyVariantStats(_currentVarient == Varients.Player ? _playerVarientStats : _enemyVarientStats);

        Destroy(gameObject, _currentStats.LifeTime);
    }

    protected virtual void Update() 
    {
        transform.Translate(Direction * _currentStats.Speed * Time.deltaTime);   
    }

    public void Init(Vector3 flyDirection, LayerMask? layerMask = null)
    {
        Direction = flyDirection;
        _currentStats.EnemyLayerMask = layerMask ?? LayerMask.GetMask("Default");
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        // Wrong layer
        if (((1 << collision.gameObject.layer) & _currentStats.EnemyLayerMask) == 0) return;

        if (collision.transform.parent.TryGetComponentInChildren(out Target target))
        {
            if (target == null || !target.IsTargetable) return;
        }
        
        if (collision.transform.parent.TryGetComponentInChildren(out BaseDamagable damagable)) 
        {
            damagable.TakeDamage(_currentStats.Damage);

            Destroy(gameObject);
        }
    }

    public void Redirect(float speedMultiplier = 1f)
    {
        var newVarient = _currentVarient == Varients.Enemy ? Varients.Player : Varients.Enemy;

        _currentVarient = newVarient;
        _applyVariantStats(newVarient == Varients.Player ? _playerVarientStats : _enemyVarientStats);

        _currentStats.Speed *= speedMultiplier;

        Direction = -Direction;
    }

    private void _applyVariantStats(_BaseProjectileStatsSO newStats) 
    {
        _currentStats = newStats;
    }
}