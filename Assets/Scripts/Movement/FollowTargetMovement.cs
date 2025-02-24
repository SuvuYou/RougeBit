using UnityEngine;

public class FollowTargetMovement : CharacterMovement
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _minDistanceToTarget = 1f;
    [SerializeField] private float _maxDistanceToTarget = 2f;

    protected void SetTarget(Transform target)
    {
        _target = target;
    }

    void Update()
    {
        if (_target == null) return;
        
        var directionToTarget = _target.position - _character.position;

        Vector3 minDirection = (_target.position - directionToTarget.normalized * _minDistanceToTarget) - _character.position;
        Vector3 maxDirection = (_target.position - directionToTarget.normalized * _maxDistanceToTarget) - _character.position;

        var direction = Vector3.Dot(minDirection, maxDirection) < 0 ? Vector3.zero : maxDirection;

        MoveToDirection(direction);
    }
}
