using System;
using UnityEngine;

class LaserBeamAttack : BaseAttack
{
    public override void UpgradeValues(BaseUpgradeValuesSetSO ovrrideValues)
    {
        base.UpgradeValues(ovrrideValues);

        _stats = ovrrideValues.LaserBeamAttackStats;

        _attackTimer.SetBaseTime(_stats.BeamDuration);
        _collisionTimer.SetBaseTime(1f / _stats.BeamAttacksPerSecond);
    }

    [SerializeField] private LaserBeamAttackStatsSO _stats;
    
    private CharacterMovement _attackerMovement;

    private Vector3 _attackDirection;
    private Vector3 _attackerPosition;

    private Timer _attackTimer = new();
    private Timer _collisionTimer = new();

    public override void Setup(GameObject attacker)
    {
        base.Setup(attacker);

        if(attacker.transform.parent.TryGetComponentInChildren(out CharacterMovement movement))
        {
            _attackerMovement = movement;
        }

        _attackTimer.SetBaseTime(_stats.BeamDuration);
        _collisionTimer.SetBaseTime(1f / _stats.BeamAttacksPerSecond);

        OnAttack.AddListener((Vector3 targetPosition) => _attackerMovement.AddMovementLock(MovementLock.Attack));
    }

    protected override void _handleIsReadyForAttack(Action performAttackOrAim) 
    {
        if (!_isTargetFound) return;

        if (Vector3.Distance(_attacker.transform.position, _target.transform.position) > _stats.AttackReachDistance) return;

        if (_attackerMovement.Velocity.magnitude > _stats.MovementVelocityToCancelAttackThreshold) return;

        _collisionTimer.Reset();
        _collisionTimer.Start();
        _attackTimer.Reset();
        _attackTimer.Start();

        performAttackOrAim();
    } 

    protected override void _handleAttack(Action finishAttack, Action cancelAttack, Action cancelReloadAttack)
    {
        _attackTimer.Update(Time.deltaTime);
        _collisionTimer.Update(Time.deltaTime);
        
        if (_collisionTimer.IsFinished)
        {
            _handleCollision();
            _collisionTimer.Reset();
        }

        if (_attackTimer.IsFinished)
        {
            _attackTimer.Stop();
            _collisionTimer.Stop();

            _attackerMovement.RemoveMovementLock(MovementLock.Attack);

            finishAttack();
        }
    }

    protected override void _handleAim(Action cancelAim, Action earlyFinishAttack)
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

    private void _handleCollision()
    {    
        float angle = Vector3.Angle(Vector3.right, _attackDirection);

        if (_attackDirection.y < 0)
            angle = -angle;

        Vector3 spawnPosition = _attackerPosition + _attackDirection * (_stats.BeamLength / 2);

        var colliders = Physics2D.OverlapBox(spawnPosition, new Vector2(_stats.BeamLength, _stats.BeamWidth), angle, _enemyLayerMask);

        if (colliders != null)
        {
            Transform parent = colliders.gameObject.transform.parent;

            if (parent.TryGetComponentInChildren(out BaseDamagable damagable))
            {
                damagable.TakeDamage(_stats.BeamDamage);
            }

            if (_baseStats.AddsKnockback && parent.TryGetComponentInChildren(out BaseKnockback knockable))
            {
                knockable.AddKnockback(_attackDirection);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (_attacker == null)
            return;

        Gizmos.color = Color.red;

        Vector3 spawnPosition = _attackerPosition + _attackDirection * (_stats.BeamLength / 2);
        Vector3 center = spawnPosition;

        float angle = Vector3.Angle(Vector3.right, _attackDirection);
        if (_attackDirection.y < 0)
            angle = -angle;

        Matrix4x4 oldMatrix = Gizmos.matrix;

        Gizmos.matrix = Matrix4x4.TRS(center, Quaternion.Euler(0, 0, angle), Vector3.one);

        Gizmos.DrawWireCube(Vector3.zero, new Vector3(_stats.BeamLength, _stats.BeamWidth, 0f));

        Gizmos.matrix = oldMatrix;
    }
}