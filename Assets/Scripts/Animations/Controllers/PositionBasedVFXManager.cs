using System.Linq;
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
        var newAnimation = Instantiate(_animatedPrefab, spawnPosition, Quaternion.identity);

        _cachedAnimations.Add(newAnimation);

        return newAnimation.GetComponentInChildren<IVFXAnimationTrigger>();
    }
}
