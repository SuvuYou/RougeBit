using UnityEngine;

public class BaseWeaponVisual : MonoBehaviour
{
    [Header("Sprite references")]
    [SerializeField] private RotateSpritesetByDirection _weaponSpriteRotator;
    [SerializeField] private SnapToRotationByDirection _shootingSpriteRotator;

    public bool IsRotationEnabled { get; private set; } = true;

    public void EnableRotation() => IsRotationEnabled = true;
    public void DisableRotation() => IsRotationEnabled = false;

    protected Vector3 _currentTarget;

    public void SetTarget(Vector3 target) 
    {
        _currentTarget = target;

        _weaponSpriteRotator.SetTarget(target);
        _shootingSpriteRotator.SetTarget(target);
    }

    protected virtual void Update()
    {
        if (IsRotationEnabled && _currentTarget != null)
        {
            _weaponSpriteRotator.Rotate();
            _shootingSpriteRotator.Rotate();
        }
    } 
}
