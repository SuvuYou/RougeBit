using UnityEngine;

public class CollisionWeapon : BaseWeapon
{
    [SerializeField] AttackOnCollision _attack;
    [SerializeField] RotationController _rotationController;
    
    public override void Activate () { _attack.ActivateAttack(); }
    public override void Deactivate () { _attack.DeactivateAttack(); }

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
