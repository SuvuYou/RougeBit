using System;
using System.Collections;
using UnityEngine;

class RangedAttack : BaseAttack
{
    [SerializeField] private LayerMask _enemyLayerMask;
    [SerializeField] private BaseProjectile _projectilePrefab; 
    [SerializeField] private float _attackDistance;

    private Vector3 _attackDirection;
    private Vector3 _attackerPosition;

    private Vector3 _getSpawnPosition() => _attackerPosition;

    public override void Setup(GameObject attacker, Func<(GameObject, bool)> getTarget)
    {
        base.Setup(attacker, getTarget);
    }

    protected override bool _shouldAttack() 
    {
        var (target, isTargetFound) = _getTarget();

        if (!isTargetFound) return false;

        return Vector3.Distance(_attacker.transform.position, target.transform.position) <= _attackDistance;
    } 

    protected override void _handleAttack()
    {
        DeactivateAttack();

        _takeAim(
            onAim: () => 
            {
                _updateAttackState();
            }, 
            onFinishAiming: () =>
            {   
                _throwProjectileInDirection();
                ActivateAttack();
            });
    }

    private void _updateAttackState()
    {
        var (target, isTargetFound) = _getTarget();

        if (!isTargetFound || _attacker == null) return;

        _attackDirection = (target.transform.position - _attacker.transform.position).normalized;
        _attackerPosition = _attacker.transform.position;
    }

    private void _takeAim(Action onAim, Action onFinishAiming)
    {
        StartCoroutine(_takeAimForDuration(1f, onAim, onFinishAiming));
    }

    private IEnumerator _takeAimForDuration(float aimDuration, Action onAim, Action onFinishAiming)
    {
        float elapsedTime = 0f;

        while (elapsedTime < aimDuration)
        {
            onAim?.Invoke();
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        onFinishAiming?.Invoke();
    }

    private void _throwProjectileInDirection()
    {
        BaseProjectile projectile = Instantiate(_projectilePrefab, _getSpawnPosition(), Quaternion.identity);
        projectile.Init(_attackDirection, _enemyLayerMask);
    }
}