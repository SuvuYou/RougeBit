using UnityEngine;

public class Enemy : Target
{
    [field: SerializeField] public Spawnable SpawnableComponent { get; private set; }
    
    public Target Target { get; private set; }

    public void SetTarget(Target target) => Target = target;
}