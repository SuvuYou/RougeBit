using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Attacks/DashAttackOnCollisionStatsSO")]
public class DashAttackOnCollisionStatsSO : ScriptableObject
{
    [Header("Dash Settings")]
    public float AttackDamage = 10f;
    public float DashTriggerDistance = 6f;
    public float DashDistance = 3f;
    [Space(15)]

    [Header("Collision Settings")]
    public float AttackReachDistance = 2f;
    public float CollisionRadius = 2.5f;
}
