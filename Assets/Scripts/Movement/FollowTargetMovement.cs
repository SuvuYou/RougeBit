using UnityEngine;

public class FollowTargetMovement : CharacterMovement
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _distanceToTarget = 1f;

    void Update()
    {
        var directionToTarget = _target.position - _character.position;

        var direction = (_target.position - directionToTarget.normalized * _distanceToTarget) - _character.position;

        MoveToDirection(direction);
    }
}
