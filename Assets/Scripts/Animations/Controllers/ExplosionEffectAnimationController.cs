using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExplosionEffectAnimationController : BaseAnimationController, IVFXAnimationTrigger
{
    private enum ExplosionEffectType
    {
        Boom,
        BigBoom,
        BiggerBoom,
        PurpleBoom1,
        PurpleBoom2,
        DustShockWave
    }

    private static readonly int IDLE_HASH = Animator.StringToHash("Idle");

    private static readonly Dictionary<ExplosionEffectType, int> ExplosionEffectHashes = new()
    {
        { ExplosionEffectType.Boom, Animator.StringToHash("Boom") },
        { ExplosionEffectType.BigBoom, Animator.StringToHash("BigBoom") },
        { ExplosionEffectType.BiggerBoom, Animator.StringToHash("BiggerBoom") },
        { ExplosionEffectType.PurpleBoom1, Animator.StringToHash("PurpleBoom1") },
        { ExplosionEffectType.PurpleBoom2, Animator.StringToHash("PurpleBoom2") },
        { ExplosionEffectType.DustShockWave, Animator.StringToHash("DustShockWave") }
    };

    private static readonly Dictionary<ExplosionEffectType, string> ExplosionEffectClipNames = new()
    {
        { ExplosionEffectType.Boom, "Boom" },
        { ExplosionEffectType.BigBoom, "BigBoom" },
        { ExplosionEffectType.BiggerBoom, "BiggerBoom" },
        { ExplosionEffectType.PurpleBoom1, "PurpleBoom1" },
        { ExplosionEffectType.PurpleBoom2, "PurpleBoom2" },
        { ExplosionEffectType.DustShockWave, "DustShockWave" }
    };

    [Space(20)]
    [SerializeField] private ExplosionEffectType _explosionEffectType;
    [SerializeField] private float _overrideExplosionAnimationLength = 0f;

    public UnityEvent OnExplosion;

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

    public void TriggerAnimation() 
    {
        _switchAnimationState(ExplosionEffectHashes[_explosionEffectType], lockTime: _animationLength);

        OnExplosion?.Invoke();
    }

    public void TriggerAnimationLooped() 
    {
        _isLockedInLoop = true;
        
        _switchAnimationState(ExplosionEffectHashes[_explosionEffectType]);
        OnExplosion?.Invoke();
    }

    public void TriggerAnimationOnce()
    {
        _switchAnimationState(ExplosionEffectHashes[_explosionEffectType], lockTime: _animationLength);
        StartCoroutine(_destroyAfterSeconds(_animationLength));
        OnExplosion?.Invoke();
    } 

    private System.Collections.IEnumerator _destroyAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
