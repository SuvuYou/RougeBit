using UnityEngine;

public class AttackAnimationController : MonoBehaviour
{
    private readonly static int ATTACK_HASH = Animator.StringToHash("Attack");
    private readonly static int ATTACK_DIRECTION_X_HASH = Animator.StringToHash("AttackDirectionX");
    private readonly static int ATTACK_DIRECTION_Y_HASH = Animator.StringToHash("AttackDirectionY");

    [SerializeField] private Animator _animator;

    public void TriggerAttackAnimation(Vector2 attackDirection)
    {
        _animator.SetFloat(ATTACK_DIRECTION_X_HASH, attackDirection.x);
        _animator.SetFloat(ATTACK_DIRECTION_Y_HASH, attackDirection.y);
        _animator.SetTrigger(ATTACK_HASH);
    }
}
