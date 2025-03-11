using UnityEngine;

class ComplexEntityAnimationController : BaseEntityAnimationController
{
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

    private float _attackAnimationLength = 0f;
    private float _hurtAnimationLength = 0f;

    private void Start()
    {
        _attackAnimationLength = _animator.GetClipLength("Attack_Side");
        _hurtAnimationLength = _animator.GetClipLength("Hurt_Side");
    }

    protected override void Update()
    {
        base.Update();

        if (!_characterMovement.Enabled || _characterMovement.Velocity.magnitude <= 0f) 
        {
            if (_characterMovement.LastNonZeroDirection.x.AbsoluteValue() > _characterMovement.LastNonZeroDirection.y.AbsoluteValue())
            {
                _switchAnimationState(IDLE_SIDE_HASH);

                return;
            }

            _switchAnimationState(_isFaceDown ? IDLE_DOWN_HASH : IDLE_UP_HASH);

            return;
        }

        if (_characterMovement.LastNonZeroDirection.x.AbsoluteValue() > _characterMovement.LastNonZeroDirection.y.AbsoluteValue())
        {
            _switchAnimationState(WALK_SIDE_HASH);

            return;
        }

        _switchAnimationState(_isFaceDown ? WALK_DOWN_HASH : WALK_UP_HASH);
    }


    public void TriggerAttackAnimation(Vector2 attackDirection)
    {
         if (attackDirection.x.AbsoluteValue() > attackDirection.y.AbsoluteValue())
        {
            _switchAnimationState(ATTACK_SIDE_HASH, lockTime: _attackAnimationLength);

            return;
        }

        _switchAnimationState(attackDirection.y > 0 ? ATTACK_DOWN_HASH : ATTACK_UP_HASH, lockTime: _attackAnimationLength);
    }

    public void TriggerHurtAnimation()
    {
         if (_characterMovement.LastNonZeroDirection.x.AbsoluteValue() > _characterMovement.LastNonZeroDirection.y.AbsoluteValue())
        {
            _switchAnimationState(HURT_SIDE_HASH, lockTime: _hurtAnimationLength);

            return;
        }

        _switchAnimationState(_characterMovement.LastNonZeroDirection.y > 0 ? HURT_DOWN_HASH : HURT_UP_HASH, lockTime: _hurtAnimationLength);
    }
}
