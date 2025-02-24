using System;
using UnityEngine;

public class BaseAttack : MonoBehaviour
{
    public enum AttackState { Reload, ReadyForAttack, Aiming, Attacking }

    [SerializeField] private float _reloadDuration;
    [SerializeField] protected bool _addsKnockback; 
    [SerializeField] protected bool _requiresAim; 
    [SerializeField] protected float _aimDuration; 

    public Action OnReload;
    public Action OnReadyForAttack;
    public Action OnAim;
    public Action OnAttack;

    protected GameObject _attacker;
    protected GameObject _target;

    protected bool _isTargetFound => _target != null;

    private Timer _reloadTimer;
    private Timer _aimTimer;

    protected AttackState _attackState = AttackState.Reload;

    public void ActivateAttack() { _reloadTimer.Start(); _reloadTimer.Reset(); }
    public void DeactivateAttack() { _reloadTimer.Stop(); _aimTimer.Stop(); }

    public void SetTarget(GameObject target) => _target = target;

    public virtual void Setup(GameObject attacker, GameObject target)
    {
        _reloadTimer = new Timer(_reloadDuration);
        _aimTimer = new Timer(_aimDuration);

        _attacker = attacker;
        _target = target;

        ActivateAttack();
    }

    private void Update()
    {
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
                _handleAim(cancelAim: _cancelAttack);
            
                _aimTimer.Update(Time.deltaTime);
                
                if (_aimTimer.IsRunning && _aimTimer.IsFinished) 
                {
                    _switchAttackState(AttackState.Attacking);
                }
                break;
            case AttackState.Attacking:
                _handleAttack(finishAttack: _finishAttack, cancelAttack: _cancelAttack);
                break;
        }
    }

    protected void _switchAttackState(AttackState newState) 
    {
        _aimTimer.Reset();
        _reloadTimer.Reset();

        if (newState == AttackState.Reload) OnReload?.Invoke();
        if (newState == AttackState.ReadyForAttack) OnReadyForAttack?.Invoke();
        if (newState == AttackState.Aiming) OnAim?.Invoke();
        if (newState == AttackState.Attacking) OnAttack?.Invoke();

        if (newState == AttackState.Aiming) _aimTimer?.Start();
        
        _attackState = newState;
    } 

    protected virtual void _handleIsReadyForAttack(Action performAttackOrAim) => performAttackOrAim();

    protected virtual void _handleAttack(Action finishAttack, Action cancelAttack) {}

    protected virtual void _handleAim(Action cancelAim) {}

    private void _cancelAttack() => _switchAttackState(AttackState.ReadyForAttack);

    private void _finishAttack() => _switchAttackState(AttackState.Reload);

    private void _performAttackOrAim() => _switchAttackState(_requiresAim ? AttackState.Aiming : AttackState.Attacking);

    private void OnDisable() => DeactivateAttack();
}