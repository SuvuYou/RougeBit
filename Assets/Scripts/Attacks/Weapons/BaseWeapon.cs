using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    [Header("Sprite references")]
    [SerializeField] private RotateSpritesetByDirection _weaponSpriteRotator;
    [SerializeField] private SnapToRotationByDirection _shootingSpriteRotator;

    public void SetTarget(Target target) 
    {
        _weaponSpriteRotator.SetTarget(target);
        _shootingSpriteRotator.SetTarget(target);
    } 
}
