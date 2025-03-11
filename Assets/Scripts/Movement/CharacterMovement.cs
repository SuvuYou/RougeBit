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

        _character.position += Velocity * Time.deltaTime;
    }

    public void Dash(Vector3 currentPositoin, Vector3 targetPosition, float duration)
    {
        AddMovementLock(MovementLock.Dash);

        StartCoroutine(_dashCoroutine(currentPositoin, targetPosition, duration, () => RemoveMovementLock(MovementLock.Dash)));
    }

    private IEnumerator _dashCoroutine(Vector3 currentPositoin, Vector3 targetPosition, float duration, Action onComplete = null)
    {
        float elapsedTime = 0f, smoothT;

        while (elapsedTime < duration)
        {
            smoothT = elapsedTime / duration;
            smoothT = smoothT * smoothT * (3f - 2f * smoothT);

            _character.position = Vector3.Lerp(currentPositoin, targetPosition, smoothT);
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
