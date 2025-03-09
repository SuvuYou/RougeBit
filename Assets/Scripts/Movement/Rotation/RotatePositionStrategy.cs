using UnityEngine;

public class RotatePositionStrategy : RotationStrategyBase
{
    [SerializeField] private Transform _rotatedTransform;
    [SerializeField] private Transform _centerTransform;
    [SerializeField] private float _distanceFromCenter = 2f;

    private Vector3 _targetPosition;

    public override void SetTargetPosition(Vector3 target) => _targetPosition = target;

    public override void Rotate() => _rotatePositionAroundCenter(_targetPosition - _centerTransform.position);

    private void _rotatePositionAroundCenter(Vector3 direction)
    {
        if (direction == Vector3.zero)
            return;

        _rotatedTransform.position = _centerTransform.position + direction.normalized * _distanceFromCenter;
    }
}
