using UnityEngine;

[CreateAssetMenu(fileName = "WaveConfigSO", menuName = "ScriptableObjects/Level Design/WaveConfigSO")]
class WaveConfigSO : ScriptableObject
{
    public float MinGroupSpawnTimeout;
    public float MaxGroupSpawnTimeout;
    public int MinGroupSize;
    public int MaxGroupSize;
    public WeightedDictionary<Enemy> EnemiesWithSpawnWeights;
    public FloatRangeValueConfigSO XPRequirement;
}