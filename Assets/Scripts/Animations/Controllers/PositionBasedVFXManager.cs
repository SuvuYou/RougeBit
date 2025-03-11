using UnityEngine;

public class PositionBasedVFXManager : BaseVFXManager
{
    public void TriggerAnimationAtPositionLooped(Vector3 spawnPosition) 
    {
        var animationTrigger = _instantiateAnimationTrigger(spawnPosition);

        _triggerAnimation(animationTrigger, AnimationTriggerType.Loop);
    }
    
    public void TriggerAnimationAtPositionOnce(Vector3 spawnPosition) 
    {
        var animationTrigger = _instantiateAnimationTrigger(spawnPosition);

        _triggerAnimation(animationTrigger, AnimationTriggerType.Once);
    }

    protected virtual IVFXAnimationTrigger _instantiateAnimationTrigger(Vector3 spawnPosition) 
    {
        _cachedAnimation = Instantiate(_animatedPrefab, spawnPosition, Quaternion.identity);

        return _cachedAnimation.GetComponentInChildren<IVFXAnimationTrigger>();
    }
}
