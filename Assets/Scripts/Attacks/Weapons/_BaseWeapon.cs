using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    protected GameObject _attacker;

    public enum TargetType { Player, Enemy, EnemyProjectile, PlayerProjectile};

    [field: SerializeField] public TargetType WeaponTargetType { get; private set; }

    public abstract void SetTarget(Target targetPosition, bool isTargetFound);

    public virtual void Setup(GameObject attacker, LayerMask enemyLayerMask) { }

    public virtual void Activate () { }
    public virtual void Deactivate () { }
}
