using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class WaveManager : Singlton<WaveManager>, IResettable
{
    public void Reset()
    {
        _clearEnemies();
        _switchStates(WaveManagerStates.Idle);
    }

    private enum WaveManagerStates 
    {
        Idle,
        Spawning,
        OnTimeout
    } 

    public List<Enemy> Enemies { get; private set; } = new();

    [Header("Target for spawned enemies")]
    [SerializeField] private Target _enemyTarget;

    [Header("Spawn position settings")]
    [SerializeField] private float _safeRadious = 10f;

    [SerializeField] private Vector2 _lowerSpawnBounds;
    [SerializeField] private Vector2 _upperSpawnBounds; 

    private WaveConfigSO _currentWaveConfig;

    private WaveManagerStates _currentState = WaveManagerStates.Idle;

    private Timer _timeoutTimer = new (0f);

    private void Update()
    {
        switch(_currentState)
        {
            case WaveManagerStates.Idle:
                break;
            case WaveManagerStates.Spawning:
                SpawnEnemies(_currentWaveConfig);
                break;
        }
    }

    public void StartNextWave()
    {
        _startWave(GameManager.Instance.GameLevels.GetCurrentLevel());
    }

    public void StopWave() 
    {  
        _clearEnemies();
        _switchStates(WaveManagerStates.Idle);
    } 

    private void _startWave(WaveConfigSO waveConfig)
    {
        _currentWaveConfig = waveConfig;
        _switchStates(WaveManagerStates.Spawning);
    }

    private Vector3 _getRandomSpawnPosition()
    {
        Vector3 spawnPosition;

        do 
        {
            spawnPosition = new (UnityEngine.Random.Range(_lowerSpawnBounds.x, _upperSpawnBounds.x), UnityEngine.Random.Range(_lowerSpawnBounds.y, _upperSpawnBounds.y), 0);
        } while (Vector3.Distance(_enemyTarget.transform.position, spawnPosition) <= _safeRadious);

        return spawnPosition;
    }

    #region State management
    private void _switchStates(WaveManagerStates newState)
    {   
        _currentState = newState;

        if (_currentState == WaveManagerStates.Spawning)
        {
            _timeoutTimer.SetBaseTime(UnityEngine.Random.Range(_currentWaveConfig.MinGroupSpawnTimeout, _currentWaveConfig.MaxGroupSpawnTimeout));
            _timeoutTimer.Start();   
        }
        
        if (_currentState == WaveManagerStates.OnTimeout)
        {
            _timeoutTimer.Stop(); 
        }
    }
    #endregion

    #region Enemy spawn
    public void SpawnEnemies(WaveConfigSO waveConfig)
    {
        _timeoutTimer.Update(Time.deltaTime);

        if (_timeoutTimer.IsFinished) 
        {
            SpawnEnemyGroup(waveConfig);

            _timeoutTimer.SetBaseTime(UnityEngine.Random.Range(waveConfig.MinGroupSpawnTimeout, waveConfig.MaxGroupSpawnTimeout));
            _timeoutTimer.Start();   
        }
    }

    public void SpawnEnemyGroup(WaveConfigSO waveConfig) 
    {
        int groupSize = UnityEngine.Random.Range(waveConfig.MinGroupSize, waveConfig.MaxGroupSize);
            
        for (int i = 0; i < groupSize; i++)
        {
            Vector3 spawnPosition = _getRandomSpawnPosition();

            var enemy = Instantiate(waveConfig.EnemiesWithSpawnWeights.GetRandomItem(), spawnPosition, Quaternion.identity);
            enemy.SetTarget(_enemyTarget);
            enemy.SpawnableComponent.Spawn();

            Enemies.Add(enemy);
        }
    } 
    #endregion

    #region Enemy list control
    public bool TryGetClosestEnemy(Vector3 position, out Enemy closestEnemy) 
    {
        _clearNullEnemies();

        float minDistance = float.MaxValue;
        closestEnemy = null;

        foreach (var enemy in Enemies) 
        {
            if (!enemy.IsTargetable) continue;

            float distance = Vector3.Distance(position, enemy.transform.position);

            if (distance < minDistance) 
            {
                minDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy != null;
    }

    private void _clearNullEnemies()
    {
        Enemies = Enemies.Where((enemy) => enemy != null).ToList();
    }

    private void _clearEnemies()
    {
        foreach (var enemy in Enemies) 
        {
            enemy.Clean();
            Destroy(enemy.gameObject);
        }

        Enemies.Clear();
    }
    #endregion
}