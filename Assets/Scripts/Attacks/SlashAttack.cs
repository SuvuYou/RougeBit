using System;
using UnityEngine;

class SlashAttack : BaseAttack
{
    [SerializeField] private AttackAnimation _attackAnimation;
    [SerializeField] private SlashAttackStatsSO _stats;

    private Vector3 _attackDirection;
    private Vector3 _attackerPosition;

    private Vector3 _getSpawnPosition() => _attackerPosition + _attackDirection * 1.5f;

    protected override void _handleIsReadyForAttack(Action performAttackOrAim) 
    {
        if (!_isTargetFound) return;

        if (Vector3.Distance(_attacker.transform.position, _target.transform.position) > _stats.AttackDistance) return;

        performAttackOrAim();
    } 

    protected override void _handleAttack(Action finishAttack, Action cancelAttack, Action cancelReloadAttack)
    {
        _updateAttackState();
        _spawnAttackAnimation();
        _handleCollision(); 

        finishAttack();
    }

    private void _updateAttackState()
    {
        if (!_isTargetFound) return;

        _attackDirection = (_target.transform.position - _attacker.transform.position).normalized;
        _attackerPosition = _attacker.transform.position;
    }

    private void _spawnAttackAnimation() 
    {
        var attackAnimation = Instantiate(_attackAnimation, _getSpawnPosition(), Quaternion.identity);
        attackAnimation.AttackIntoDirection(_attackDirection);
    }

    private void _handleCollision()
    {    
        var angle = _attackAnimation.transform.rotation.z;
        var colliders = Physics2D.OverlapBoxAll(_getSpawnPosition(), new Vector2(_stats.AttackDistance + 0.5f, _stats.AttackDistance + 0.5f), angle, _baseStats.EnemyLayerMask);

        foreach (var collider in colliders)
        {
            if (collider == null) continue;

            Transform parent = collider.gameObject.transform.parent;

            if (parent.TryGetComponentInChildren(out Target target))
            {
                if (target == null || !target.IsTargetable) return;
            }

            if (parent.TryGetComponentInChildren(out BaseDamagable damagable))
            {
                damagable.TakeDamage(_stats.AttackDamage);
            }

            if (_baseStats.AddsKnockback && parent.TryGetComponentInChildren(out BaseKnockback knockable))
            {
                knockable.AddKnockback(_attackDirection);
            }
        }
        
    }
}