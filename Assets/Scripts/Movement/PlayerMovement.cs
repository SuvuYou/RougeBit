using UnityEngine;

// [SerializeField] private float _maxMovementSpeed = 10f;
// [SerializeField] private float _accelerationSpeed = 250f;
// [SerializeField] private float _drag = 15f;
// [SerializeField] private float _dragThreshold = 0.1f;

public class PlayerMovement : CharacterMovement
{
    [SerializeField] private PalyerInputSO _movementDirectionSO;

    void Update()
    {
        MoveToDirection(_movementDirectionSO.MovementInput);
    }
}
