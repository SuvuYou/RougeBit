using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "ScriptableObjects/Attacks/Projectiles/MineStatsSO")]
public class MineStatsSO : ScriptableObject
{ 
    public LayerMask EnemyLayerMask;

    public float Damage = 50f;
    public float ExplosionRadious = 2f;
}
