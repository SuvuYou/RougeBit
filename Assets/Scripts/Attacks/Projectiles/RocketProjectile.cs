using UnityEngine;

public class RocketProjectiles : MonoBehaviour
{
    [field: SerializeField] public Target TargetComponent { get; private set; }

    [SerializeField] private RocketProjectileStatsSO _stats;
    [SerializeField] Vector3UnityEvent _onProjectileHit = new ();

    public Target CurrentTarget { get; private set; }

    private float _randomDeviationTimeMultiplier = 1f;

    public void Init(Target target, Vector3 initialDirection)
    {
        CurrentTarget = target;
        
        transform.rotation = _getRotationByDirection(initialDirection);
    }

    protected virtual void Start() 
    {
        _randomDeviationTimeMultiplier = Random.Range(0.8f, 1.2f);

        Destroy(gameObject, _stats.LifeTime);
    }

    protected virtual void Update() 
    {
        if (CurrentTarget == null) 
        {
            _searchForClosestTarget();

            return;
        }

        _rotateTowardsTarget();
        transform.Translate(Vector2.right * (_stats.Speed * Time.deltaTime));   
    }

    #region Rotation
    private void _rotateTowardsTarget()
    {
        var heading = _addDeviationToTarget(CurrentTarget.transform.position) - transform.position;
        
        var rotation = _getRotationByDirection(heading);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, _stats.RotationSpeed * Time.deltaTime);
    }

    private Quaternion _getRotationByDirection(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        return Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private Vector3 _addDeviationToTarget(Vector3 target)
    {
        var deviation = new Vector2(Mathf.Cos(Time.time * _stats.DeviationCycleSpeed * _randomDeviationTimeMultiplier) * _stats.DeviationStrength, Mathf.Sin(Time.time * _stats.DeviationCycleSpeed * _randomDeviationTimeMultiplier) * _stats.DeviationStrength);

        return target + transform.TransformDirection(deviation);
    } 
    #endregion

    #region Collisions
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        // Wrong layer
        if (((1 << collision.gameObject.layer) & _stats.EnemyLayerMask) == 0) return;

        if (collision.transform.parent.TryGetComponentInChildren(out Target target))
        {
            if (target == null || !target.IsTargetable) return;
        }
        
        if (collision.transform.parent.TryGetComponentInChildren(out BaseDamagable _)) 
        {
            _handleExplosionCollisionAtPosition(transform.position);
            _onProjectileHit?.Invoke(transform.position);

            Destroy(gameObject);
        }
    }

    private void _handleExplosionCollisionAtPosition(Vector3 position) 
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, _stats.ExplosionRadious, _stats.EnemyLayerMask);

        foreach (var collider in colliders)
        {
            if (collider == null) continue;

            Transform parent = collider.gameObject.transform.parent;

            if (parent.TryGetComponentInChildren(out BaseDamagable damagable))
            {
                damagable.TakeDamage(_stats.Damage);
            }
        }
    }
    #endregion
    
    #region Search
    private void _searchForClosestTarget() 
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _stats.SearchRadious, _stats.EnemyLayerMask);

        Target newTarget = null;

        foreach (var collider in colliders)
        {
            if (collider == null) continue;

            Transform parent = collider.gameObject.transform.parent;

            if (parent.TryGetComponentInChildren(out Target target))
            {
                if (target.IsTargetable) newTarget = target;
            }
        }

        if (newTarget != null) 
        {
            CurrentTarget = newTarget;
        }
        else
        {
            _handleExplosionCollisionAtPosition(transform.position);
            _onProjectileHit?.Invoke(transform.position);

            Destroy(gameObject);
        }
    }
    #endregion
}