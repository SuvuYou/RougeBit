using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controlls a set of upgradable components related to a single prefab or gameobject
/// </summary>
class UpgradablesBundleController : MonoBehaviour
{
    [SerializeField] List<BaseUpgradeValuesSetSO> _levels;

    [SerializeField] List<Upgradable> _upgradableComponents;

    private int _currentLevel = 0;
    private int _maxLevel => _levels.Count - 1;

    public bool IsUpgradable => _currentLevel < _maxLevel;

    private void Awake() => Reset();

    public void Reset() 
    {
        _currentLevel = 0;

        _loopUpgradeComponents(_currentLevel);
    } 

    public void Upgrade() 
    {
        _currentLevel++;

        if (_currentLevel > _maxLevel)
            _currentLevel = _maxLevel;

        _loopUpgradeComponents(_currentLevel);
    }

    public BaseUpgradeValuesSetSO GetNextLevelInfo() 
    {
        if (this.gameObject.IsPrefab())
        {
            return _levels[0];
        }

        return _levels[_currentLevel + 1];
    } 

    private void _loopUpgradeComponents(int level)
    {
        foreach (var upgradable in _upgradableComponents)
        {
            upgradable.UpgradeValues(_levels[level]);
        }
    }
}