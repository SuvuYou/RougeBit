using System.Collections.Generic;
using UnityEngine;

class ShootingEffectAnimationController : BaseAnimationController
{
    private enum ShootingEffectType
    {
        ElectricShock,
        SmallBullet,
        LaserBeam,
        LaserBeam2,
        LaserBeamContinuous
    }

    private static readonly int IDLE_HASH = Animator.StringToHash("Idle");

    private static readonly Dictionary<ShootingEffectType, int> ShootingEffectHashes = new()
    {
        { ShootingEffectType.ElectricShock, Animator.StringToHash("Electric Shock Shot") },
        { ShootingEffectType.SmallBullet, Animator.StringToHash("Small Bullet Shot") },
        { ShootingEffectType.LaserBeam, Animator.StringToHash("Laser Beam") },
        { ShootingEffectType.LaserBeam2, Animator.StringToHash("Laser Beam 2") },
        { ShootingEffectType.LaserBeamContinuous, Animator.StringToHash("Laser Beam Continuous") }
    };

    private static readonly Dictionary<ShootingEffectType, string> ShootingEffectClipNames = new()
    {
        { ShootingEffectType.ElectricShock, "Electric Shock Shot" },
        { ShootingEffectType.SmallBullet, "Small Bullet Shot" },
        { ShootingEffectType.LaserBeam, "Laser Beam" },
        { ShootingEffectType.LaserBeam2, "Laser Beam 2" },
        { ShootingEffectType.LaserBeamContinuous, "Laser Beam Continuous" }
    };


    [Space(20)]
    [SerializeField] private ShootingEffectType _shootingEffectType;

    private float _shootingAnimationLength = 0f;

    private void Start()
    {
        _shootingAnimationLength = _animator.GetClipLength(ShootingEffectClipNames[_shootingEffectType]);
    }

    protected void Update()
    {
        _switchAnimationState(IDLE_HASH);
    }

    public void TriggerShootingEffectAnimation() => _switchAnimationState(ShootingEffectHashes[_shootingEffectType], lockTime: _shootingAnimationLength);
}
