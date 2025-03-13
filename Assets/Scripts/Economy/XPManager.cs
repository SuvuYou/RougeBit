using System.Collections.Generic;
using UnityEngine;

class XPManager : MonoBehaviour
{
    [SerializeField] FloatRangeValue _currentXp;
    [SerializeField] List<FloatRangeValueConfigSO> _xpRequirementsLevels;

    public IntUnityEvent OnLevelUp;

    private int _maxLevel => _xpRequirementsLevels.Count;

    private int _currentLevel = 0;

    private void Awake() => _currentXp.SetConfig(_xpRequirementsLevels[_currentLevel]);

    public void AddXP(int xpAmount)
    {
        _currentXp.AddValue(xpAmount);
        _checkLevelUp();
    }

    private void _checkLevelUp()
    {
        if (_currentXp.Value >= _currentXp.Config.MaxValue)
        {
            _increaseLevel();
        }
    }

    private void _increaseLevel()
    {
        if (_currentLevel >= _maxLevel)
        {

            return;
        }

        _currentLevel += 1;
        _currentXp.SetConfig(_xpRequirementsLevels[_currentLevel]);

        // TODO: Reset the map here
        OnLevelUp?.Invoke(_currentLevel);
    }
}