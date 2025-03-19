using UnityEngine;

public class ParentBasedSoundEventTrigger : MonoBehaviour
{
    [SerializeField] private Transform _parentTransform;
    [SerializeField] private SoundEventsSO _soundEvents;

    // public void TriggerSoundEvent(SoundEvents eventType) => _soundEvents.TriggerSoundEvent(eventType, _parentTransform.position);
}