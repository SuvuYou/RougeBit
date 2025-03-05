using System;
using UnityEngine;

class DashAttackOnCollision : BaseAttack
{
    [Header("References")]
    [SerializeField] private LayerMask _enemyLayerMask;
    [SerializeField] private CharacterMovement _attackerMovement;
    [Space(15)]

    [Header("Dash Settings")]
    [SerializeField] private float _attackDamage = 50f;
    [SerializeField] private float _dashTriggerDistance = 5f;
    [SerializeField] private float _dashDistance = 2f;
    [Space(15)]

    [Header("Collision Settings")]
    [SerializeField] private float _attackReachDistance = 2f;
    [SerializeField] private float _collisionRadius = 2f;

    private Vector3 _attackDirection;
    private Vector3 _attackerPosition;

    private Vector3 _positionBeforeDash;
    private Vector3 _dashTargetPosition;

    public override void Setup(GameObject attacker, Target target)
    {
        base.Setup(attacker, target);

        OnAim.AddListener(() => _attackerMovement.Dash(_positionBeforeDash, _dashTargetPosition, _aimTimer.Duration));
    }

    protected override void _handleIsReadyForAttack(Action performAttackOrAim) 
    {
        if (!_isTargetFound) return;

        if (Vector3.Distance(_attacker.transform.position, _target.transform.position) > _dashTriggerDistance) return;

        _updateAttackState();
        performAttackOrAim();
    } 

    protected override void _handleAttack(Action finishAttack, Action cancelAttack, Action cancelReloadAttack)
    {
        _updateAttackState();

        if (Vector3.Distance(_attacker.transform.position, _target.transform.position) > _attackReachDistance) 
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
        _dashTargetPosition = _positionBeforeDash + _attackDirection.normalized * _dashDistance;
    }

    private void _handleCollision()
    {    
        var colliders = Physics2D.OverlapBox(_attackerPosition, new Vector2(_collisionRadius, _collisionRadius), 0, _enemyLayerMask);

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