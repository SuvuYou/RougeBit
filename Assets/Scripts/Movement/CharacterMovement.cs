using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] protected Transform _character;
    [SerializeField] EntityMovementStats _movementStats;
    
    public bool Enabled { get; private set; } = true;

    public Vector3 Velocity { get; private set; }
    public Vector3 CurrentDirection { get; private set; }
    public Vector3 LastNonZeroDirection { get; private set; }

    public void MoveToDirection(Vector3 direction)
    {
        if (!Enabled) return;

        CurrentDirection = direction;

        if (direction.magnitude > _movementStats.DragThreshold)
        {
            Velocity += direction.normalized * _movementStats.AccelerationSpeed  * Time.deltaTime;

            LastNonZeroDirection = direction;
        }
        else
        {
            Velocity -= Velocity * _movementStats.Drag  * Time.deltaTime;
        }

        if (direction.magnitude <= _movementStats.DragThreshold && Velocity.magnitude < _movementStats.DragThreshold)
        {
            Velocity = Vector3.zero;
        }

        Velocity = Vector3.ClampMagnitude(Velocity, _movementStats.MaxMovementSpeed);

        _character.position += Velocity * Time.deltaTime;
    }

    public void DisableMovement()
    {
        Velocity = Vector3.zero;
        Enabled = false;
    }

    public void EnableMovement()
    {
        Enabled = true;
    }
}
