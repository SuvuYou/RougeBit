using UnityEngine;

public class EnemyFollowTargetMovement : FollowTargetMovement
{
    [SerializeField] private Enemy _enemyComponent;

    void Start()
    {
        SetTarget(_enemyComponent.Target.transform);
    }
}
