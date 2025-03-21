using UnityEngine;
using UnityEngine.Events;

public class Enemy : Target, ICleanable
{
    [field: SerializeField] public Spawnable SpawnableComponent { get; private set; }

    public UnityEvent OnClean;
    
    public Target Target { get; private set; }

    public void SetTarget(Target target) => Target = target;

    public void Clean() { OnClean?.Invoke(); }
}