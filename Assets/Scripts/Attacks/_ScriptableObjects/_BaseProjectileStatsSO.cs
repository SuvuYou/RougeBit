using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "ScriptableObjects/Attacks/Projectiles/_BaseProjectileStatsSO")]
public class _BaseProjectileStatsSO : ScriptableObject
{
    [Header("Projectile")]
    public Sprite ProjectileSprite;

    [Header("Target")]
    public LayerMask EnemyLayerMask;

    [Header("Stats")]
    public float Damage;
    public float Speed;
    public float LifeTime;
}
