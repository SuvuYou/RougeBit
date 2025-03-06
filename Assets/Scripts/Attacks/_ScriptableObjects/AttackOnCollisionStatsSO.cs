using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "ScriptableObjects/Attacks/AttackOnCollisionStatsSO")]
public class AttackOnCollisionStatsSO : ScriptableObject
{
    public float AttackDamage = 50f;
    public float AttackDistance = 2f;
    public float CollisionRadius = 1.5f;
}
