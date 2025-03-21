using UnityEngine;
using UnityEngine.Events;

class XPManager : Singlton<XPManager>, IResettable
{
    public void Reset() 
    {
        _currentXp.SetConfig(GameManager.Instance.GameLevels.Levels[0].XPRequirement);
    }

    [SerializeField] FloatRangeValue _currentXp;

    public UnityEvent OnLevelUp;

    private void Start() 
    {
        _currentXp.SetConfig(GameManager.Instance.GameLevels.GetCurrentLevel().XPRequirement);
    } 

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
        GameManager.Instance.GameLevels.NextLevel();

        _currentXp.SetConfig(GameManager.Instance.GameLevels.GetCurrentLevel().XPRequirement);

        OnLevelUp?.Invoke();
    }
}