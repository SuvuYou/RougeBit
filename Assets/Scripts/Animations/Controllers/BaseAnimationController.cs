using UnityEngine;

public class BaseAnimationController : MonoBehaviour
{
    [Header("Base References")]
    [SerializeField] protected Animator _animator;
    [SerializeField] protected CharacterMovement _characterMovement;
    [SerializeField] protected SpriteRenderer _spriteRenderer;

    protected int _currentState;

    protected bool _isFaceDown => _characterMovement.LastNonZeroDirection.y < 0;
    protected bool _isFaceLeft => _characterMovement.LastNonZeroDirection.x < 0;

    protected float _lockedUntillTime = 0;

    protected virtual void Update()
    {
        _spriteRenderer.flipX = _isFaceLeft;
    }

    protected void _switchAnimationState(int newState, float lockTime = 0f)
    {
        if (Time.time < _lockedUntillTime) return;
        if (_currentState == newState) return;

        _animator.CrossFade(newState, 0, 0);
        _currentState = newState;

        _lockedUntillTime = Time.time + lockTime;
    }
}
