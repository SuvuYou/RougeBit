using UnityEngine;

public class WingsLaserBeamWeapon : BaseWeapon
{
    [SerializeField] LaserBeamAttack _attack;
    [SerializeField] RotationController _rotationController;

    public override void Setup(GameObject attacker) => _attack.Setup(attacker);

    public override void SetTarget(Target target, bool isTargetFound)
    {
        if (isTargetFound && !_attack.IsAttacking)
        {
            _attack.SetTarget(target);
            _rotationController.SetNewTarget(target.transform.position);
        }
    }
}
