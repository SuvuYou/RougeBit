using System;
using System.Collections;
using UnityEngine;

[Serializable]
[Flags]
public enum MovementLock {
    None            = 0,
    AttackStunned   = 1 << 0,
    Attack          = 1 << 1,
    Knockback       = 1 << 2,
    Dash            = 1 << 3,
    Dead            = 1 << 4,
    Timed           = 1 << 5
}

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] protected Transform _character;
    [SerializeField] EntityMovementStats _movementStats;

        // Collision settings
    [SerializeField] float collisionRadius = 0.5f; // adjust as needed
    [SerializeField] LayerMask collisionLayer;      // set this to the layer(s) that represent obstacles


    private MovementLock _activeMovementLocks = MovementLock.None;

    public bool Enabled => _activeMovementLocks == MovementLock.None;

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

        Vector3 predictedPosition = _character.position + Velocity * Time.deltaTime;

        if (Physics.SphereCast(_character.position, collisionRadius, Velocity.normalized, out RaycastHit hit, Velocity.magnitude * Time.deltaTime, collisionLayer))
        {
            if (Vector3.Dot(direction.normalized, hit.normal) > -0.94f)
            {
                Vector3 slideVelocity = Vector3.ProjectOnPlane(Velocity, hit.normal);
                predictedPosition = _character.position + slideVelocity * Time.deltaTime;
                Velocity = slideVelocity; 
            }
            else
            {
                Velocity = Vector3.zero;
                predictedPosition = _character.position;
            }
        }

        _character.position = predictedPosition;
    }

    private bool IsCollisionAtPosition(Vector3 position)
    {
        Collider[] hitColliders = Physics.OverlapSphere(position, collisionRadius, collisionLayer);

        return hitColliders.Length > 0;
    }

    public void Dash(Vector3 currentPositoin, Vector3 targetPosition, float duration)
    {
        AddMovementLock(MovementLock.Dash);

        StartCoroutine(_dashCoroutine(currentPositoin, targetPosition, duration, () => RemoveMovementLock(MovementLock.Dash)));
    }

    private IEnumerator _dashCoroutine(Vector3 currentPosition, Vector3 targetPosition, float duration, Action onComplete = null)
    {
        float elapsedTime = 0f, smoothT;

        while (elapsedTime < duration)
        {
            smoothT = elapsedTime / duration;
            smoothT = smoothT * smoothT * (3f - 2f * smoothT);

            Vector3 newPosition = Vector3.Lerp(currentPosition, targetPosition, smoothT);
            
            if (!IsCollisionAtPosition(newPosition))
            {
                _character.position = newPosition;
            }
            else
            {
                break;
            }
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _character.position = targetPosition;

        onComplete?.Invoke();
    }

    public void AddMovementLock(MovementLock lockType) 
    {
        Velocity = Vector3.zero;
        _activeMovementLocks |= lockType;
    } 

    public void RemoveMovementLock(MovementLock lockType) => _activeMovementLocks &= ~lockType;

    public void LockMovementByAttack() => AddMovementLock(MovementLock.Attack);
    public void LockMovementByDead() => AddMovementLock(MovementLock.Dead);

    public void UnlockMovementByAttack() => RemoveMovementLock(MovementLock.Attack);
    public void UnlockMovementByDead() => RemoveMovementLock(MovementLock.Dead);
}
