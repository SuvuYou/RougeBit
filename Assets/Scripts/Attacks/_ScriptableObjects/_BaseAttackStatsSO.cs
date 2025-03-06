using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "ScriptableObjects/Attacks/_BaseAttackStatsSO")]
public class _BaseAttackStatsSO : ScriptableObject
{
    [Header("Target")]
    public LayerMask EnemyLayerMask;

    [Header("Reload")]
    public float ReloadDuration;

    [Header("Aim")]
    public bool RequiresAim; 
    public float AimDuration; 

    [Header("Knockback")]
    public bool AddsKnockback; 
}
