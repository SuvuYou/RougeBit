using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    public enum TargetType { Player, Enemy, Projectile};

    [field: SerializeField] public TargetType WeaponTargetType { get; private set; }

    public abstract void SetTarget(Target targetPosition, bool isTargetFound);

    public virtual void Setup(GameObject attacker) { }
}
