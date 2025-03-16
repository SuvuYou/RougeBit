using UnityEngine;

public class BaseEntityAnimationController : BaseAnimationController
{
    [Header("Base Entity References")]
    [SerializeField] protected CharacterMovement _characterMovement;
    [SerializeField] protected SpriteRenderer _spriteRenderer;

    [SerializeField] protected bool _isSpriteRightBased = true;

    protected bool _isFaceDown => _characterMovement.LastNonZeroDirection.y < 0;
    protected bool _isFaceLeft => _characterMovement.LastNonZeroDirection.x < 0;

    protected virtual void Update()
    {
        _spriteRenderer.flipX = _isSpriteRightBased && _isFaceLeft || !_isSpriteRightBased && !_isFaceLeft;
    }
}
