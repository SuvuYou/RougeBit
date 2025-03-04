using System;
using System.Collections;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] protected Transform _character;
    [SerializeField] EntityMovementStats _movementStats;
    
    public bool PermaDisabled { get; private set; } = false;
    public bool Enabled { get; private set; } = true;

    public Vector3 Velocity { get; private set; }
    public Vector3 CurrentDirection { get; private set; }
    public Vector3 LastNonZeroDirection { get; private set; }

    public void MoveToDirection(Vector3 direction)
    {
        if (PermaDisabled || !Enabled) return;

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
        DisableMovement();

        StartCoroutine(_dashCoroutine(currentPositoin, targetPosition, duration, () => EnableMovement()));
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

    public void DisableMovement()
    {
        Velocity = Vector3.zero;
        Enabled = false;
    }

    public void EnableMovement()
    {
        Enabled = true;
    }

    public void DisableMovementPermanently() => PermaDisabled = true;
}
