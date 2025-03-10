using UnityEngine;

public class MinerWeapon : BaseWeapon
{
    [SerializeField] PlantMineAttack _attack;
    [SerializeField] RotationController _rotationController;

    public override void Setup(GameObject attacker) 
    {
        _attack.Setup(attacker);
        _rotationController.EnableIdleRotation();
    }

    public override void SetTarget(Target targetPosition, bool isTargetFound = false)
    {

    }
}
