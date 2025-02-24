using System;
using UnityEngine;

class RangedAttack : BaseAttack
{
    [SerializeField] private CharacterMovement _attackerMovement;
    [SerializeField] private float _movementVelocityToCancelAttackThreshold = 1f;

    [SerializeField] private LayerMask _enemyLayerMask;
    [SerializeField] private BaseProjectile _projectilePrefab; 
    [SerializeField] private float _attackDistance;

    private Vector3 _attackDirection;
    private Vector3 _attackerPosition;

    private Vector3 _getSpawnPosition() => _attackerPosition;

    protected override void _handleIsReadyForAttack(Action performAttackOrAim) 
    {
        if (!_isTargetFound) return;

        if (Vector3.Distance(_attacker.transform.position, _target.transform.position) > _attackDistance) return;

        if (_attackerMovement.Velocity.magnitude > _movementVelocityToCancelAttackThreshold) return;

        performAttackOrAim();
    } 

    protected override void _handleAttack(Action finishAttack, Action cancelAttack) 
    {
        _throwProjectileInDirection();
        finishAttack();
    }

    protected override void _handleAim(Action cancelAim) 
    {
        if (_attackerMovement.Velocity.magnitude > _movementVelocityToCancelAttackThreshold) cancelAim();

        _updateAttackState();
    }

    private void _updateAttackState()
    {
        if (!_isTargetFound || _attacker == null) return;

        _attackDirection = (_target.transform.position - _attacker.transform.position).normalized;
        _attackerPosition = _attacker.transform.position;
    }

    private void _throwProjectileInDirection()
    {
        BaseProjectile projectile = Instantiate(_projectilePrefab, _getSpawnPosition(), Quaternion.identity);
        projectile.Init(_attackDirection, _enemyLayerMask);
    }
}