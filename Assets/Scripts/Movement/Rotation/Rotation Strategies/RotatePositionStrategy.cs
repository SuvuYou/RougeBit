using UnityEngine;

public class RotatePositionStrategy : RotationStrategyBase
{
    [SerializeField] private Transform _pivotPoint;
    [SerializeField] private Transform _rotatedTransform;
    [SerializeField] private float _distanceFromCenter = 2f;

    private Vector3 _targetPosition;

    public override void SetPivotPoint(Transform pivotPoint)
    {
        _pivotPoint = pivotPoint;
    }

    public override void SetTargetPosition(Vector3 target) => _targetPosition = target;

    public override void Rotate() => _rotatePositionAroundCenter(_targetPosition - _pivotPoint.position);

    private void _rotatePositionAroundCenter(Vector3 direction)
    {
        if (direction == Vector3.zero)
            return;

        _rotatedTransform.position = _pivotPoint.position + direction.normalized * _distanceFromCenter;
    }
}
