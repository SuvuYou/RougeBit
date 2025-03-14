using UnityEngine;

class GameManager : Singlton<GameManager>
{
    [field: SerializeField] public GameLevelsConfigSO GameLevels { get; private set; }

    protected override void Awake() 
    {
        base.Awake();

        GameLevels.Init();
    }

    private void Start()
    {
        XPManager.Instance.OnLevelUp.AddListener(() => _stopRound());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _startRound();
        }
    }

    private void _startRound()
    {
        WaveManager.Instance.StartNextWave();
    }

    private void _stopRound()
    {
        WaveManager.Instance.StopWave();
        ProjectileManager.Instance.ClearProjectiles();
        CollectablesManager.Instance.ClearCollectableItem();
    }
}