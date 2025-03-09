using UnityEngine;

public class CollisionWeapon : BaseWeapon
{
    [SerializeField] AttackOnCollision _attack;

    public override void Setup(GameObject attacker) => _attack.Setup(attacker);

    public override void SetTarget(Target target, bool isTargetFound)
    {
        if (isTargetFound && !_attack.IsAttacking)
        {
            _attack.SetTarget(target);
        }
    }
}
