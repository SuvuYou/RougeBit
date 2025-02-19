using UnityEngine;

public class EntityMovementAnimatorController : MonoBehaviour
{
    private readonly static int MOVEMENT_X_HASH = Animator.StringToHash("MovementX");
    private readonly static int MOVEMENT_Y_HASH = Animator.StringToHash("MovementY");
    private readonly static int IS_MOVING_HASH = Animator.StringToHash("IsMoving");

    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private CharacterMovement _characterMovement;

    [SerializeField] private float _movementThreshold = 0.1f;

    private void Update()
    {
        _animator.SetFloat(MOVEMENT_X_HASH, _characterMovement.LastNonZeroDirection.x);
        _animator.SetFloat(MOVEMENT_Y_HASH, _characterMovement.LastNonZeroDirection.y);
        _animator.SetBool(IS_MOVING_HASH, _characterMovement.CurrentDirection.magnitude > _movementThreshold);

        _spriteRenderer.flipX = _characterMovement.LastNonZeroDirection.x > 0;
    }
}
