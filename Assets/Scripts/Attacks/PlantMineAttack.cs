using System;
using UnityEngine;

class PlantMineAttack : BaseAttack
{
    public override void UpgradeValues(BaseUpgradeValuesSetSO ovrrideValues)
    {
        base.UpgradeValues(ovrrideValues);

        _stats = ovrrideValues.PlantMineAttackStats;
    }

    [SerializeField] private PlantMineAttackStatsSO _stats;

    protected override void _handleIsReadyForAttack(Action performAttackOrAim) 
    {
        performAttackOrAim();
    } 

    protected override void _handleAttack(Action finishAttack, Action cancelAttack, Action cancelReloadAttack)
    {
        _spawnMines();
        finishAttack();
    }

    private void _spawnMines()
    {
        for (int i = 0; i < _stats.NumberOfMines; i++)
        {
            var randomPosition = _attacker.transform.position + new Vector3(UnityEngine.Random.Range(-_stats.PlantDistance, _stats.PlantDistance), UnityEngine.Random.Range(-_stats.PlantDistance, _stats.PlantDistance), 0);

            _plantMine(randomPosition);
        }
    }

    private void _plantMine(Vector3 position)
    {
        Instantiate(_getRandomMinePrefab(), position, Quaternion.identity);
    }

    private Mine _getRandomMinePrefab() => _stats.MinePrefabs[UnityEngine.Random.Range(0, _stats.MinePrefabs.Length - 1)];
}