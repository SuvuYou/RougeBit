using UnityEngine;

public class ShotGunWeapon : BaseWeapon
{
    [SerializeField] RangedAttack _attack;
    [SerializeField] RotationController _rotationController;

    public override void Setup(GameObject attacker) => _attack.Setup(attacker);

    public override void SetTarget(Target target, bool isTargetFound = false)
    {
        if (isTargetFound && !_attack.IsAttacking)
        {
            _attack.SetTarget(target);
            _rotationController.SetNewTarget(target.transform.position);
        }
    }
}
