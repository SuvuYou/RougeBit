using System;
using UnityEngine;

class AttackOnCollision : BaseAttack
{
    [SerializeField] private LayerMask _enemyLayerMask;
    [SerializeField] private float _attackDamage = 50f;

    private BoxCollider2D _collider;

    private float _attackDistance;

    private Vector3 _attackDirection;
    private Vector3 _attackerPosition;

    private Vector3 _getSpawnPosition() => _attackerPosition;

    public override void Setup(GameObject attacker, Func<GameObject> getTarget)
    {
        base.Setup(attacker, getTarget);

        if(attacker.transform.parent.TryGetComponentInChildren(out BoxCollider2D collider))
        {
            _attackDistance = collider.bounds.size.x;
            _collider = collider;
        }
    }

    protected override bool _shouldAttack() 
    {
        return Vector3.Distance(_attacker.transform.position, _getTarget().transform.position) <= _attackDistance;
    } 

    protected override void _handleAttack()
    {
        _updateAttackState();
        _handleAttackInDirection();
    }

    private void _updateAttackState()
    {
        _attackDirection = (_getTarget().transform.position - _attacker.transform.position).normalized;
        _attackerPosition = _attacker.transform.position;
    }

    private void _handleAttackInDirection()
    {
        _handleCollision(); 
    }

    private void _handleCollision()
    {    
        var colliders = Physics2D.OverlapBox(_getSpawnPosition(), new Vector2(_collider.bounds.size.x, _collider.bounds.size.y), 0, _enemyLayerMask);

        if (colliders != null)
        {
            Transform parent = colliders.gameObject.transform.parent;
            Debug.Log(colliders.gameObject.name);

            if (parent.TryGetComponentInChildren(out BaseDamagable damagable))
            {
                damagable.TakeDamage(_attackDamage);
            }

            if (_addsKnockback && parent.TryGetComponentInChildren(out BaseKnockback knockable))
            {
                knockable.AddKnockback(_attackDirection);
            }
        }
    }
}