using UnityEngine;

public class SnapToRotationByDirection : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _pivotPoint;

    private Target _target;

    private struct DirectionRotation
    {
        public Vector2 Direction;
        public float RotationZ;
    }

    private DirectionRotation[] _directionRotations;

    public void SetTarget(Target target) => _target = target;

    private void Awake()
    {
        _directionRotations = new DirectionRotation[]
        {
            new() { Direction = Vector2.up, RotationZ = 90 },
            new() { Direction = Vector2.right, RotationZ = 0 },
            new() { Direction = Vector2.down, RotationZ = -90 },
            new() { Direction = Vector2.left, RotationZ = -180 },

            new() { Direction = new Vector2(0.866f, -0.5f), RotationZ = -30 },
            new() { Direction = new Vector2(0.707f, -0.707f), RotationZ = -45 },
            new() { Direction = new Vector2(0.5f, -0.866f), RotationZ = -60 },
            new() { Direction = new Vector2(0.866f, 0.5f), RotationZ = 30 },
            new() { Direction = new Vector2(0.707f, 0.707f), RotationZ = 45 },
            new() { Direction = new Vector2(0.5f, 0.866f), RotationZ = 60 },

            new() { Direction = new Vector2(-0.866f, -0.5f), RotationZ = -150 },
            new() { Direction = new Vector2(-0.707f, -0.707f), RotationZ = -135 },
            new() { Direction = new Vector2(-0.5f, -0.866f), RotationZ = -120 },
            new() { Direction = new Vector2(-0.866f, 0.5f), RotationZ = 150 },
            new() { Direction = new Vector2(-0.707f, 0.707f), RotationZ = 135 },
            new() { Direction = new Vector2(-0.5f, 0.866f), RotationZ = 120 }
        };
    }

    public void Rotate() => _rotateToDirection(_target.transform.position - transform.position);

    private void _rotateToDirection(Vector3 direction)
    {
        if (direction == Vector3.zero)
            return;

        direction = direction.normalized;

        float bestAngle = float.MaxValue;
        float closestRotationZ = 0;

        foreach (var entry in _directionRotations)
        {
            float angle = Vector3.Angle(direction, entry.Direction);
            if (angle < bestAngle)
            {
                bestAngle = angle;
                closestRotationZ = entry.RotationZ;
            }
        }

        _pivotPoint.rotation =  Quaternion.Euler(0, 0, closestRotationZ);
    }
}
