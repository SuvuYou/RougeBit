using System.Collections.Generic;
using UnityEngine;

public class TargetingEffectAnimationController : BaseAnimationController, IVFXAnimationTrigger
{
    private enum TargetingEffectType
    {
        DiamondTargeting,
    }

    private static readonly int IDLE_HASH = Animator.StringToHash("Idle");

    private static readonly Dictionary<TargetingEffectType, int> TargetingEffectHashes = new()
    {
        { TargetingEffectType.DiamondTargeting, Animator.StringToHash("DiamondTargeting") },
    };

    private static readonly Dictionary<TargetingEffectType, string> TargetingEffectClipNames = new()
    {
        { TargetingEffectType.DiamondTargeting, "DiamondTargeting" },
    };

    [Space(20)]
    [SerializeField] private TargetingEffectType _targetingEffectType;
    [SerializeField] private float _overrideTargetingAnimationLength = 0f;

    private float _targetingAnimationLength = 0f;

    private bool _isLockedInLoop = false;

    private float _animationLength => _overrideTargetingAnimationLength > 0f ? _overrideTargetingAnimationLength : _targetingAnimationLength;

    private void Start()
    {
        _targetingAnimationLength = _animator.GetClipLength(TargetingEffectClipNames[_targetingEffectType]);
    }

    protected void Update()
    {
        if (_isLockedInLoop) return;

        _switchAnimationState(IDLE_HASH);
    }

    public void TriggerAnimation() => _switchAnimationState(TargetingEffectHashes[_targetingEffectType], lockTime: _animationLength);

    public void TriggerAnimationLooped() 
    {
        _isLockedInLoop = true;
        
        _switchAnimationState(TargetingEffectHashes[_targetingEffectType]);
    }

    public void TriggerAnimationOnce()
    {
        _switchAnimationState(TargetingEffectHashes[_targetingEffectType], lockTime: _animationLength);
        StartCoroutine(_destroyAfterSeconds(_animationLength));
    } 

    private System.Collections.IEnumerator _destroyAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
