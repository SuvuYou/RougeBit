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

    [SerializeField] [Range(1, 12)] private int _numberOfProjectiles = 1;
    [SerializeField] [Range(0, 360)] private float _spreadAngle = 45f;

    protected override void _handleIsReadyForAttack(Action performAttackOrAim) 
    {
        if (!_isTargetFound) return;

        if (Vector3.Distance(_attacker.transform.position, _target.transform.position) > _attackDistance) return;

        if (_attackerMovement.Velocity.magnitude > _movementVelocityToCancelAttackThreshold) return;

        performAttackOrAim();
    } 

    protected override void _handleAttack(Action finishAttack, Action cancelAttack, Action cancelReloadAttack)
    {
        _spawnProjectiles();
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

    private void _spawnProjectiles()
    {
        if (_numberOfProjectiles == 1)
        {
            _throwProjectileInDirection(_attackDirection);
        }
        else
        {
            float startAngle = -_spreadAngle / 2f;
            float angleIncrement = _spreadAngle / (_numberOfProjectiles - 1);
            
            for (int i = 0; i < _numberOfProjectiles; i++)
            {
                float angle = startAngle + i * angleIncrement;

                _throwProjectileInDirection(direction: Quaternion.Euler(0, 0, angle) * _attackDirection);
            }
        }
    }

    private void _throwProjectileInDirection(Vector3 direction)
    {
        BaseProjectile projectile = Instantiate(_projectilePrefab, _attackerPosition, Quaternion.identity);
        projectile.Init(direction, _enemyLayerMask);
    }
}