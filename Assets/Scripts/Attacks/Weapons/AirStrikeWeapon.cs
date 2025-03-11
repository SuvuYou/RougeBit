using UnityEngine;

public class AirStrikeWeapon : BaseWeapon
{
    [SerializeField] AirStrikeAttack _attack;

    public override void Setup(GameObject attacker, LayerMask enemyLayerMask) => _attack.Setup(attacker, enemyLayerMask);

    public override void SetTarget(Target target, bool isTargetFound)
    {
        if (isTargetFound && !_attack.IsAttacking)
        {
            _attack.SetTarget(target);
        }
    }
}
