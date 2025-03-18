using System.Collections.Generic;
using UnityEngine;

public class GeneralVFXAnimationController : BaseAnimationController, IVFXAnimationTrigger
{
    private enum GeneralEffectType
    {
        DiamondTargeting,
        ElectricCharge,
        TargetSpriteTargeting
    }

    private static readonly int IDLE_HASH = Animator.StringToHash("Idle");

    private static readonly Dictionary<GeneralEffectType, int> TargetingEffectHashes = new()
    {
        { GeneralEffectType.DiamondTargeting, Animator.StringToHash("DiamondTargeting") },
        { GeneralEffectType.ElectricCharge, Animator.StringToHash("ElectricCharge") },
        { GeneralEffectType.TargetSpriteTargeting, Animator.StringToHash("TargetSpriteTargeting") },
    };

    private static readonly Dictionary<GeneralEffectType, string> TargetingEffectClipNames = new()
    {
        { GeneralEffectType.DiamondTargeting, "DiamondTargeting" },
        { GeneralEffectType.ElectricCharge, "ElectricCharge" },
        { GeneralEffectType.TargetSpriteTargeting, "TargetSpriteTargeting" },
    };

    [Space(20)]
    [SerializeField] private GeneralEffectType _targetingEffectType;
    [SerializeField] private float _overrideTargetingAnimationLength = 0f;

    private float _targetingAnimationLength = 0f;

    private bool _isLockedInLoop = false;

    private float _animationLength => _overrideTargetingAnimationLength > 0f ? _overrideTargetingAnimationLength : _targetingAnimationLength;

    private void Awake()
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
