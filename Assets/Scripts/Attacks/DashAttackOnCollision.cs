using System;
using UnityEngine;

class DashAttackOnCollision : BaseAttack
{
    public override void UpgradeValues(BaseUpgradeValuesSetSO ovrrideValues)
    {
        base.UpgradeValues(ovrrideValues);

        _stats = ovrrideValues.DashAttackOnCollisionStats;
    }

    [Header("References")]
    [SerializeField] private DashAttackOnCollisionStatsSO _stats;

    private CharacterMovement _attackerMovement;

    private Vector3 _attackDirection;
    private Vector3 _attackerPosition;

    private Vector3 _positionBeforeDash;
    private Vector3 _dashTargetPosition;

    public override void Setup(GameObject attacker, LayerMask enemyLayerMask)
    {
        base.Setup(attacker, enemyLayerMask);

        if (attacker.transform.TryGetComponentInChildrenOfParent(out CharacterMovement movement))
        {
            _attackerMovement = movement;
        }
        else    
        {
            Debug.LogError("Attacker does not have a CharacterMovement component!");
        }

        OnAim.AddListener((Vector3 targetPosition) => _attackerMovement.Dash(_positionBeforeDash, _dashTargetPosition, _aimTimer.Duration));
    }

    protected override void _handleIsReadyForAttack(Action performAttackOrAim) 
    {
        if (!_isTargetFound) return;

        if (Vector3.Distance(_attacker.transform.position, _target.transform.position) > _stats.DashTriggerDistance) return;

        _updateAttackState();
        performAttackOrAim();
    } 

    protected override void _handleAim(Action cancelAim, Action earlyFinishAttack)
    {
        _updateAttackState();

        if (_handleCollision()) earlyFinishAttack();
    }

    protected override void _handleAttack(Action finishAttack, Action cancelAttack, Action cancelReloadAttack)
    {
        _updateAttackState();

        if (Vector3.Distance(_attacker.transform.position, _target.transform.position) > _stats.AttackReachDistance) 
        {
            cancelReloadAttack();

            return;
        }

        _handleCollision();

        finishAttack();
    }

    private void _updateAttackState()
    {
        if (!_isTargetFound) return;

        _attackDirection = (_target.transform.position - _attacker.transform.position).normalized;
        _attackerPosition = _attacker.transform.position;

        _positionBeforeDash = _attackerPosition;
        _dashTargetPosition = _positionBeforeDash + _attackDirection.normalized * _stats.DashDistance;
    }

    private bool _handleCollision()
    {    
        var colliders = Physics2D.OverlapBox(_attackerPosition, new Vector2(_stats.CollisionRadius, _stats.CollisionRadius), 0, _enemyLayerMask);

        if (colliders == null) return false;

        Transform parent = colliders.gameObject.transform.parent;

        if (parent.TryGetComponentInChildren(out BaseDamagable damagable))
        {
            damagable.TakeDamage(_stats.AttackDamage);
        }

        if (_baseStats.AddsKnockback && parent.TryGetComponentInChildren(out BaseKnockback knockable))
        {
            knockable.AddKnockback(_attackDirection);
        }

        return true;   
    }
}