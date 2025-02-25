using System;
using UnityEngine;

class LaserBeamAttack : BaseAttack
{
    [SerializeField] private float _movementVelocityToCancelAttackThreshold = 1f;

    [SerializeField] private LayerMask _enemyLayerMask;
    [SerializeField] private float _attackReachDistance;

    [SerializeField] private float _beamDamage = 1f;
    [SerializeField] private float _beamAttacksPerSecond = 5f;
    [SerializeField] private float _beamDuration = 3f;
    [SerializeField] private float _beamLength = 8f;
    [SerializeField] private float _beamWidth = 1f;

    [SerializeField] private GameObject _beamSprite;

    private GameObject _beam;

    private CharacterMovement _attackerMovement;

    private Vector3 _attackDirection;
    private Vector3 _attackerPosition;

    private Timer _attackTimer = new();
    private Timer _collisionTimer = new();

    public override void Setup(GameObject attacker, GameObject target)
    {
        base.Setup(attacker, target);

        if(attacker.transform.parent.TryGetComponentInChildren(out CharacterMovement movement))
        {
            _attackerMovement = movement;
        }

        _attackTimer.SetBaseTime(_beamDuration);
        _collisionTimer.SetBaseTime(1f / _beamAttacksPerSecond);

        OnAttack += () => _spawnBeamSprite();
        OnAttack += () => _attackerMovement.DisableMovement();
    }

    protected override void _handleIsReadyForAttack(Action performAttackOrAim) 
    {
        if (!_isTargetFound) return;

        if (Vector3.Distance(_attacker.transform.position, _target.transform.position) > _attackReachDistance) return;

        if (_attackerMovement.Velocity.magnitude > _movementVelocityToCancelAttackThreshold) return;

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

            _attackerMovement.EnableMovement();

            Destroy(_beam.gameObject);

            finishAttack();
        }
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

    private void _handleCollision()
    {    
        float angle = Vector3.Angle(Vector3.right, _attackDirection);

        if (_attackDirection.y < 0)
            angle = -angle;

        Vector3 spawnPosition = _attackerPosition + _attackDirection * (_beamLength / 2);

        var colliders = Physics2D.OverlapBox(spawnPosition, new Vector2(_beamLength, _beamWidth), angle, _enemyLayerMask);

        if (colliders != null)
        {
            Transform parent = colliders.gameObject.transform.parent;

            if (parent.TryGetComponentInChildren(out BaseDamagable damagable))
            {
                damagable.TakeDamage(_beamDamage);
            }

            if (_addsKnockback && parent.TryGetComponentInChildren(out BaseKnockback knockable))
            {
                knockable.AddKnockback(_attackDirection);
            }
        }
    }

    private void _spawnBeamSprite()
    {
        float angle = Vector3.Angle(Vector3.right, _attackDirection);

        if (_attackDirection.y < 0)
            angle = -angle;

        Vector3 spawnPosition = _attackerPosition + _attackDirection * (_beamLength / 2);

        _beam = Instantiate(_beamSprite, spawnPosition, Quaternion.Euler(0, 0, angle));
    }

    private void OnDrawGizmos()
    {
        if (_attacker == null)
            return;

        Gizmos.color = Color.red;

        Vector3 spawnPosition = _attackerPosition + _attackDirection * (_beamLength / 2);
        Vector3 center = spawnPosition;

        float angle = Vector3.Angle(Vector3.right, _attackDirection);
        if (_attackDirection.y < 0)
            angle = -angle;

        Matrix4x4 oldMatrix = Gizmos.matrix;

        Gizmos.matrix = Matrix4x4.TRS(center, Quaternion.Euler(0, 0, angle), Vector3.one);

        Gizmos.DrawWireCube(Vector3.zero, new Vector3(_beamLength, _beamWidth, 0f));

        Gizmos.matrix = oldMatrix;
    }
}