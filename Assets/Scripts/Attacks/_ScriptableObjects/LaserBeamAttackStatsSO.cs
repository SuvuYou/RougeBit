using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Attacks/LaserBeamAttackStatsSO")]
public class LaserBeamAttackStatsSO : ScriptableObject
{
    public float MovementVelocityToCancelAttackThreshold = 1f;
    public float AttackReachDistance;

    [Header("Beam Damage Settings")]
    public float BeamDamage = 1f;
    public float BeamAttacksPerSecond = 5f;
    public float BeamDuration = 3f;

    [Header("Beam Visuals Settings")]
    public GameObject BeamSprite;
    public float BeamLength = 8f;
    public float BeamWidth = 1f;
}
