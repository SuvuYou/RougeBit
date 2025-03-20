using UnityEngine;
using UnityEngine.Events;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _player;

    [SerializeField] private Vector3 _spawnPosition = Vector3.zero;
    [SerializeField] private Vector3 _despawnPosition;

    public UnityEvent OnPlayerDespawned;
    public UnityEvent OnPlayerSpawned;
    
    public UnityEvent OnPlayerDeactivated;
    public UnityEvent OnPlayerActivated;

    private void Start()
    {
        Despawn();
    }

    public void Spawn()
    {
        _player.transform.position = _spawnPosition;

        OnPlayerSpawned?.Invoke();

        ActivatePlayer();
    }

    public void Despawn()
    {
        _player.transform.position = _despawnPosition;

        OnPlayerDespawned?.Invoke();
    
        DeactivatePlayer();
    }

    public void ActivatePlayer() => OnPlayerActivated?.Invoke();
    public void DeactivatePlayer() => OnPlayerDeactivated?.Invoke();
}