using System;
using UnityEngine;
using UnityEngine.Events;

class DashAttackOnCollision : BaseAttack
{
    public override void UpgradeValues(BaseUpgradeValuesSetSO ovrrideValues)
    {
        base.UpgradeValues(ovrrideValues);

        _chargeAttackTimer = new (ovrrideValues.DashAttackOnCollisionStats.ChargeAttackDuration);
        _chargeAttackTimer.Stop();

        _stats = ovrrideValues.DashAttackOnCollisionStats;
    }

    [Header("References")]
    [SerializeField] private DashAttackOnCollisionStatsSO _stats;

    public UnityEvent OnDealDamage;

    public UnityEvent OnStartCharge;
    public UnityEvent OnEndCharge;

    private CharacterMovement _attackerMovement;

    private Vector3 _attackDirection;
    private Vector3 _attackerPosition;

    private Vector3 _positionBeforeDash;
    private Vector3 _dashTargetPosition;

    private Timer _chargeAttackTimer;

    public override void Setup(GameObject attacker, LayerMask enemyLayerMask)
    {
        base.Setup(attacker, enemyLayerMask);

        _chargeAttackTimer = new (_stats.ChargeAttackDuration);
        _chargeAttackTimer.Stop();

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
        if (!_isTargetFound) 
        {
            OnEndCharge.Invoke();
            _chargeAttackTimer.Stop();

            return;
        }

        if (Vector3.Distance(_attacker.transform.position, _target.transform.position) > _stats.DashTriggerDistance) 
        {
            OnEndCharge.Invoke();
            _chargeAttackTimer.Stop();

            return;
        }

        if (!_chargeAttackTimer.IsRunning)
        {
            OnStartCharge.Invoke();
            _chargeAttackTimer.Reset();
            _chargeAttackTimer.Start(); 
        }

        _chargeAttackTimer.Update(Time.deltaTime);

        _updateAttackState();

        if (_chargeAttackTimer.IsRunning && _chargeAttackTimer.IsFinished)
        {           
            performAttackOrAim();
            OnEndCharge.Invoke();
            _chargeAttackTimer.Stop();
        }
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

            OnDealDamage?.Invoke();
        }

        if (_baseStats.AddsKnockback && parent.TryGetComponentInChildren(out BaseKnockback knockable))
        {
            knockable.AddKnockback(_attackDirection);
        }

        return true;   
    }
}