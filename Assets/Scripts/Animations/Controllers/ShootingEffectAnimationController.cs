using System.Collections.Generic;
using UnityEngine;

class ShootingEffectAnimationController : BaseAnimationController, IVFXAnimationTrigger
{
    private enum ShootingEffectType
    {
        ElectricShock,
        SmallBullet,
        LaserBeam,
        LaserBeam2,
        LaserBeamContinuous,
        LaserBeamCharge,
        Slash1,
        Slash2,
        SmallDirectionalExplosion
    }

    private static readonly int IDLE_HASH = Animator.StringToHash("Idle");

    private static readonly Dictionary<ShootingEffectType, int> ShootingEffectHashes = new()
    {
        { ShootingEffectType.ElectricShock, Animator.StringToHash("Electric Shock Shot") },
        { ShootingEffectType.SmallBullet, Animator.StringToHash("Small Bullet Shot") },
        { ShootingEffectType.LaserBeam, Animator.StringToHash("Laser Beam") },
        { ShootingEffectType.LaserBeam2, Animator.StringToHash("Laser Beam 2") },
        { ShootingEffectType.LaserBeamContinuous, Animator.StringToHash("Laser Beam Continuous") },
        { ShootingEffectType.LaserBeamCharge, Animator.StringToHash("Laser Beam Charge") },
        { ShootingEffectType.Slash1, Animator.StringToHash("Slash 1") },
        { ShootingEffectType.Slash2, Animator.StringToHash("Slash 2") },
        { ShootingEffectType.SmallDirectionalExplosion, Animator.StringToHash("Small Directional Explosion") }
    };

    private static readonly Dictionary<ShootingEffectType, string> ShootingEffectClipNames = new()
    {
        { ShootingEffectType.ElectricShock, "Electric Shock Shot" },
        { ShootingEffectType.SmallBullet, "Small Bullet Shot" },
        { ShootingEffectType.LaserBeam, "Laser Beam" },
        { ShootingEffectType.LaserBeam2, "Laser Beam 2" },
        { ShootingEffectType.LaserBeamContinuous, "Laser Beam Continuous" },
        { ShootingEffectType.LaserBeamCharge, "Laser Beam Charge" },
        { ShootingEffectType.Slash1, "Slash 1" },
        { ShootingEffectType.Slash2, "Slash 2" },
        { ShootingEffectType.SmallDirectionalExplosion, "Small Directional Explosion" }
    };

    [Space(20)]
    [SerializeField] private ShootingEffectType _shootingEffectType;
    [SerializeField] private float _overrideShootingAnimationLength = 0f;

    private float _shootingAnimationLength = 0f;

    private bool _isLockedInLoop = false;

    private float _animationLength => _overrideShootingAnimationLength > 0 ? _overrideShootingAnimationLength : _shootingAnimationLength;

    private void Awake()
    {
        _shootingAnimationLength = _animator.GetClipLength(ShootingEffectClipNames[_shootingEffectType]);
    }

    protected void Update()
    {
        if (_isLockedInLoop) return;

        _switchAnimationState(IDLE_HASH);
    }

    public void TriggerAnimation() => _switchAnimationState(ShootingEffectHashes[_shootingEffectType], lockTime: _animationLength);

    public void TriggerAnimationLooped() 
    {
        _isLockedInLoop = true;
        
        _switchAnimationState(ShootingEffectHashes[_shootingEffectType]);
    }

    public void TriggerAnimationOnce()
    {
        _switchAnimationState(ShootingEffectHashes[_shootingEffectType], lockTime: _animationLength);
        StartCoroutine(_destroyAfterSeconds(_animationLength));
    } 

    private System.Collections.IEnumerator _destroyAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
