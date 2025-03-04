using UnityEngine;

class EntityAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private CharacterMovement _characterMovement;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [SerializeField] private BaseDamagable _baseDamagable;

    private static readonly int RUN_SD_HASH = Animator.StringToHash("Walk_Side_Down");
    private static readonly int RUN_SU_HASH = Animator.StringToHash("Walk_Side_Up"); 
    private static readonly int DIE_SD_HASH = Animator.StringToHash("Death_Side_Down");
    private static readonly int DIE_SU_HASH = Animator.StringToHash("Death_Side_Up"); 

    private int _currentState;

    private bool _isFaceDown => _characterMovement.LastNonZeroDirection.y < 0;
    private bool _isFaceLeft => _characterMovement.LastNonZeroDirection.x < 0;

    private float _deathAnimationLength;

    private float _lockedUntillTime = 0;

    private void Start()
    {
        _deathAnimationLength = _getAnimationClipLength("Death_Side_Down");
        _baseDamagable.SetObjectDestroyDelay(_deathAnimationLength);
    }

    private void Update()
    {
        _spriteRenderer.flipX = _isFaceLeft;

        if (!_characterMovement.Enabled || _characterMovement.Velocity.magnitude <= 0f) return;

        _switchAnimationState(_isFaceDown ? RUN_SD_HASH : RUN_SU_HASH);
    }

    private void _switchAnimationState(int newState, float lockTime = 0f)
    {
        if (Time.time < _lockedUntillTime) return;
        if (_currentState == newState) return;

        _animator.CrossFade(newState, 0, 0);
        _currentState = newState;

        _lockedUntillTime = Time.time + lockTime;
    }

    public void TriggerDeathAnimation() => _switchAnimationState(_isFaceDown ? DIE_SD_HASH : DIE_SU_HASH, lockTime: _deathAnimationLength);

    private float _getAnimationClipLength(string clipName)
    {
        foreach (AnimationClip clip in _animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName)
                return clip.length;
        }
        
        return 0f;
    }
}
