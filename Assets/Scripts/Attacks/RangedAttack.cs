using System;
using UnityEngine;

class RangedAttack : BaseAttack
{
    [SerializeField] private CharacterMovement _attackerMovement;

    [SerializeField] private RangedAttackStatsSO _stats;

    private Vector3 _attackDirection;
    private Vector3 _attackerPosition;

    protected override void _handleIsReadyForAttack(Action performAttackOrAim) 
    {
        if (!_isTargetFound) return;

        if (Vector3.Distance(_attacker.transform.position, _target.transform.position) > _stats.AttackDistance) return;

        if (_attackerMovement.Velocity.magnitude > _stats.MovementVelocityToCancelAttackThreshold) return;

        performAttackOrAim();
    } 

    protected override void _handleAttack(Action finishAttack, Action cancelAttack, Action cancelReloadAttack)
    {
        _spawnProjectiles();
        finishAttack();
    }

    protected override void _handleAim(Action cancelAim) 
    {
        if (_attackerMovement.Velocity.magnitude > _stats.MovementVelocityToCancelAttackThreshold) cancelAim();

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
        if (_stats.NumberOfProjectiles == 1)
        {
            _throwProjectileInDirection(_attackDirection);
        }
        else
        {
            float startAngle = -_stats.SpreadAngle / 2f;
            float angleIncrement = _stats.SpreadAngle / (_stats.NumberOfProjectiles - 1);
            
            for (int i = 0; i < _stats.NumberOfProjectiles; i++)
            {
                float angle = startAngle + i * angleIncrement;

                _throwProjectileInDirection(direction: Quaternion.Euler(0, 0, angle) * _attackDirection);
            }
        }
    }

    private void _throwProjectileInDirection(Vector3 direction)
    {
        BaseProjectile projectile = Instantiate(_stats.ProjectilePrefab, _attackerPosition, Quaternion.identity);
        projectile.Init(direction, _baseStats.EnemyLayerMask);
    }
}