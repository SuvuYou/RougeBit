using UnityEngine;

public class BaseEntityAnimationController : BaseAnimationController
{
    [Header("Base Entity References")]
    [SerializeField] protected CharacterMovement _characterMovement;
    [SerializeField] protected SpriteRenderer _spriteRenderer;

    protected bool _isFaceDown => _characterMovement.LastNonZeroDirection.y < 0;
    protected bool _isFaceLeft => _characterMovement.LastNonZeroDirection.x < 0;

    protected virtual void Update()
    {
        _spriteRenderer.flipX = _isFaceLeft;
    }
}
