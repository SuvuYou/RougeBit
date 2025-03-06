using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffectAnimationController : BaseAnimationController, IVFXAnimationTrigger
{
    private enum ExplosionEffectType
    {
        Boom,
        BigBoom,
        BiggerBoom
    }

    private static readonly int IDLE_HASH = Animator.StringToHash("Idle");

    private static readonly Dictionary<ExplosionEffectType, int> ExplosionEffectHashes = new()
    {
        { ExplosionEffectType.Boom, Animator.StringToHash("Boom") },
        { ExplosionEffectType.BigBoom, Animator.StringToHash("BigBoom") },
        { ExplosionEffectType.BiggerBoom, Animator.StringToHash("BiggerBoom") },
    };

    private static readonly Dictionary<ExplosionEffectType, string> ExplosionEffectClipNames = new()
    {
        { ExplosionEffectType.Boom, "Boom" },
        { ExplosionEffectType.BigBoom, "BigBoom" },
        { ExplosionEffectType.BiggerBoom, "BiggerBoom" },
    };

    [Space(20)]
    [SerializeField] private ExplosionEffectType _explosionEffectType;
    [SerializeField] private float _overrideExplosionAnimationLength = 0f;

    private float _explosionAnimationLength = 0f;

    private bool _isLockedInLoop = false;

    private float _animationLength => _overrideExplosionAnimationLength > 0f ? _overrideExplosionAnimationLength : _explosionAnimationLength;

    private void Awake()
    {
        _explosionAnimationLength = _animator.GetClipLength(ExplosionEffectClipNames[_explosionEffectType]);
    }

    protected void Update()
    {
        if (_isLockedInLoop) return;

        _switchAnimationState(IDLE_HASH);
    }

    public void TriggerAnimation() => _switchAnimationState(ExplosionEffectHashes[_explosionEffectType], lockTime: _animationLength);

    public void TriggerAnimationLooped() 
    {
        _isLockedInLoop = true;
        
        _switchAnimationState(ExplosionEffectHashes[_explosionEffectType]);
    }

    public void TriggerAnimationOnce()
    {
        _switchAnimationState(ExplosionEffectHashes[_explosionEffectType], lockTime: _animationLength);
        StartCoroutine(_destroyAfterSeconds(_animationLength));
    } 

    private System.Collections.IEnumerator _destroyAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
