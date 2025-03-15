using UnityEngine;
using UnityEngine.Events;

class GameManager : Singlton<GameManager>, IResettable
{
    public void Reset()
    {
        GameLevels.Init();
        _currentState = GameStates.None;
    }

    public enum GameStates { None, MainMenu, InGame, Shop, Paused, GameOverSuccess, GameOverFail }

    [field: SerializeField] public GameLevelsConfigSO GameLevels { get; private set; }

    private GameStates _currentState = GameStates.None;

    public UnityEvent OnRoundStarted;
    public UnityEvent OnRoundStopped;

    protected override void Awake() 
    {
        base.Awake();

        GameLevels.Init();
    }

    private void Start()
    {
        XPManager.Instance.OnLevelUp.AddListener(() => 
        {
            if (GameLevels.HasPassedLastLevel)
            {
                _setState(GameStates.GameOverSuccess);
            }
            else
            {
                _setState(GameStates.Shop);
            } 
        });

        EnterMainMenu();
    }

    public void StartGame() => _setState(GameStates.InGame);

    public void EndGame() => _setState(GameStates.GameOverFail);

    public void TryAgain() 
    {
        ResetGame();
        _setState(GameStates.InGame);
    } 

    public void EnterMainMenu() 
    {
        ResetGame();
        _setState(GameStates.MainMenu);
    }

    public void ResetGame() 
    {
        ResetManager.Instance.Reset();
    }

    #region State Management
    private void _setState(GameStates newState)
    {
        if (_currentState == newState) return;

        // Old state
        switch (_currentState)
        {
            case GameStates.Paused:
                UIManager.Instance.CloseWindow(UIManager.UIWindows.Pause);
                break;
            default:
                break;
        }

        // New State
        switch (newState)
        {
            case GameStates.MainMenu:
                _stopRound();
                UIManager.Instance.CloseAllWindows();
                UIManager.Instance.OpenWindow(UIManager.UIWindows.MainMenu);
                break;
            case GameStates.InGame:
                _startRound();
                UIManager.Instance.CloseAllWindows();
                UIManager.Instance.OpenWindow(UIManager.UIWindows.HUD);
                break;
            case GameStates.Shop:
                _stopRound();
                UIManager.Instance.OpenWindow(UIManager.UIWindows.Shop);
                break;
            case GameStates.Paused:
                UIManager.Instance.OpenWindow(UIManager.UIWindows.Pause);
                break;
            case GameStates.GameOverSuccess:
                _stopRound();
                UIManager.Instance.OpenWindow(UIManager.UIWindows.GameEndSuccess);
                break;
            case GameStates.GameOverFail:
                _stopRound();
                UIManager.Instance.OpenWindow(UIManager.UIWindows.GameEndFail);
                break;
        }

        _currentState = newState;
    }
    #endregion

    #region Wave Management
    private void _startRound()
    {
        OnRoundStarted?.Invoke();
        WaveManager.Instance.StartNextWave();
    }

    private void _stopRound()
    {
        OnRoundStopped?.Invoke();
        WaveManager.Instance.StopWave();
        ProjectileManager.Instance.ClearProjectiles();
        CollectablesManager.Instance.ClearCollectableItem();
    }
    #endregion
}