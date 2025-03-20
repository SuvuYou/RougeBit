using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundEventsSO", menuName = "ScriptableObjects/SoundEventsSO")]
public class SoundEventsSO : ScriptableObject
{
    public enum GameSound
    {
        COIN_PICKUP = 100,
        
        ENEMY_PROJECTILE_SHOOTING = 200,
        PLAYER_PROJECTILE_SHOOTING = 300,
        PLACE_MINE = 400,
        ROCKET_LAUNCHING = 500,
        LASER_BEAM = 600,
        LASER_BEAM_CHARGE = 700,
        COLLISION = 800,
        SLASH = 900,

        EXPLOSION = 1000,
        TARGET_PLACING = 1100,
        DASH = 1200,

        PLAYER_DEATH = 1300,
        ENEMY_DEATH = 1400,
        PLAYER_HURT = 1500,
        ENEMY_SPAWN = 1600,
    }

    public UglySerializableDictionary<GameSound, GameSoundInfo> SoundLookup;

    public void TriggerExplosionSoundEvent(Transform triggerLocationTransform) => SoundManager.Instance.PlaySoundAtPoint(GameSound.EXPLOSION, triggerLocationTransform.position); 

    public void TriggerPickupSoundEvent(Transform triggerLocationTransform) => SoundManager.Instance.PlaySoundAtPoint(GameSound.COIN_PICKUP, triggerLocationTransform.position); 

    public void TriggerPlayerHurtSoundEvent(Transform triggerLocationTransform) => SoundManager.Instance.PlaySoundAtPoint(GameSound.PLAYER_HURT, triggerLocationTransform.position); 
    
    public void TriggerEnemyProjectileShootingSoundEvent(Transform triggerLocationTransform) => SoundManager.Instance.PlaySoundAtPoint(GameSound.ENEMY_PROJECTILE_SHOOTING, triggerLocationTransform.position);

    public void TriggerPlayerProjectileShootingSoundEvent(Transform triggerLocationTransform) => SoundManager.Instance.PlaySoundAtPoint(GameSound.PLAYER_PROJECTILE_SHOOTING, triggerLocationTransform.position); 

    public void TriggerPlaceMineSoundEvent(Transform triggerLocationTransform) => SoundManager.Instance.PlaySoundAtPoint(GameSound.PLACE_MINE, triggerLocationTransform.position); 

    public void TriggerRocketLaunchSoundEvent(Transform triggerLocationTransform) => SoundManager.Instance.PlaySoundAtPoint(GameSound.ROCKET_LAUNCHING, triggerLocationTransform.position); 

    public void TriggerLaserBeamSoundEvent(Transform triggerLocationTransform) => SoundManager.Instance.PlaySoundAtPoint(GameSound.LASER_BEAM, triggerLocationTransform.position, volume: 0.2f); 

    public void TriggerLaserBeamChargeSoundEvent(Transform triggerLocationTransform) => SoundManager.Instance.PlaySoundAtPoint(GameSound.LASER_BEAM_CHARGE, triggerLocationTransform.position, volume: 0.05f); 

    public void TriggerTargetPlacingSoundEvent(Transform triggerLocationTransform) => SoundManager.Instance.PlaySoundAtPoint(GameSound.TARGET_PLACING, triggerLocationTransform.position, volume: 0.25f); 

    public void TriggerCollisionSoundEvent(Transform triggerLocationTransform) => SoundManager.Instance.PlaySoundAtPoint(GameSound.COLLISION, triggerLocationTransform.position); 

    public void TriggerDashSoundEvent(Transform triggerLocationTransform) => SoundManager.Instance.PlaySoundAtPoint(GameSound.DASH, triggerLocationTransform.position); 

    public void TriggerSlashSoundEvent(Transform triggerLocationTransform) => SoundManager.Instance.PlaySoundAtPoint(GameSound.SLASH, triggerLocationTransform.position); 

    public void TriggerPlayerDeathSoundEvent(Transform triggerLocationTransform) => SoundManager.Instance.PlaySoundAtPoint(GameSound.PLAYER_DEATH, triggerLocationTransform.position); 

    public void TriggerEnemyDeathSoundEvent(Transform triggerLocationTransform) => SoundManager.Instance.PlaySoundAtPoint(GameSound.ENEMY_DEATH, triggerLocationTransform.position); 
     
    public void TriggerEnemySpawnSoundEvent(Transform triggerLocationTransform) => SoundManager.Instance.PlaySoundAtPoint(GameSound.ENEMY_SPAWN, triggerLocationTransform.position); 
}

[System.Serializable]
public class GameSoundInfo
{
    public List<AudioClip> SoundClips;

    public int InstanceLimit = 2;

    public bool ShouldStopOldestSourceOnLimitReached = false;
}