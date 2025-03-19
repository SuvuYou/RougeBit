using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundEventsSO", menuName = "ScriptableObjects/SoundEventsSO")]
public class SoundEventsSO : ScriptableObject
{
    public enum GameSound
    {
        EXPLOSION,
        COIN_PICKUP,
        SHOOTING,
        HURT
    }

    public UglySerializableDictionary<GameSound, GameSoundInfo> SoundLookup;

    public void TriggerExplosionSoundEvent(Transform triggerLocationTransform) => SoundManager.Instance.PlaySoundAtPoint(GameSound.EXPLOSION, triggerLocationTransform.position); 

    public void TriggerPickupSoundEvent(Transform triggerLocationTransform) => SoundManager.Instance.PlaySoundAtPoint(GameSound.COIN_PICKUP, triggerLocationTransform.position); 
    
    public void TriggerShootingSoundEvent(Transform triggerLocationTransform) => SoundManager.Instance.PlaySoundAtPoint(GameSound.SHOOTING, triggerLocationTransform.position); 

    public void TriggerHurtSoundEvent(Transform triggerLocationTransform) => SoundManager.Instance.PlaySoundAtPoint(GameSound.HURT, triggerLocationTransform.position); 
}

[System.Serializable]
public class GameSoundInfo
{
    public List<AudioClip> SoundClips;

    public int InstanceLimit = 2;

    public bool ShouldStopOldestSourceOnLimitReached = false;

    [HideInInspector]
    public List<AudioSource> CurrentSources = new ();
}