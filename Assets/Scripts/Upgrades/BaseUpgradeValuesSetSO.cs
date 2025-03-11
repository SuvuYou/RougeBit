using UnityEngine;

[CreateAssetMenu(fileName = "BaseUpgradeValuesSet", menuName = "ScriptableObjects/Upgrades/BaseUpgradeValuesSet")]
public class BaseUpgradeValuesSetSO : ScriptableObject
{
    [Header("Display Info")]
    public string Title = "Attack Name";

    [Header("Attacks")]
    public _BaseAttackStatsSO BaseAttackStats;
    public RocketLaunchAttackStatsSO RocketLaunchAttackStats;
    public RangedAttackStatsSO RangedAttackStats;
    public PlantMineAttackStatsSO PlantMineAttackStats;
    public LaserBeamAttackStatsSO LaserBeamAttackStats;
    public DashAttackOnCollisionStatsSO DashAttackOnCollisionStats;
    public AttackOnCollisionStatsSO AttackOnCollisionStats;
    public AirStrikeAttackStatsSO AirStrikeAttackStats;
    public SlashAttackStatsSO SlashAttackStats;

    [Header("Rotation Controller")]
    public RotationControllerUpgradeValuesSO RotationControllerUpgradeValues;
}
