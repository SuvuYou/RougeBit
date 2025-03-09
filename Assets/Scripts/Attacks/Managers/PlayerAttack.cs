using System.Collections;
using UnityEngine;

class PlayerAttack : MonoBehaviour
{
    [SerializeField] private BaseWeapon[] _weapons;
    [SerializeField] private PalyerInputSO _palyerInputSO;
    [SerializeField] private CharacterMovement _movement;

    [SerializeField] private AttackAnimationController _attackAnimationController;

    private void Start()
    {
        _setupAttacks();
    }

    private void Update()
    {
        _setAttacksTarget();
    }

    private void _setupAttacks()
    {
        foreach (var attack in _weapons) 
        {
            attack.Setup(this.gameObject);
        }

        _setAttacksTarget();
    }

    private void _setAttacksTarget()
    {
        (var enemy, var isEnemyFound) = _getEnemyTarget();
        (var projectile, var isProjectileFound) = _getProjectileTarget();
        
        foreach (var weapon in _weapons)
        {
            if (weapon.WeaponTargetType == BaseWeapon.TargetType.Projectile) weapon.SetTarget(projectile, isProjectileFound);
            else weapon.SetTarget(enemy, isEnemyFound);
        }
    }

    private (Target, bool) _getEnemyTarget()
    {
        bool isTargetFound = WaveManager.Instance.TryGetClosestEnemy(transform.position, out Enemy enemy);

        return (enemy, isTargetFound);
    }

    private (Target, bool) _getProjectileTarget()
    {
        bool isTargetFound = ProjectileManager.Instance.TryGetClosestProjectile(transform.position, out BaseProjectile closestProjectile);

        return (closestProjectile?.TargetComponent, isTargetFound);
    }

    public void DisableMovementOnAttack() => StartCoroutine(_disableMovementForSeconds(0.1f));
    public void TriggerAttackAnimation() => _triggerAttackAnimation();

    private IEnumerator _disableMovementForSeconds(float seconds)
    {
        _movement.AddMovementLock(MovementLock.AttackStunned);
        yield return new WaitForSeconds(seconds);
        _movement.RemoveMovementLock(MovementLock.AttackStunned);
    }

    private void _triggerAttackAnimation()
    {
        var (target, isTargetFound) = _getEnemyTarget();

        if (!isTargetFound) return;

        _attackAnimationController.TriggerAttackAnimation((target.transform.position - transform.position).normalized);
    }

}