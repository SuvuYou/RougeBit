using System;
using System.Linq;
using UnityEngine;

class SlashAttack : BaseAttack
{
    [SerializeField] private AttackAnimation _attackAnimation;
    [SerializeField] private SlashAttackStatsSO _stats;

    private Vector3 _attackDirection;
    private Vector3 _attackerPosition;

    private Vector3 _getSpawnPosition() => _attackerPosition + _attackDirection.normalized * 2f;

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
        var colliders = Physics2D.OverlapBoxAll(_getSpawnPosition(), new Vector2(_stats.AttackDistance + 1f, _stats.AttackDistance + 1f), angle, _baseStats.EnemyLayerMask);

        foreach (var collider in colliders)
        {
            if (collider == null) continue;

            Transform parent = collider.gameObject.transform.parent;

            if (parent.TryGetComponentInChildren(out Target target))
            {
                if (target == null || !target.IsTargetable) continue;
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

    // Draws the attack collider in the Unity Editor for visualization
    private void OnDrawGizmosSelected()
    {
        if (!_isTargetFound) return;

        Gizmos.color = new Color(1, 0, 0, 0.5f); // Semi-transparent red
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(_getSpawnPosition(), Quaternion.Euler(0, 0, _attackAnimation.transform.rotation.eulerAngles.z), Vector3.one);
        Gizmos.matrix = rotationMatrix;

        Vector2 size = new Vector2(_stats.AttackDistance + 0.5f, _stats.AttackDistance + 0.5f);
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(size.x, size.y, 0));
    }
}