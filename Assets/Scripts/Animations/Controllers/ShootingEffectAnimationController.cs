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
        LaserBeamContinuous,
        Slash1,
        Slash2
    }

    private static readonly int IDLE_HASH = Animator.StringToHash("Idle");

    private static readonly Dictionary<ShootingEffectType, int> ShootingEffectHashes = new()
    {
        { ShootingEffectType.ElectricShock, Animator.StringToHash("Electric Shock Shot") },
        { ShootingEffectType.SmallBullet, Animator.StringToHash("Small Bullet Shot") },
        { ShootingEffectType.LaserBeam, Animator.StringToHash("Laser Beam") },
        { ShootingEffectType.LaserBeam2, Animator.StringToHash("Laser Beam 2") },
        { ShootingEffectType.LaserBeamContinuous, Animator.StringToHash("Laser Beam Continuous") },
        { ShootingEffectType.Slash1, Animator.StringToHash("Slash 1") },
        { ShootingEffectType.Slash2, Animator.StringToHash("Slash 2") }
    };

    private static readonly Dictionary<ShootingEffectType, string> ShootingEffectClipNames = new()
    {
        { ShootingEffectType.ElectricShock, "Electric Shock Shot" },
        { ShootingEffectType.SmallBullet, "Small Bullet Shot" },
        { ShootingEffectType.LaserBeam, "Laser Beam" },
        { ShootingEffectType.LaserBeam2, "Laser Beam 2" },
        { ShootingEffectType.LaserBeamContinuous, "Laser Beam Continuous" },
        { ShootingEffectType.Slash1, "Slash 1" },
        { ShootingEffectType.Slash2, "Slash 2" },
    };

    [Space(20)]
    [SerializeField] private ShootingEffectType _shootingEffectType;
    [SerializeField] private float _overrideShootingAnimationLength = 0f;

    private float _shootingAnimationLength = 0f;

    private void Start()
    {
        _shootingAnimationLength = _animator.GetClipLength(ShootingEffectClipNames[_shootingEffectType]);
    }

    protected void Update()
    {
        _switchAnimationState(IDLE_HASH);
    }

    public void TriggerShootingEffectAnimation() => _switchAnimationState(ShootingEffectHashes[_shootingEffectType], lockTime: _overrideShootingAnimationLength > 0f ? _overrideShootingAnimationLength : _shootingAnimationLength);
}
