using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] private MineStatsSO _stats;
    [SerializeField] Vector3UnityEvent _onProjectileHit = new ();

    public void Init(Vector3 spawnPosition)
    {
        transform.position = spawnPosition;
    }

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
}