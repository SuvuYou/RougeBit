using System;
using UnityEngine;

class BaseAttack : MonoBehaviour
{
    [SerializeField] private float _cooldown;
    [SerializeField] protected bool _addsKnockback; 

    public Action OnAttack;

    protected GameObject _attacker;
    protected Func<(GameObject, bool)> _getTarget;

    private Timer _attackTimer;

    public virtual void Setup(GameObject attacker, Func<(GameObject, bool)> getTarget)
    {
        _attackTimer = new Timer(_cooldown);

        _attacker = attacker;
        _getTarget = getTarget;

        ActivateAttack();
    }

    private void OnDisable() => DeactivateAttack();

    public void ActivateAttack() { _attackTimer.Start(); _attackTimer.Reset(); }
    public void DeactivateAttack() => _attackTimer.Stop();

    private void Update()
    {
        _attackTimer.Update(Time.deltaTime);

        if (_attackTimer.IsRunning && _attackTimer.IsFinished)
        {
            if (!_shouldAttack()) return;

            _handleAttack();

            OnAttack?.Invoke();

            _attackTimer.Reset();
        }
    }

    protected virtual void _handleAttack() {}

    protected virtual bool _shouldAttack() => true;
}