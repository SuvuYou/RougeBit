using UnityEngine;

public class RotateSpritesetByDirection : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _upSprite, _rightSprite, _downSprite;
    [SerializeField] private Sprite _downRightSprite30, _downRightSprite45, _downRightSprite60;
    [SerializeField] private Sprite _upRightSprite30, _upRightSprite45, _upRightSprite60;

    [SerializeField] private Transform _targetTransform;

    private struct DirectionSprite
    {
        public Vector2 Direction;
        public Sprite Sprite;
    }

    private DirectionSprite[] _directionSprites;

    private void Awake()
    {
        _directionSprites = new DirectionSprite[]
        {
            new DirectionSprite { Direction = Vector2.up, Sprite = _upSprite },
            new DirectionSprite { Direction = Vector2.right, Sprite = _rightSprite },
            new DirectionSprite { Direction = Vector2.down, Sprite = _downSprite },
            new DirectionSprite { Direction = new Vector2(0.866f, -0.5f), Sprite = _downRightSprite30 },
            new DirectionSprite { Direction = new Vector2(0.707f, -0.707f), Sprite = _downRightSprite45 },
            new DirectionSprite { Direction = new Vector2(0.5f, -0.866f), Sprite = _downRightSprite60 },
            new DirectionSprite { Direction = new Vector2(0.866f, 0.5f), Sprite = _upRightSprite30 },
            new DirectionSprite { Direction = new Vector2(0.707f, 0.707f), Sprite = _upRightSprite45 },
            new DirectionSprite { Direction = new Vector2(0.5f, 0.866f), Sprite = _upRightSprite60 }
        };
    }

    private void Update() => RotateToDirection(_targetTransform.position - transform.position);

    public void RotateToDirection(Vector3 direction)
    {
        if (direction == Vector3.zero)
            return;

        direction = direction.normalized;

        _spriteRenderer.flipX = direction.x < 0;
        if (direction.x < 0) direction.x *= -1;

        float bestAngle = float.MaxValue;
        Sprite bestSprite = _rightSprite;

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
