using UnityEngine;

public class EnemyFollowTargetMovement : FollowTargetMovement
{
    [SerializeField] private Enemy _enemyComponent;

    void Start()
    {
        if (_enemyComponent?.Target == null) return;
        
        SetTarget(_enemyComponent.Target.transform);
    }
}
