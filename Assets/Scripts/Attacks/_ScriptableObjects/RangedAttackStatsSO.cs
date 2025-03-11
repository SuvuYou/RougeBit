using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Attacks/RangedAttackStatsSO")]
public class RangedAttackStatsSO : ScriptableObject
{
    public BaseProjectile[] ProjectilePrefabs; 

    public float MovementVelocityToCancelAttackThreshold = 1f;
    public float AttackDistance = 15f;

    [Range(1, 12)] public int NumberOfProjectiles = 1;
    [Range(0, 360)] public float SpreadAngle = 45f;
}
