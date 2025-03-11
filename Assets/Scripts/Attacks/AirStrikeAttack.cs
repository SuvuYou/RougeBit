using System;
using UnityEngine;

class AirStrikeAttack : BaseAttack
{
    public override void UpgradeValues(BaseUpgradeValuesSetSO ovrrideValues)
    {
        base.UpgradeValues(ovrrideValues);

        _stats = ovrrideValues.AirStrikeAttackStats;
    }

    [SerializeField] private CharacterMovement _attackerMovement;

    [SerializeField] private AirStrikeAttackStatsSO _stats;

    private Vector3 _cachedTargetPosition;

    protected override Vector3 _targetPosition => _cachedTargetPosition == Vector3.zero ? _target.transform.position : _cachedTargetPosition;

    public override void Setup(GameObject attacker)
    {
        base.Setup(attacker);
        
        OnAim.AddListener((Vector3 _) => _cachedTargetPosition = _target.transform.position);
    }

    protected override void _handleIsReadyForAttack(Action performAttackOrAim) 
    {
        if (!_isTargetFound) return;

        if (Vector3.Distance(_attacker.transform.position, _target.transform.position) > _stats.AttackDistance) return;

        performAttackOrAim();
    }

    protected override void _handleAttack(Action finishAttack, Action cancelAttack, Action cancelReloadAttack)
    {
        _handleCollision();

        finishAttack();
    }

    private void _handleCollision() 
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_cachedTargetPosition, _stats.ExplosionRadious, _enemyLayerMask);

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
}