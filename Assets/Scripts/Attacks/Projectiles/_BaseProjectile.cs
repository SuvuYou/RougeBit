using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    public enum Varients { Player, Enemy };

    [Header("References")]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [field: SerializeField] public Target TargetComponent { get; private set; }

    [Header("Varients")]
    [SerializeField] private _BaseProjectileStatsSO _playerVarientStats;
    [SerializeField] private _BaseProjectileStatsSO _enemyVarientStats;

    [field: SerializeField] public Varients CurrentVarient;
    
    private _BaseProjectileStatsSO _currentStats;

    public Vector3 Direction { get; private set; }

    private float _cachedSpeed;

    protected virtual void Awake() 
    {
        _applyVariantStats(CurrentVarient == Varients.Player ? _playerVarientStats : _enemyVarientStats);

        Destroy(gameObject, _currentStats.LifeTime);
    }

    protected virtual void Update() 
    {
        transform.Translate(Direction * _cachedSpeed * Time.deltaTime);   
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
        var newVarient = CurrentVarient == Varients.Enemy ? Varients.Player : Varients.Enemy;

        CurrentVarient = newVarient;
        _applyVariantStats(newVarient == Varients.Player ? _playerVarientStats : _enemyVarientStats);

        _cachedSpeed *= speedMultiplier;

        Direction = -Direction;
    }

    private void _applyVariantStats(_BaseProjectileStatsSO newStats) 
    {
        _currentStats = newStats;

        _spriteRenderer.sprite = _currentStats.ProjectileSprite;
        _spriteRenderer.color = _currentStats.ProjectileColor;

        _cachedSpeed = _currentStats.Speed;
    }
}