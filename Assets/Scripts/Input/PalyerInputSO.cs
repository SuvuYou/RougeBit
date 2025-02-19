using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PalyerInputSO")]
public class PalyerInputSO : ScriptableObject
{
    [field: SerializeField] public Vector2 MovementInput { get; private set; }
    public Vector2 LastNonZeroMovementInput { get; private set; }

    public void SetMovementInput(Vector2 newInput)
    {
        MovementInput = newInput;

        if (newInput.magnitude > 0) LastNonZeroMovementInput = newInput;
    }
}
