using System;
using UnityEngine;

class DashAttackOnCollision : BaseAttack
{
    [SerializeField] private LayerMask _enemyLayerMask;
    [SerializeField] private float _attackDamage = 50f;
    [SerializeField] private float _dashTriggerDistance = 5f;
    [SerializeField] private float _dashDistance = 2f;

    private BoxCollider2D _collider;
    private CharacterMovement _attackerMovement;
    private float _attackReachDistance;

    private Vector3 _attackDirection;
    private Vector3 _attackerPosition;

    private Vector3 _positionBeforeDash;
    private Vector3 _dashTargetPosition;

    public override void Setup(GameObject attacker, GameObject target)
    {
        base.Setup(attacker, target);

        if(attacker.transform.parent.TryGetComponentInChildren(out BoxCollider2D collider))
        {
            _attackReachDistance = collider.bounds.size.x;
            _collider = collider;
        }

        if(attacker.transform.parent.TryGetComponentInChildren(out CharacterMovement movement))
        {
            _attackerMovement = movement;
        }

        OnAim += () => _attackerMovement.Dash(_positionBeforeDash, _dashTargetPosition, _aimTimer.Duration);
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
        var colliders = Physics2D.OverlapBox(_attackerPosition, new Vector2(_collider.bounds.size.x, _collider.bounds.size.y), 0, _enemyLayerMask);

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