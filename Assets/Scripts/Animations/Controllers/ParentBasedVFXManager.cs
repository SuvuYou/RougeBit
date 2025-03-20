using System.Linq;
using UnityEngine;

public class ParentBasedVFXManager : BaseVFXManager
{
    [SerializeField] Transform _positionParent;
    [SerializeField] Transform _rotationParent;

    public void TriggerAnimationAtPositionLooped() 
    {
        var animationTrigger = _instantiateAnimationTrigger();

        _triggerAnimation(animationTrigger, AnimationTriggerType.Loop);
    }
    
    public void TriggerAnimationAtPositionOnce() 
    {
        var animationTrigger = _instantiateAnimationTrigger();

        _triggerAnimation(animationTrigger, AnimationTriggerType.Once);
    }

    protected virtual IVFXAnimationTrigger _instantiateAnimationTrigger() 
    {
        var newAnimation = Instantiate(_animatedPrefab, _positionParent.position, _rotationParent.rotation);

        VFXCleanerManager.Instance.AddVFXObject(newAnimation);

        _cachedAnimations.Add(newAnimation);

        return newAnimation.GetComponentInChildren<IVFXAnimationTrigger>();
    }
}
