using UnityEngine;

class ComplexEntityAnimationController : BaseEntityAnimationController, IResettable
{
    public void Reset()
    {
        _isDead = false;
    }

    private static readonly int IDLE_DOWN_HASH = Animator.StringToHash("Idle_Down");
    private static readonly int IDLE_SIDE_HASH = Animator.StringToHash("Idle_Side"); 
    private static readonly int IDLE_UP_HASH = Animator.StringToHash("Idle_Up");

    private static readonly int WALK_DOWN_HASH = Animator.StringToHash("Walk_Down");
    private static readonly int WALK_SIDE_HASH = Animator.StringToHash("Walk_Side"); 
    private static readonly int WALK_UP_HASH = Animator.StringToHash("Walk_Up");

    private static readonly int ATTACK_DOWN_HASH = Animator.StringToHash("Attack_Down");
    private static readonly int ATTACK_SIDE_HASH = Animator.StringToHash("Attack_Side"); 
    private static readonly int ATTACK_UP_HASH = Animator.StringToHash("Attack_Up");

    private static readonly int HURT_DOWN_HASH = Animator.StringToHash("Hurt_Down");
    private static readonly int HURT_SIDE_HASH = Animator.StringToHash("Hurt_Side"); 
    private static readonly int HURT_UP_HASH = Animator.StringToHash("Hurt_Up");

    private static readonly int DEATH_DOWN_HASH = Animator.StringToHash("Death_Down");
    private static readonly int DEATH_SIDE_HASH = Animator.StringToHash("Death_Side"); 
    private static readonly int DEATH_UP_HASH = Animator.StringToHash("Death_Up");

    private static readonly int DEATH_IDLE_DOWN_HASH = Animator.StringToHash("Death_Idle_Down");
    private static readonly int DEATH_IDLE_SIDE_HASH = Animator.StringToHash("Death_Idle_Side");
    private static readonly int DEATH_IDLE_UP_HASH = Animator.StringToHash("Death_Idle_Up");

    private float _attackAnimationLength = 0f;
    private float _hurtAnimationLength = 0f;
    private float _deathAnimationLength = 0f;

    private bool _isDead = false;

    private void Start()
    {
        _attackAnimationLength = _animator.GetClipLength("Attack_Side");
        _hurtAnimationLength = _animator.GetClipLength("Hurt_Side");
        _deathAnimationLength = _animator.GetClipLength("Death_Down");
    }

    protected override void Update()
    {
        base.Update();

        if (_isDead) 
        {
            if (_isVectorHorizontal(_characterMovement.LastNonZeroDirection)) 
            {
                _switchAnimationState(DEATH_IDLE_SIDE_HASH);
            }
            else 
            {
                _switchAnimationState(_characterMovement.LastNonZeroDirection.y > 0 ? DEATH_IDLE_DOWN_HASH : DEATH_IDLE_UP_HASH);
            }
        
            return;
        }

        if (!_characterMovement.Enabled || _characterMovement.Velocity.magnitude <= 0f) 
        {
            if (_isVectorHorizontal(_characterMovement.LastNonZeroDirection)) 
            {
                _switchAnimationState(IDLE_SIDE_HASH);
            }
            else 
            {
                _switchAnimationState(_isFaceDown ? IDLE_DOWN_HASH : IDLE_UP_HASH);
            }

            return;
        }

        if (_isVectorHorizontal(_characterMovement.LastNonZeroDirection)) 
        {
            _switchAnimationState(WALK_SIDE_HASH);
        }
        else 
        {
            _switchAnimationState(_isFaceDown ? WALK_DOWN_HASH : WALK_UP_HASH);
        }
    }


    public void TriggerAttackAnimation(Vector2 attackDirection)
    {
        if (_isVectorHorizontal(attackDirection)) 
        {
            _switchAnimationState(ATTACK_SIDE_HASH, lockTime: _attackAnimationLength);
        }
        else 
        {
            _switchAnimationState(attackDirection.y > 0 ? ATTACK_DOWN_HASH : ATTACK_UP_HASH, lockTime: _attackAnimationLength);
        }
    }

    public void TriggerHurtAnimation()
    {
        if (_isVectorHorizontal(_characterMovement.LastNonZeroDirection)) 
        {
            _switchAnimationState(HURT_SIDE_HASH, lockTime: _hurtAnimationLength);
        }
        else 
        {
            _switchAnimationState(_characterMovement.LastNonZeroDirection.y > 0 ? HURT_DOWN_HASH : HURT_UP_HASH, lockTime: _hurtAnimationLength);
        }
    }

    public void TriggerDeathAnimation()
    {
        _isDead = true;

        if (_isVectorHorizontal(_characterMovement.LastNonZeroDirection)) 
        {
            _switchAnimationState(DEATH_SIDE_HASH, lockTime: _hurtAnimationLength);
        }
        else 
        {
            _switchAnimationState(_characterMovement.LastNonZeroDirection.y > 0 ? DEATH_DOWN_HASH : DEATH_UP_HASH, lockTime: _deathAnimationLength, shouldOverride: true);
        }
    }

    private bool _isVectorHorizontal(Vector2 vector) => vector.x.AbsoluteValue() > vector.y.AbsoluteValue();
}
