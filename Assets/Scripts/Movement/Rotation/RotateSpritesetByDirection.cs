using UnityEngine;

public class RotateSpritesetByDirection : RotationStrategyBase
{
    [Header("References")]
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [Header("Directions")]
    [SerializeField] private WeaponRotationSpritesetSO _rotations;

    private Vector3 _targetPosition;

    private struct DirectionSprite
    {
        public Vector2 Direction;
        public Sprite Sprite;
    }

    private DirectionSprite[] _directionSprites;

    public override void SetTargetPosition(Vector3 target) => _targetPosition = target;

    private void Awake()
    {
        _directionSprites = new DirectionSprite[]
        {
            new() { Direction = Vector2.up, Sprite = _rotations.UpSprite },
            new() { Direction = Vector2.right, Sprite = _rotations.RightSprite },
            new() { Direction = Vector2.down, Sprite = _rotations.DownSprite },
            new() { Direction = new Vector2(0.866f, -0.5f), Sprite = _rotations.DownRightSprite30 },
            new() { Direction = new Vector2(0.707f, -0.707f), Sprite = _rotations.DownRightSprite45 },
            new() { Direction = new Vector2(0.5f, -0.866f), Sprite = _rotations.DownRightSprite60 },
            new() { Direction = new Vector2(0.866f, 0.5f), Sprite = _rotations.UpRightSprite30 },
            new() { Direction = new Vector2(0.707f, 0.707f), Sprite = _rotations.UpRightSprite45 },
            new() { Direction = new Vector2(0.5f, 0.866f), Sprite = _rotations.UpRightSprite60 }
        };
    }

    public override void Rotate() => _rotateToDirection(_targetPosition - transform.position);

    private void _rotateToDirection(Vector3 direction)
    {
        if (direction == Vector3.zero)
            return;

        direction = direction.normalized;

        _spriteRenderer.flipX = direction.x < 0;
        if (direction.x < 0) direction.x *= -1;

        float bestAngle = float.MaxValue;
        Sprite bestSprite = _rotations.UpSprite;

        foreach (var entry in _directionSprites)
        {
            float angle = Vector3.Angle(direction, entry.Direction);
            if (angle < bestAngle)
            {
                bestAngle = angle;
                bestSprite = entry.Sprite;
            }
        }

        _spriteRenderer.sprite = bestSprite;
    }
}
