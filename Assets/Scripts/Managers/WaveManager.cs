using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class WaveManager : Singlton<WaveManager>
{
    private enum WaveManagerStates 
    {
        Idle,
        Spawning,
        OnTimeout
    } 

    public List<Enemy> Enemies { get; private set; } = new();

    [SerializeField] private GameObject _enemyTarget;

    [SerializeField] private float _safeRadious = 5f;

    [SerializeField] private Vector2 _lowerSpawnBounds;
    [SerializeField] private Vector2 _upperSpawnBounds; 

    [SerializeField] private WeightedDictionary<Enemy> _enemiesWithSpawnWeights; 

    public event Action OnWaveFinished;

    private int _waveNumber = 0;

    private Wave _currentWave;

    private WaveManagerStates _currentState = WaveManagerStates.Idle;

    private Timer _waveTimer = new (0f);
    private Timer _timeoutTimer = new (0f);

    private void Update()
    {
        switch(_currentState)
        {
            case WaveManagerStates.Idle:
                break;
            case WaveManagerStates.Spawning:
                _checkIsWaveFinished();
                break;
            case WaveManagerStates.OnTimeout:
                _checkIsTimeoutFinished();
                _checkIsWaveFinished();
                break;
        }
    }

    public void StartNextWave()
    {
        _startWave(_generateNewWave(_waveNumber + 1));
    }

    private Wave _generateNewWave(int waveNumber)
    {
        return new Wave()
        {
            WaveDurationTime = 30,
            MinGroupSpawnTimeout = 2,
            MaxGroupSpawnTimeout = 10,
            MinGroupSize = 1,
            MaxGroupSize = 3,
            EnemiesWithSpawnWeights = _enemiesWithSpawnWeights
        };
    }

    private void _startWave(Wave wave)
    {
        _currentWave = wave;

        _waveTimer.SetBaseTime(wave.WaveDurationTime);
        _waveTimer.Start();

        _waveNumber++;

        _switchStates(WaveManagerStates.Spawning);
    }

    private void _checkIsWaveFinished() 
    {   
        _waveTimer.Update(Time.deltaTime);

        if (_waveTimer.IsFinished) 
        {
            _waveTimer.Stop();
            _switchStates(WaveManagerStates.Idle);
            OnWaveFinished?.Invoke();
        }
    } 

    private void _checkIsTimeoutFinished()
    {
        _timeoutTimer.Update(Time.deltaTime);

        if (_timeoutTimer.IsFinished) 
        {
            _timeoutTimer.Stop();
            _switchStates(WaveManagerStates.Spawning);
        }     
    }

    private void _switchStates(WaveManagerStates newState)
    {   
        _currentState = newState;

        if (_currentState == WaveManagerStates.Spawning)
        {
            int groupSize = UnityEngine.Random.Range(_currentWave.MinGroupSize, _currentWave.MaxGroupSize);
            
            for (int i = 0; i < groupSize; i++)
            {
                Vector3 spawnPosition = _getRandomSpawnPosition();

                var enemy = Instantiate(_currentWave.EnemiesWithSpawnWeights.GetRandomItem(), spawnPosition, Quaternion.identity);
                enemy.SetTarget(_enemyTarget);

                Enemies.Add(enemy);
            }
        }

        if (_currentState == WaveManagerStates.OnTimeout)
        {
            _timeoutTimer.SetBaseTime(UnityEngine.Random.Range(_currentWave.MinGroupSpawnTimeout, _currentWave.MaxGroupSpawnTimeout));
            _timeoutTimer.Start();   
        }
    }

    private Vector3 _getRandomSpawnPosition()
    {
        Vector3 spawnPosition = Vector3.zero;

        do 
        {
            spawnPosition = new (UnityEngine.Random.Range(_lowerSpawnBounds.x, _upperSpawnBounds.x), UnityEngine.Random.Range(_lowerSpawnBounds.y, _upperSpawnBounds.y), 0);
        } while (Vector3.Distance(_enemyTarget.transform.position, spawnPosition) <= _safeRadious);

        return spawnPosition;
    }

    public bool TryGetClosestEnemy(Vector3 position, out Enemy closestEnemy) 
    {
        _clearNullEnemies();

        float minDistance = float.MaxValue;
        closestEnemy = null;

        foreach (var enemy in Enemies) 
        {
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
}

struct Wave
{
    public int WaveDurationTime;
    public int MinGroupSpawnTimeout;
    public int MaxGroupSpawnTimeout;
    public int MinGroupSize;
    public int MaxGroupSize;
    public WeightedDictionary<Enemy> EnemiesWithSpawnWeights;

    public Wave(int waveDurationTime, int minGroupSpawnTimeout, int maxGroupSpawnTimeout, int minGroupSize, int maxGroupSize, WeightedDictionary<Enemy> enemiesWithSpawnWeights)
    {
        WaveDurationTime = waveDurationTime;
        MinGroupSpawnTimeout = minGroupSpawnTimeout;
        MaxGroupSpawnTimeout = maxGroupSpawnTimeout;
        MinGroupSize = minGroupSize;
        MaxGroupSize = maxGroupSize;
        EnemiesWithSpawnWeights = enemiesWithSpawnWeights;
    }
}