using System;
using UnityEngine;

class AttackOnCollision : BaseAttack
{
    [SerializeField] private LayerMask _enemyLayerMask;
    [SerializeField] private float _attackDamage = 50f;

    [SerializeField] private float _attackDistance;
    [SerializeField] private float _collisionRadius;

    private Vector3 _attackDirection;
    private Vector3 _attackerPosition;

    private Vector3 _getSpawnPosition() => _attackerPosition;

    public override void Setup(GameObject attacker, Target target)
    {
        base.Setup(attacker, target);
    }

    protected override void _handleIsReadyForAttack(Action performAttackOrAim) 
    {
        if (!_isTargetFound) return;

        if (Vector3.Distance(_attacker.transform.position, _target.transform.position) > _attackDistance) return;

        performAttackOrAim();
    } 

    protected override void _handleAttack(Action finishAttack, Action cancelAttack, Action cancelReloadAttack)
    {
        _updateAttackState();
        _handleCollision();

        finishAttack();
    }

    private void _updateAttackState()
    {
        if (!_isTargetFound) return;

        _attackDirection = (_target.transform.position - _attacker.transform.position).normalized;
        _attackerPosition = _attacker.transform.position;
    }

    private void _handleCollision()
    {    
        var colliders = Physics2D.OverlapBox(_getSpawnPosition(), new Vector2(_collisionRadius, _collisionRadius), 0, _enemyLayerMask);

        if (colliders != null)
        {
            Transform parent = colliders.gameObject.transform.parent;

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