using UnityEngine;

public class EnemyFollowTargetMovement : CharacterMovement
{
    [SerializeField] private Enemy _enemyComponent;
    [SerializeField] private float _distanceToTarget = 1f;

    private Transform _target;

    void Start()
    {
        _target = _enemyComponent.Target.transform;
    }

    void Update()
    {
        if (_target == null) return;

        var directionToTarget = _target.position - _character.position;

        var direction = (_target.position - directionToTarget.normalized * _distanceToTarget) - _character.position;

        MoveToDirection(direction);
    }
}
