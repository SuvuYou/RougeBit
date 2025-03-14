using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Connectors/FloatRangeValue")]
public class FloatRangeValue : ScriptableObject
{
    public float Value;
    public FloatRangeValueConfigSO Config;

    public Action<float> OnValueChanged;

    public void SetValue(float value) 
    {
        Value = Mathf.Clamp(value, Config.MinValue, Config.MaxValue);
        OnValueChanged?.Invoke(Value);
    }

    public void AddValue(float increase) => SetValue(Value + increase); 

    public void ReduceValue(float decrease) => SetValue(Value - decrease);

    public void Init()
    {
        OnValueChanged = null;
    }

    public void SetConfig(FloatRangeValueConfigSO config) 
    {
        Config = config;
        SetValue(config.DefaultValue);
    }
}
