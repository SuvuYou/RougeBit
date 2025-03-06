using UnityEngine;

public class TakeDamageAnimationController : MonoBehaviour
{
    private readonly static int HURT_HASH = Animator.StringToHash("Hurt");

    [SerializeField] private Animator _animator;

    public void TriggerHurtAnimation()
    {
        _animator.SetTrigger(HURT_HASH);
    }
}
