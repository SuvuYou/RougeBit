using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public enum AnimationTriggerType
    {
        Once,
        Loop,
        Internal
    }

    [SerializeField] private GameObject _animatedPrefab;

    private GameObject _animation;

    private IVFXAnimationTrigger _animationTrigger;

    private void Awake()
    {
        if (_animatedPrefab != null)
        {
            _animationTrigger = _animatedPrefab.GetComponentInChildren<IVFXAnimationTrigger>();
        }

        if (_animationTrigger == null)
        {
            Debug.LogError("Assigned GameObject does not implement the required interface!");
        }
    }

    public void TriggerAnimationAtPositionLooped(Vector3 spawnPosition) => TriggerAnimationAtPosition(spawnPosition, AnimationTriggerType.Loop);
    public void TriggerAnimationAtPositionOnce(Vector3 spawnPosition) => TriggerAnimationAtPosition(spawnPosition, AnimationTriggerType.Once);

    public void TriggerAnimationAtPosition(Vector3 spawnPosition, AnimationTriggerType triggerType = AnimationTriggerType.Loop)
    {
        _animation = Instantiate(_animatedPrefab, spawnPosition, Quaternion.identity);
        _animationTrigger = _animation.GetComponentInChildren<IVFXAnimationTrigger>();

        switch (triggerType)
        {
            case AnimationTriggerType.Once:
                _animationTrigger.TriggerAnimationOnce();
                break;
            case AnimationTriggerType.Loop:
                _animationTrigger.TriggerAnimationLooped();
                break;
            case AnimationTriggerType.Internal:
                _animationTrigger.TriggerAnimation();
                break;
            default:
                break;
        }
    }

    public void DestroyAnimation() => Destroy(_animation);
}
