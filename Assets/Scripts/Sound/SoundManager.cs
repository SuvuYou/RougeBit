using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singlton<SoundManager>
{
    [Header("Audio Settings")]
    [SerializeField] private SoundEventsSO _soundEvents;
    [SerializeField] private AudioSource _audioPrefab;
    [SerializeField] private int _maxAudioSources = 20;

    private Dictionary<SoundEventsSO.GameSound, GameSoundInfo> _soundLookup;

    private ObjectPool<AudioSource> _audioSourcesPool;

    protected override void Awake()
    {
        base.Awake();

        _soundLookup = _soundEvents.SoundLookup.ToDictionary();

        _audioSourcesPool = new ObjectPool<AudioSource>(
            _audioPrefab, 
            transform, 
            _maxAudioSources, 
            availabilityPredicate: source => !source.isPlaying
        );
    }

    private void Update()
    {
        foreach (GameSoundInfo gameSoundInfo in _soundLookup.Values)
        {
            gameSoundInfo.CurrentSources.RemoveAll(x => !x.isPlaying);
        }
    }

    public void PlaySoundAtPoint(SoundEventsSO.GameSound sound, Vector3 position, float volume = 0.5f, float pitch = 1f)
    {
        if (!_soundLookup.TryGetValue(sound, out GameSoundInfo gameSoundInfo))
        {
            Debug.LogWarning("No sound definition found for " + sound);

            return;
        }

        if (gameSoundInfo.CurrentSources.Count >= gameSoundInfo.InstanceLimit)
        {
            if (!gameSoundInfo.ShouldStopOldestSourceOnLimitReached)
            {
                return;
            }

            AudioSource oldestSource = gameSoundInfo.CurrentSources[0];
            gameSoundInfo.CurrentSources.RemoveAt(0);
            oldestSource.Stop();
        }

        AudioSource source = _audioSourcesPool.GetObject();

        AudioClip clip = gameSoundInfo.SoundClips.Count == 1 ? gameSoundInfo.SoundClips[0] : gameSoundInfo.SoundClips[Random.Range(0, gameSoundInfo.SoundClips.Count)];

        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.transform.position = position;
        source.Play();

        gameSoundInfo.CurrentSources.Add(source);
    }
}