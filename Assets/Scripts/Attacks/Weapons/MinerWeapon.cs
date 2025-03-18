using UnityEngine;

public class MinerWeapon : BaseWeapon
{
    [SerializeField] PlantMineAttack _attack;
    [SerializeField] RotationController _rotationController;
    
    public override void Activate () { _attack.ActivateAttack(); }
    public override void Deactivate () { _attack.DeactivateAttack(); }

    public override void Setup(GameObject attacker, LayerMask enemyLayerMask) 
    {
        _attack.Setup(attacker, enemyLayerMask);
        
        _rotationController.SetupPivotPoint(attacker.transform);
        _rotationController.EnableIdleRotation();
    }

    public override void SetTarget(Target targetPosition, bool isTargetFound = false) { }
}
