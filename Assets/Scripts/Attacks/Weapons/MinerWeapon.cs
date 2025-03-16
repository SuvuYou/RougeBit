using UnityEngine;

public class MinerWeapon : BaseWeapon
{
    [SerializeField] PlantMineAttack _attack;
    [SerializeField] RotationController _rotationController;

    public override void Setup(GameObject attacker, LayerMask enemyLayerMask) 
    {
        _attack.Setup(attacker, enemyLayerMask);
        _rotationController.EnableIdleRotation();
    }

    public override void SetTarget(Target targetPosition, bool isTargetFound = false) { }
}
