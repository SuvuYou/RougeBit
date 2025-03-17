using UnityEngine;

public class DashCollisionWeapon : BaseWeapon
{
    [SerializeField] DashAttackOnCollision _attack;
    [SerializeField] RotationController _rotationController;

    public override void Setup(GameObject attacker, LayerMask enemyLayerMask) => _attack.Setup(attacker, enemyLayerMask);

    public override void SetTarget(Target target, bool isTargetFound)
    {
        if (isTargetFound && !_attack.IsAttacking)
        {
            _attack.SetTarget(target);
            _rotationController.SetNewTarget(target.transform.position);
        }
    }
}
