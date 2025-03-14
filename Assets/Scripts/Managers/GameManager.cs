using UnityEngine;

class GameManager : Singlton<GameManager>
{
    [field: SerializeField] public GameLevelsConfigSO GameLevels { get; private set; }

    [SerializeField] private ShopController _shop;

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
            StartRound();
        }
    }

    public void StartRound()
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