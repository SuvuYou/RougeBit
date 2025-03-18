using UnityEngine;

public class ShotGunWeapon : BaseWeapon
{
    [SerializeField] RangedAttack _attack;
    [SerializeField] RotationController _rotationController;

    public override void Activate () { _attack.ActivateAttack(); }
    public override void Deactivate () { _attack.DeactivateAttack(); }

    public override void Setup(GameObject attacker, LayerMask enemyLayerMask) 
    {
        _attack.Setup(attacker, enemyLayerMask);      

        _rotationController.SetupPivotPoint(attacker.transform);
    } 

    public override void SetTarget(Target target, bool isTargetFound = false)
    {
        if (isTargetFound && !_attack.IsAttacking)
        {
            _attack.SetTarget(target);
            _rotationController.SetNewTarget(target.transform.position);
        }
    }
}
