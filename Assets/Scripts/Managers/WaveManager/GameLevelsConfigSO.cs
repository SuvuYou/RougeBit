using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameLevelsConfig", menuName = "ScriptableObjects/Level Design/GameLevelsConfig")]
class GameLevelsConfigSO : ScriptableObject
{
    public List<WaveConfigSO> Levels;

    public WaveConfigSO GetCurrentLevel() => Levels[CurrentLevelNumber];

    private int _currentLevel = 0;
    public int CurrentLevelNumber { get => _currentLevel > MaxLevel ? MaxLevel : _currentLevel; private set => _currentLevel = value; }

    [HideInInspector]  
    public int MaxLevel = 0;

    public bool IsLastLevel => _currentLevel == MaxLevel;
    public bool HasPassedLastLevel => _currentLevel > MaxLevel;
    
    public void NextLevel() => CurrentLevelNumber++;

    public void Init()
    {
        CurrentLevelNumber = 0;
        MaxLevel = Levels.Count - 1;
    }
}