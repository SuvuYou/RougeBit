using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Attacks/PlantMineAttackStatsSO")]
public class PlantMineAttackStatsSO : ScriptableObject
{
    public Mine[] MinePrefabs; 

    public float PlantDistance = 15f;

    [Range(1, 12)] public int NumberOfMines = 1;
}
