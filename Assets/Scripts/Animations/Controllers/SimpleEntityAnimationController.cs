using UnityEngine;

class SimpleEntityAnimationController : BaseEntityAnimationController
{
    [Space(20)]
    [SerializeField] private BaseDamagable _baseDamagable;
    [SerializeField] private Spawnable _spawnable;

    [SerializeField] private bool _includeIdleAnimations = false;

    private static readonly int RUN_SD_HASH = Animator.StringToHash("Walk_Side_Down");
    private static readonly int RUN_SU_HASH = Animator.StringToHash("Walk_Side_Up"); 

    private static readonly int DIE_SD_HASH = Animator.StringToHash("Death_Side_Down");
    private static readonly int DIE_SU_HASH = Animator.StringToHash("Death_Side_Up"); 

    private static readonly int IDLE_SD_HASH = Animator.StringToHash("Idle_Side_Down");
    private static readonly int IDLE_SU_HASH = Animator.StringToHash("Idle_Side_Up"); 

    private static readonly int SPAWN_SD_HASH = Animator.StringToHash("Spawn_Side_Down");
    private static readonly int SPAWN_SU_HASH = Animator.StringToHash("Spawn_Side_Up"); 

    private float _deathAnimationLength;
    private float _spawnAnimationLength;

    private void Awake()
    {
        _deathAnimationLength = _animator.GetClipLength("Death_Side_Down");
        _baseDamagable.SetObjectDestroyDelay(_deathAnimationLength);

        _spawnAnimationLength = _animator.GetClipLength("Spawn_Side_Down");
        _spawnable.SetSpawnDuration(_spawnAnimationLength);
    }

    protected override void Update()
    {
        base.Update();

        if (!_characterMovement.Enabled || _characterMovement.Velocity.magnitude <= 0f) 
        {
            if (_includeIdleAnimations) _switchAnimationState(_isFaceDown ? IDLE_SD_HASH : IDLE_SU_HASH);

            return;
        }

        _switchAnimationState(_isFaceDown ? RUN_SD_HASH : RUN_SU_HASH);
    }

    public void TriggerDeathAnimation() => _switchAnimationState(_isFaceDown ? DIE_SD_HASH : DIE_SU_HASH, lockTime: _deathAnimationLength);

    public void TriggerSpawnAnimation() => _switchAnimationState(_isFaceDown ? SPAWN_SD_HASH : SPAWN_SU_HASH, lockTime: _spawnAnimationLength);
}
