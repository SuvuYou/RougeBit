using System.Collections.Generic;
using UnityEngine;

class UpgradeManager : MonoBehaviour
{
    [SerializeField] List<BaseUpgradeValuesSetSO> _levels;

    [SerializeField] List<Upgradable<MonoBehaviour>> _upgradableComponents;

    private int _currentLevel;
    private int _maxLevel => _levels.Count - 1;

    public bool IsUpgradable => _currentLevel < _maxLevel;

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

    private void _loopUpgradeComponents(int level)
    {
        foreach (var upgradable in _upgradableComponents)
        {
            upgradable.UpgradeValues(_levels[level]);
        }
    }
}