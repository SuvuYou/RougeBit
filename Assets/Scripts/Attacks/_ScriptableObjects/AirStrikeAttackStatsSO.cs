using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Attacks/AirStrikeAttackStatsSO")]
public class AirStrikeAttackStatsSO : ScriptableObject
{
    public GeneralVFXAnimationController TargetingAnimationPrefab;
    public ExplosionEffectAnimationController ExplosionAnimationPrefab; 

    public float Damage = 50f;
    public float ExplosionRadious = 5f;
    public float AttackDistance = 15f;
}
