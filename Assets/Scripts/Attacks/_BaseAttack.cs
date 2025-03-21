using System;
using UnityEngine;
using UnityEngine.Events;

public class BaseAttack : Upgradable
{
    public override void UpgradeValues(BaseUpgradeValuesSetSO ovrrideValues)
    {
        _baseStats = ovrrideValues.BaseAttackStats;

        _reloadTimer = new Timer(_baseStats.ReloadDuration);
        _aimTimer = new Timer(_baseStats.AimDuration);

        ActivateAttack();
    }

    public enum AttackState { Reload, ReadyForAttack, Aiming, Attacking }

    [SerializeField] protected _BaseAttackStatsSO _baseStats;

    [Header("Target")]
    protected LayerMask _enemyLayerMask;

    [Header("Attack Events")]
    public UnityEvent OnReload;
    public UnityEvent OnReadyForAttack;
    public Vector3UnityEvent OnAim;
    public Vector3UnityEvent OnAttack;
    public UnityEvent OnFinishAttack;

    protected GameObject _attacker;
    protected Target _target;

    protected virtual Vector3 _targetPosition => _isTargetFound ? _target.transform.position : Vector3.zero;

    protected bool _isTargetFound => _target != null;

    protected Timer _reloadTimer;
    protected Timer _aimTimer;

    private bool _isActivated = true;

    protected AttackState _attackState = AttackState.Reload;

    public bool IsAttacking => _attackState == AttackState.Attacking;

    public void ActivateAttack() { _reloadTimer?.Start(); _reloadTimer?.Reset(); _isActivated = true; }
    public void DeactivateAttack() { _reloadTimer?.Stop(); _aimTimer?.Stop(); _isActivated = false; }

    public void SetTarget(Target target) => _target = target;

    public virtual void Setup(GameObject attacker, LayerMask enemyLayerMask)
    {
        _reloadTimer = new Timer(_baseStats.ReloadDuration);
        _aimTimer = new Timer(_baseStats.AimDuration);

        _attacker = attacker;
        _enemyLayerMask = enemyLayerMask;

        ActivateAttack();
    }

    private void Update()
    {
        if (!_isActivated) return;

        switch (_attackState)
        {
            case AttackState.Reload:
                _reloadTimer.Update(Time.deltaTime);

                if (_reloadTimer.IsRunning && _reloadTimer.IsFinished) 
                {
                    _switchAttackState(AttackState.ReadyForAttack);
                }

                break;
            case AttackState.ReadyForAttack:
                _handleIsReadyForAttack(performAttackOrAim: _performAttackOrAim);
                break;
            case AttackState.Aiming:
                _handleAim(cancelAim: _cancelAttack, earlyFinishAttack: _finishAttack);
            
                _aimTimer.Update(Time.deltaTime);
                
                if (_aimTimer.IsRunning && _aimTimer.IsFinished) 
                {
                    _switchAttackState(AttackState.Attacking);
                }
                break;
            case AttackState.Attacking:
                _handleAttack(finishAttack: _finishAttack, cancelAttack: _cancelAttack, cancelReloadAttack: _cancelReloadAttack);
                break;
        }
    }

    protected void _switchAttackState(AttackState newState) 
    {
        _aimTimer.Reset();
        _reloadTimer.Reset();

        if (newState == AttackState.Reload) OnReload?.Invoke();
        if (newState == AttackState.ReadyForAttack) OnReadyForAttack?.Invoke();
        if (newState == AttackState.Aiming) OnAim?.Invoke(_targetPosition);
        if (newState == AttackState.Attacking) OnAttack?.Invoke(_targetPosition);

        if (newState == AttackState.Aiming) _aimTimer?.Start();
        
        _attackState = newState;
    } 

    protected virtual void _handleIsReadyForAttack(Action performAttackOrAim) => performAttackOrAim();

    protected virtual void _handleAttack(Action finishAttack, Action cancelAttack, Action cancelReloadAttack) {}

    protected virtual void _handleAim(Action cancelAim, Action earlyFinishAttack) {}

    private void _cancelAttack() => _switchAttackState(AttackState.ReadyForAttack);
    private void _cancelReloadAttack() => _switchAttackState(AttackState.Reload);

    private void _finishAttack() 
    {
        OnFinishAttack?.Invoke();
        _switchAttackState(AttackState.Reload);
    }

    private void _performAttackOrAim() => _switchAttackState(_baseStats.RequiresAim ? AttackState.Aiming : AttackState.Attacking);
}