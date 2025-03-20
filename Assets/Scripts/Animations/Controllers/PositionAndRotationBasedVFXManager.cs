using UnityEngine;

public class PositionAndRotationBasedVFXManager : BaseVFXManager
{
    public void TriggerAnimationAtPositionLooped(Vector3 spawnPosition, Quaternion spawnRotation) 
    {
        var animationTrigger = _instantiateAnimationTrigger(spawnPosition, spawnRotation);

        _triggerAnimation(animationTrigger, AnimationTriggerType.Loop);
    }
    
    public void TriggerAnimationAtPositionOnce(Vector3 spawnPosition, Quaternion spawnRotation) 
    {
        var animationTrigger = _instantiateAnimationTrigger(spawnPosition, spawnRotation);

        _triggerAnimation(animationTrigger, AnimationTriggerType.Once);
    }

    protected virtual IVFXAnimationTrigger _instantiateAnimationTrigger(Vector3 spawnPosition, Quaternion spawnRotation) 
    {
        var newAnimation = Instantiate(_animatedPrefab, spawnPosition, spawnRotation);

        VFXCleanerManager.Instance.AddVFXObject(newAnimation);

        _cachedAnimations.Add(newAnimation);

        return newAnimation.GetComponentInChildren<IVFXAnimationTrigger>();
    }
}
