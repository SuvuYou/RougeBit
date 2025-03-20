using UnityEngine;

public class RotateTowardsStrategy : RotationStrategyBase
{
    [Header("References")]
    [SerializeField] private Transform _pivotPoint;

    private Vector3 _targetPosition;

    public override void SetTargetPosition(Vector3 target) => _targetPosition = target;

    public override void Rotate() => _rotateToDirection(_targetPosition - transform.position);

    private void _rotateToDirection(Vector3 direction)
    {
        if (direction == Vector3.zero)
            return;

        direction = direction.normalized;

        float angle = Vector3.Angle(direction, Vector3.right);

        if (direction.y < 0)
            angle = -angle;

        _pivotPoint.rotation = Quaternion.Euler(0, 0, angle);
    }
}
