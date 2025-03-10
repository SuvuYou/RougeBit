using UnityEngine;

public class BaseVFXManager : MonoBehaviour
{
    public enum AnimationTriggerType
    {
        Once,
        Loop,
        Internal
    }

    [SerializeField] protected GameObject _animatedPrefab;

    protected GameObject _cachedAnimation;

    private void Awake()
    {
        if (_animatedPrefab != null && _animatedPrefab.GetComponentInChildren<IVFXAnimationTrigger>() == null)
        {
            Debug.LogError("Assigned GameObject does not implement the required interface!");
        }
    }

    protected void _triggerAnimation(IVFXAnimationTrigger trigger, AnimationTriggerType triggerType = AnimationTriggerType.Loop)
    {
        switch (triggerType)
        {
            case AnimationTriggerType.Once:
                trigger.TriggerAnimationOnce();
                break;
            case AnimationTriggerType.Loop:
                trigger.TriggerAnimationLooped();
                break;
            case AnimationTriggerType.Internal:
                trigger.TriggerAnimation();
                break;
            default:
                break;
        }
    }

    public void DestroyAnimation() => Destroy(_cachedAnimation?.gameObject);
}
