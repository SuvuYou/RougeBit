using UnityEngine;

public class BaseWeaponVisual : MonoBehaviour
{
    [Header("Sprite references")]
    [SerializeField] private RotateSpritesetByDirection _weaponSpriteRotator;
    [SerializeField] private SnapToRotationByDirection _shootingSpriteRotator;

    public bool IsRotationEnabled { get; private set; } = true;

    public void EnableRotation() => IsRotationEnabled = true;
    public void DisableRotation() => IsRotationEnabled = false;

    public void SetTarget(Target target) 
    {
        _weaponSpriteRotator.SetTarget(target);
        _shootingSpriteRotator.SetTarget(target);
    }

    private void Update()
    {
        if (IsRotationEnabled)
        {
            _weaponSpriteRotator.Rotate();
            _shootingSpriteRotator.Rotate();
        }
    } 
}
