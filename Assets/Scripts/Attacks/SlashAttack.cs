using System;
using UnityEngine;

class SlashAttack : BaseAttack
{
    public override void UpgradeValues(BaseUpgradeValuesSetSO ovrrideValues)
    {
        base.UpgradeValues(ovrrideValues);

        _stats = ovrrideValues.SlashAttackStats;
    }

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
        var angle = Vector3.Angle(Vector3.left, _attackDirection);
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
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(_getSpawnPosition(), Quaternion.Euler(0, 0, Vector3.Angle(Vector3.left, _attackDirection)), Vector3.one);
        Gizmos.matrix = rotationMatrix;

        Vector2 size = new Vector2(_stats.AttackDistance + 0.5f, _stats.AttackDistance + 0.5f);
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(size.x, size.y, 0));
    }
}