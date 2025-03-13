using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Connectors/FloatRangeValueConfigSO")]
public class FloatRangeValueConfigSO : ScriptableObject
{
    public float DefaultValue;
    public float MinValue;
    public float MaxValue;  
}