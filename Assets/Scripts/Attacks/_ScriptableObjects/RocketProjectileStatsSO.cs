using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "ScriptableObjects/Attacks/Projectiles/RocketProjectileStatsSO")]
public class RocketProjectileStatsSO : ScriptableObject
{
    [Header("Rocket Projectile")]
    public Sprite RocketProjectileSprite;

    [Header("Target layer mask")]
    public LayerMask EnemyLayerMask;

    [Header("Stats")]
    public float Damage;
    public float Speed;
    public float RotationSpeed;
    public float LifeTime;

    [Header("Ranges")]
    public float ExplosionRadious;
    public float SearchRadious;

    [Header("Deviation")]
    public float DeviationStrength;
    public float DeviationCycleSpeed;
}
